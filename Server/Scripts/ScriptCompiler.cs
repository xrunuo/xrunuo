using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace Server
{
	public class ScriptCompiler
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static List<Library> m_Libraries;

		public static Library[] Libraries
		{
			get { return m_Libraries.ToArray(); }
		}

		/// <summary>
		/// Find a loaded library by its name.
		/// </summary>
		/// <param name="name">The name of the library.</param>
		/// <returns></returns>
		public static Library GetLibrary( string name )
		{
			return m_Libraries.FirstOrDefault( lib => lib.Name == name );
		}

		private static List<String> m_AdditionalReferences;

		private static string[] GetReferenceAssemblies()
		{
			var assemblies = new List<string>();

			var path = Path.Combine( Core.Config.ConfigDirectory, "Assemblies.cfg" );

			if ( File.Exists( path ) )
			{
				var lines = File.ReadLines( path )
					.Where( line => line.Length > 0 && !line.StartsWith( "#" ) );

				assemblies.AddRange( lines );
			}

			assemblies.Add( Core.ExePath );
			assemblies.AddRange( m_AdditionalReferences );

			return assemblies.ToArray();
		}

		private static Dictionary<string, DateTime> ReadStampFile( string filename )
		{
			if ( !File.Exists( filename ) )
				return null;

			var fs = new FileStream( filename, FileMode.Open, FileAccess.Read );
			var br = new BinaryReader( fs );

			int version = br.ReadInt32();
			if ( version != 1 )
				return null;

			var stamps = new Dictionary<string, DateTime>();

			uint count = br.ReadUInt32();
			for ( uint i = 0; i < count; i++ )
			{
				string fn = br.ReadString();
				long ticks = br.ReadInt64();
				stamps[fn] = new DateTime( ticks );
			}

			br.Close();
			fs.Close();

			return stamps;
		}

		private static void WriteStampFile( string filename, Dictionary<string, DateTime> stamps )
		{
			var fs = new FileStream( filename, FileMode.OpenOrCreate, FileAccess.Write );
			var bw = new BinaryWriter( fs );
			bw.Write( (int) 1 );

			bw.Write( (uint) stamps.Count );
			foreach ( var kvp in stamps )
			{
				bw.Write( kvp.Key );
				bw.Write( (long) kvp.Value.Ticks );
			}

			bw.Close();
			fs.Close();
		}

		private static bool CheckStamps( Dictionary<string, DateTime> files, string stampFile )
		{
			var stamps = ReadStampFile( stampFile );
			if ( stamps == null )
				return false;

			foreach ( var kvp in files )
			{
				var filename = kvp.Key;
				if ( !stamps.ContainsKey( filename ) )
					return false;

				var newStamp = kvp.Value;
				var oldStamp = stamps[filename];

				if ( oldStamp != newStamp )
					return false;

				stamps.Remove( filename );
			}

			return stamps.Count == 0;
		}

		private static CompilerResults CompileCSScripts( ICollection<string> fileColl, string assemblyFile,
			Configuration.Library libConfig, bool debug )
		{
			var provider = new CSharpCodeProvider();
#pragma warning disable 618
			var compiler = provider.CreateCompiler();
#pragma warning restore 618

			string[] files;

			log.Info( "Compiling library {0}, {1} C# sources", libConfig.Name, fileColl.Count );

			string tempFile = compiler.GetType().FullName == "Mono.CSharp.CSharpCodeCompiler"
				? Path.GetTempFileName()
				: null;

			if ( tempFile == String.Empty )
				tempFile = null;

			if ( tempFile == null )
			{
				files = new string[fileColl.Count];
				fileColl.CopyTo( files, 0 );
			}
			else
			{
				/* to prevent an "argument list too long" error, we
				   write a list of file names to a temporary file
				   and add them with @filename */
				var w = new StreamWriter( tempFile, false );
				foreach ( string file in fileColl )
				{
					w.Write( "\"" + file + "\" " );
				}
				w.Close();

				files = new string[0];
			}

			var parms = new CompilerParameters( GetReferenceAssemblies(), assemblyFile, debug );
			if ( tempFile != null )
				parms.CompilerOptions += "@" + tempFile;

			if ( libConfig.WarningLevel >= 0 )
				parms.WarningLevel = libConfig.WarningLevel;

			CompilerResults results = null;
			try
			{
				results = compiler.CompileAssemblyFromFileBatch( parms, files );
			}
			catch ( System.ComponentModel.Win32Exception e )
			{
				/* from WinError.h:
				 * #define ERROR_FILE_NOT_FOUND 2L
				 * #define ERROR_PATH_NOT_FOUND 3L
				 */
				if ( e.NativeErrorCode == 2 || e.NativeErrorCode == 3 )
				{
					log.Fatal( "Could not find the compiler - are you sure MCS is installed? (on Debian, try: apt-get install mono-mcs)" );
					Environment.Exit( 2 );
				}
				else
				{
					throw e;
				}
			}

			if ( tempFile != null )
				File.Delete( tempFile );

			m_AdditionalReferences.Add( assemblyFile );

			Display( results );

			return results;
		}

		public static void Display( CompilerResults results )
		{
			if ( results.Errors.Count > 0 )
			{
				var errors = new Dictionary<string, List<CompilerError>>( results.Errors.Count, StringComparer.OrdinalIgnoreCase );
				var warnings =
					new Dictionary<string, List<CompilerError>>( results.Errors.Count, StringComparer.OrdinalIgnoreCase );

				foreach ( CompilerError e in results.Errors )
				{
					var file = e.FileName;

					// Rediculous. FileName is null if the warning/error is internally generated in csc.
					if ( string.IsNullOrEmpty( file ) )
					{
						Console.WriteLine( "ScriptCompiler: {0}: {1}", e.ErrorNumber, e.ErrorText );
						continue;
					}

					var table = ( e.IsWarning ? warnings : errors );

					List<CompilerError> list = null;
					table.TryGetValue( file, out list );

					if ( list == null )
						table[file] = list = new List<CompilerError>();

					list.Add( e );
				}

				if ( errors.Count > 0 )
					log.Error( "compilation failed ({0} errors, {1} warnings)", errors.Count, warnings.Count );
				else
					log.Info( "compilation done ({0} errors, {1} warnings)", errors.Count, warnings.Count );

				string scriptRoot = Path.GetFullPath(
					Path.Combine( Core.BaseDirectory, "src" + Path.DirectorySeparatorChar ) );
				Uri scriptRootUri = new Uri( scriptRoot );

				if ( Core.Debug )
				{
					Utility.PushColor( ConsoleColor.Yellow );

					if ( warnings.Count > 0 )
						Console.WriteLine( "Warnings:" );

					foreach ( KeyValuePair<string, List<CompilerError>> kvp in warnings )
					{
						string fileName = kvp.Key;
						List<CompilerError> list = kvp.Value;

						string fullPath = Path.GetFullPath( fileName );
						string usedPath = Uri.UnescapeDataString( scriptRootUri.MakeRelativeUri( new Uri( fullPath ) ).OriginalString );

						Console.WriteLine( " + {0}:", usedPath );

						Utility.PushColor( ConsoleColor.DarkYellow );

						foreach ( CompilerError e in list )
							Console.WriteLine( "    {0}: Line {1}: {3}", e.ErrorNumber, e.Line, e.Column, e.ErrorText );

						Utility.PopColor();
					}

					Utility.PopColor();
				}

				Utility.PushColor( ConsoleColor.Red );

				if ( errors.Count > 0 )
					Console.WriteLine( "Errors:" );

				foreach ( var kvp in errors )
				{
					string fileName = kvp.Key;
					List<CompilerError> list = kvp.Value;

					string fullPath = Path.GetFullPath( fileName );
					string usedPath = Uri.UnescapeDataString( scriptRootUri.MakeRelativeUri( new Uri( fullPath ) ).OriginalString );

					Console.WriteLine( " + {0}:", usedPath );

					Utility.PushColor( ConsoleColor.DarkRed );

					foreach ( CompilerError e in list )
						Console.WriteLine( "    {0}: Line {1}: {3}", e.ErrorNumber, e.Line, e.Column, e.ErrorText );

					Utility.PopColor();
				}

				Utility.PopColor();
			}
			else
			{
				log.Info( "compilation done (0 errors, 0 warnings)" );
			}
		}

		private static Dictionary<string, DateTime> GetScripts( Configuration.Library libConfig, string type )
		{
			var sourceCodeFileProvider = new SourceCodeFileProvider( libConfig, type );

			var files = sourceCodeFileProvider.ProvideSources();

			return files;
		}

		private static bool Compile( Configuration.Library libConfig, bool debug )
		{
			// Check if there is source code for this library.
			if ( libConfig.SourcePath == null )
			{
				if ( libConfig.BinaryPath == null )
				{
					log.Warning( "Library {0} does not exist", libConfig.Name );
					return true;
				}
				if ( !libConfig.BinaryPath.Exists )
				{
					log.Warning( "Library {0} does not exist: {1}", libConfig.Name, libConfig.BinaryPath );
					return false;
				}

				log.Info( "Loading library {0}", libConfig.Name );

				m_Libraries.Add( new Library( libConfig, Assembly.LoadFrom( libConfig.BinaryPath.FullName ) ) );
				m_AdditionalReferences.Add( libConfig.BinaryPath.FullName );

				return true;
			}
			if ( !libConfig.SourcePath.Exists )
			{
				log.Warning( "Library {0} does not exist", libConfig.Name );
				return true;
			}

			var cache = new DirectoryInfo( Core.Config.CacheDirectory ).CreateSubdirectory( libConfig.Name );

			if ( !cache.Exists )
			{
				log.Error( "Failed to create directory {0}", cache.FullName );
				return false;
			}

			var csFile = Path.Combine( cache.FullName, libConfig.Name + ".dll" );
			var files = GetScripts( libConfig, "*.cs" );

			if ( files.Count > 0 )
			{
				var stampFile = Path.Combine( cache.FullName, libConfig.Name + ".stm" );

				if ( File.Exists( csFile ) && CheckStamps( files, stampFile ) )
				{
					m_Libraries.Add( new Library( libConfig, Assembly.LoadFrom( csFile ) ) );
					m_AdditionalReferences.Add( csFile );
					log.Info( "Loaded binary library {0}", libConfig.Name );
				}
				else
				{
					var sorted = new List<string>( files.Keys );
					sorted.Sort();

					var results = CompileCSScripts( sorted, csFile, libConfig, debug );

					if ( results != null )
					{
						if ( results.Errors.HasErrors )
							return false;

						m_Libraries.Add( new Library( libConfig, results.CompiledAssembly ) );
						WriteStampFile( stampFile, files );
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Enqueue a library for compilation, resolving all dependencies first.
		/// </summary>
		/// <param name="dst">This array will receive the libraries in the correct order.</param>
		/// <param name="libs">Source libraries.</param>
		/// <param name="queue">Somewhat like a stack of libraries currently waiting.</param>
		/// <param name="libConfig">The library to be added.</param>
		private static void EnqueueLibrary( List<Configuration.Library> dst, List<Configuration.Library> libs,
			ISet<string> queue, Configuration.Library libConfig )
		{
			var depends = libConfig.Depends;

			if ( libConfig.Name == "Core" || libConfig.Disabled )
			{
				libs.Remove( libConfig );
				return;
			}

			if ( libConfig.IsRemote && ( !libConfig.Exists || Core.ForceUpdateDeps ) )
			{
				var srcPath = Dependencies.Fetch( libConfig );
				libConfig.SourcePath = new DirectoryInfo( srcPath );
			}

			if ( !libConfig.Exists )
			{
				libs.Remove( libConfig );
				log.Warning( "Library {0} does not exist", libConfig.Name );
				return;
			}

			// First resolve dependencies.
			if ( depends != null )
			{
				queue.Add( libConfig.Name );

				foreach ( var depend in depends )
				{
					// If the depended library is already in the queue, there is a circular dependency.
					if ( queue.Contains( depend ) )
					{
						log.Error( "Circular library dependency {0} on {1}", libConfig.Name, depend );
						throw new ApplicationException();
					}

					var next = Core.LibraryConfig.GetLibrary( depend );
					if ( next == null || !next.Exists )
					{
						log.Error( "Unresolved library dependency: {0} depends on {1}, which does not exist", libConfig.Name, depend );
						throw new ApplicationException();
					}

					if ( next.Disabled )
					{
						log.Error( "Unresolved library dependency: {0} depends on {1}, which is disabled", libConfig.Name, depend );
						throw new ApplicationException();
					}

					if ( !dst.Contains( next ) )
						EnqueueLibrary( dst, libs, queue, next );
				}

				queue.Remove( libConfig.Name );
			}

			// Then add it to 'dst'.
			dst.Add( libConfig );
			libs.Remove( libConfig );
		}

		private static IEnumerable<Configuration.Library> SortLibrariesByDepends()
		{
			var libs = new List<Configuration.Library>( Core.LibraryConfig.Libraries );
			var queue = new HashSet<string>();
			var dst = new List<Configuration.Library>();

			// Handle 'Distro' first, for most compatibility.
			var libConfig = Core.LibraryConfig.GetLibrary( "Distro" );

			if ( libConfig != null )
				EnqueueLibrary( dst, libs, queue, libConfig );

			while ( libs.Count > 0 )
				EnqueueLibrary( dst, libs, queue, libs[0] );

			return dst;
		}

		public static bool Compile( bool debug )
		{
			if ( AlreadyCompiled )
				throw new ApplicationException( "already compiled" );

			m_AdditionalReferences = new List<string>();

			m_Libraries = new List<Library>();
			m_Libraries.Add( new Library( Core.LibraryConfig.GetLibrary( "Core" ), Core.Assembly ) );

			// Collect Config.Library objects, sort them and compile.
			var libConfigs = SortLibrariesByDepends();

			foreach ( var libConfig in libConfigs )
			{
				var result = Compile( libConfig, debug );

				if ( !result )
					return false;
			}

			// Delete unused cache directories.
			var cacheDir = new DirectoryInfo( Core.Config.CacheDirectory );

			foreach ( var sub in cacheDir.GetDirectories() )
			{
				var libName = sub.Name;

				if ( GetLibrary( libName ) == null )
					sub.Delete( true );
			}

			return true;
		}

		private static bool AlreadyCompiled
		{
			get { return m_AdditionalReferences != null; }
		}

		public static void Configure()
		{
			foreach ( var library in m_Libraries )
				library.Configure();
		}

		public static void Initialize()
		{
			foreach ( var library in m_Libraries )
				library.Initialize();
		}

		private static Hashtable m_TypeCaches = new Hashtable();
		private static TypeCache m_NullCache;

		private static Library FindLibrary( Assembly assembly )
		{
			return m_Libraries.FirstOrDefault( lib => lib.Assembly == assembly );
		}

		public static TypeCache GetTypeCache( Assembly assembly )
		{
			if ( assembly == null )
			{
				if ( m_NullCache == null )
					m_NullCache = new TypeCache( Type.EmptyTypes, new TypeTable( 0 ), new TypeTable( 0 ) );

				return m_NullCache;
			}

			TypeCache cache = (TypeCache) m_TypeCaches[assembly];

			if ( cache == null )
			{
				Library library = FindLibrary( assembly );

				if ( library == null )
					throw new ApplicationException( "Invalid assembly" );

				m_TypeCaches[assembly] = cache = library.TypeCache;
			}

			return cache;
		}

		public static Type FindTypeByFullName( string fullName )
		{
			return FindTypeByFullName( fullName, true );
		}

		public static Type FindTypeByFullName( string fullName, bool ignoreCase )
		{
			for ( int i = m_Libraries.Count - 1; i >= 0; --i )
			{
				var library = m_Libraries[i];

				var type = library.TypeCache.GetTypeByFullName( fullName, ignoreCase );

				if ( type != null )
					return type;
			}

			return null;
		}

		public static Type FindTypeByName( string name )
		{
			return FindTypeByName( name, true );
		}

		public static Type FindTypeByName( string name, bool ignoreCase )
		{
			for ( int i = m_Libraries.Count - 1; i >= 0; --i )
			{
				var library = m_Libraries[i];

				var type = library.TypeCache.GetTypeByName( name, ignoreCase );

				if ( type != null )
					return type;
			}

			return null;
		}

		private static int m_ItemCount, m_MobileCount;

		public static int ScriptItems
		{
			get { return m_ItemCount; }
		}

		public static int ScriptMobiles
		{
			get { return m_MobileCount; }
		}

		public static void VerifyLibraries()
		{
			m_ItemCount = 0;
			m_MobileCount = 0;

			foreach ( var library in ScriptCompiler.Libraries )
			{
				int itemCount = 0, mobileCount = 0;
				library.Verify( ref itemCount, ref mobileCount );
				log.Info( "Library {0} verified: {1} items, {2} mobiles", library.Name, itemCount, mobileCount );

				m_ItemCount += itemCount;
				m_MobileCount += mobileCount;
			}

			log.Info( "All libraries verified: {0} items, {1} mobiles)", m_ItemCount, m_MobileCount );
		}
	}

	public class TypeCache
	{
		private Type[] m_Types;
		private TypeTable m_Names, m_FullNames;

		public Type[] Types
		{
			get { return m_Types; }
		}

		public TypeTable Names
		{
			get { return m_Names; }
		}

		public TypeTable FullNames
		{
			get { return m_FullNames; }
		}

		public Type GetTypeByName( string name, bool ignoreCase )
		{
			return m_Names.Get( name, ignoreCase );
		}

		public Type GetTypeByFullName( string fullName, bool ignoreCase )
		{
			return m_FullNames.Get( fullName, ignoreCase );
		}

		public TypeCache( Type[] types, TypeTable names, TypeTable fullNames )
		{
			m_Types = types;
			m_Names = names;
			m_FullNames = fullNames;
		}
	}

	public class TypeTable
	{
		private Dictionary<string, Type> m_Sensitive, m_Insensitive;

		public void Add( string key, Type type )
		{
			m_Sensitive[key] = type;
			m_Insensitive[key] = type;
		}

		public Type Get( string key, bool ignoreCase )
		{
			Type type = null;

			if ( ignoreCase )
				m_Insensitive.TryGetValue( key, out type );
			else
				m_Sensitive.TryGetValue( key, out type );

			return type;
		}

		public TypeTable( int capacity )
		{
			m_Sensitive = new Dictionary<string, Type>( capacity );
			m_Insensitive = new Dictionary<string, Type>( capacity, StringComparer.OrdinalIgnoreCase );
		}
	}
}
