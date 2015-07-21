//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace Server
{
	public class ScriptCompiler
	{
		private static List<Library> m_Libraries;

		public static Library[] Libraries
		{
			get
			{
				return m_Libraries.ToArray();
			}
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

			var path = Path.Combine( Environment.Config.ConfigDirectory, "Assemblies.cfg" );

			if ( File.Exists( path ) )
			{
				var lines = File.ReadLines( path )
					.Where( line => line.Length > 0 && !line.StartsWith( "#" ) );

				assemblies.AddRange( lines );
			}

			assemblies.Add( Environment.ExePath );
			assemblies.AddRange( m_AdditionalReferences );

			return assemblies.ToArray();
		}

		private static Hashtable ReadStampFile( string filename )
		{
			if ( !File.Exists( filename ) )
				return null;

			FileStream fs = new FileStream( filename, FileMode.Open, FileAccess.Read );
			BinaryReader br = new BinaryReader( fs );
			int version = br.ReadInt32();
			if ( version != 1 )
				return null;

			Hashtable stamps = new Hashtable();

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

		private static void WriteStampFile( string filename, Hashtable stamps )
		{
			FileStream fs = new FileStream( filename, FileMode.OpenOrCreate, FileAccess.Write );
			BinaryWriter bw = new BinaryWriter( fs );
			bw.Write( (int) 1 );

			bw.Write( (uint) stamps.Count );
			IDictionaryEnumerator e = stamps.GetEnumerator();
			while ( e.MoveNext() )
			{
				bw.Write( (string) e.Key );
				bw.Write( (long) ( (DateTime) e.Value ).Ticks );
			}

			bw.Close();
			fs.Close();
		}

		private static bool CheckStamps( Hashtable files,
			string stampFile )
		{
			Hashtable stamps = ReadStampFile( stampFile );
			if ( stamps == null )
				return false;

			IDictionaryEnumerator e = files.GetEnumerator();
			while ( e.MoveNext() )
			{
				string filename = (string) e.Key;
				if ( !stamps.ContainsKey( filename ) )
					return false;

				DateTime newStamp = (DateTime) e.Value;
				DateTime oldStamp = (DateTime) stamps[filename];

				if ( oldStamp != newStamp )
					return false;

				stamps.Remove( filename );
			}

			return stamps.Count == 0;
		}

		private static CompilerResults CompileCSScripts( ICollection fileColl,
														string assemblyFile,
														Configuration.Library libConfig,
														bool debug )
		{
			CSharpCodeProvider provider = new CSharpCodeProvider();
#pragma warning disable 618
			ICodeCompiler compiler = provider.CreateCompiler();
#pragma warning restore 618

			string[] files;

			Console.WriteLine( "Scripts: Compiling library {0}, {1} C# sources",
						   libConfig.Name, fileColl.Count );

			string tempFile = compiler.GetType().FullName == "Mono.CSharp.CSharpCodeCompiler"
				? Path.GetTempFileName() : null;
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
				StreamWriter w = new StreamWriter( tempFile, false );
				foreach ( string file in fileColl )
				{
					w.Write( "\"" + file + "\" " );
				}
				w.Close();

				files = new string[0];
			}

			CompilerParameters parms = new CompilerParameters( GetReferenceAssemblies(), assemblyFile, debug );
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
					Console.WriteLine( "Fatal: Could not find the compiler - are you sure MCS is installed?" );
					Console.WriteLine( "       (on Debian, try: apt-get install mono-mcs)" );
					System.Environment.Exit( 2 );
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
				Dictionary<string, List<CompilerError>> errors = new Dictionary<string, List<CompilerError>>( results.Errors.Count, StringComparer.OrdinalIgnoreCase );
				Dictionary<string, List<CompilerError>> warnings = new Dictionary<string, List<CompilerError>>( results.Errors.Count, StringComparer.OrdinalIgnoreCase );

				foreach ( CompilerError e in results.Errors )
				{
					string file = e.FileName;

					// Rediculous. FileName is null if the warning/error is internally generated in csc.
					if ( string.IsNullOrEmpty( file ) )
					{
						Console.WriteLine( "ScriptCompiler: {0}: {1}", e.ErrorNumber, e.ErrorText );
						continue;
					}

					Dictionary<string, List<CompilerError>> table = ( e.IsWarning ? warnings : errors );

					List<CompilerError> list = null;
					table.TryGetValue( file, out list );

					if ( list == null )
						table[file] = list = new List<CompilerError>();

					list.Add( e );
				}

				if ( errors.Count > 0 )
					Console.WriteLine( "Scripts: compilation failed ({0} errors, {1} warnings)", errors.Count, warnings.Count );
				else
					Console.WriteLine( "Scripts: compilation done ({0} errors, {1} warnings)", errors.Count, warnings.Count );

				string scriptRoot = Path.GetFullPath( Path.Combine( Environment.BaseDirectory, "src" + Path.DirectorySeparatorChar ) );
				Uri scriptRootUri = new Uri( scriptRoot );
				/*
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
				*/
				Utility.PushColor( ConsoleColor.Red );

				if ( errors.Count > 0 )
					Console.WriteLine( "Errors:" );

				foreach ( KeyValuePair<string, List<CompilerError>> kvp in errors )
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
				Console.WriteLine( "Scripts: compilation done (0 errors, 0 warnings)" );
			}
		}

		private static void Overlay( string base1, Hashtable files1,
									string base2, Hashtable files2 )
		{
			foreach ( string filename in files2.Keys )
			{
				files1.Remove( base1 + Path.DirectorySeparatorChar + filename.Substring( base2.Length + 1 ) );
				files1[filename] = files2[filename];
			}
		}

		private static Hashtable GetScripts( Configuration.Library libConfig, IEnumerable overlays, string type )
		{
			Hashtable files = GetScripts( libConfig, type );

			if ( overlays != null )
			{
				foreach ( Configuration.Library overlay in overlays )
				{
					Hashtable files2 = GetScripts( overlay, type );

					Overlay( libConfig.SourcePath.FullName, files, overlay.SourcePath.FullName, files2 );
				}
			}

			return files;
		}

		private static bool Compile( Configuration.Library libConfig, bool debug )
		{
			// Check if there is source code for this library.
			if ( libConfig.SourcePath == null )
			{
				if ( libConfig.BinaryPath == null )
				{
					Console.WriteLine( "Warning: library {0} does not exist", libConfig.Name );
					return true;
				}
				else if ( !libConfig.BinaryPath.Exists )
				{
					Console.WriteLine( "Warning: library {0} does not exist: {1}", libConfig.Name, libConfig.BinaryPath );
					return false;
				}

				Console.WriteLine( "Libraries: Loading library {0}", libConfig.Name );

				m_Libraries.Add( new Library( libConfig, Assembly.LoadFrom( libConfig.BinaryPath.FullName ) ) );
				m_AdditionalReferences.Add( libConfig.BinaryPath.FullName );

				return true;
			}
			else if ( !libConfig.SourcePath.Exists )
			{
				Console.WriteLine( "Warning: library {0} does not exist", libConfig.Name );
				return true;
			}

			DirectoryInfo cache = new DirectoryInfo( Environment.Config.CacheDirectory ).CreateSubdirectory( libConfig.Name );

			if ( !cache.Exists )
			{
				Console.WriteLine( "Scripts: Failed to create directory {0}", cache.FullName );
				return false;
			}

			List<Configuration.Library> overlays = null;

			if ( libConfig.Overlays != null )
			{
				overlays = new List<Configuration.Library>();

				foreach ( string name in libConfig.Overlays )
					overlays.Add( Environment.Config.GetLibrary( name ) );
			}

			string csFile = Path.Combine( cache.FullName, libConfig.Name + ".dll" );
			Hashtable files = GetScripts( libConfig, overlays, "*.cs" );

			if ( files.Count > 0 )
			{
				string stampFile = Path.Combine( cache.FullName, libConfig.Name + ".stm" );

				if ( File.Exists( csFile ) && CheckStamps( files, stampFile ) )
				{
					m_Libraries.Add( new Library( libConfig, Assembly.LoadFrom( csFile ) ) );
					m_AdditionalReferences.Add( csFile );
					Console.WriteLine( "Libraries: Loaded binary library {0}", libConfig.Name );
				}
				else
				{
					ArrayList sorted = new ArrayList( files.Keys );
					sorted.Sort();

					CompilerResults results = CompileCSScripts( sorted, csFile, libConfig, debug );

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
		private static void EnqueueLibrary( ArrayList dst, ArrayList libs, Hashtable queue, Configuration.Library libConfig )
		{
			string[] depends = libConfig.Depends;

			if ( libConfig.Name == "core" || libConfig.Disabled )
			{
				libs.Remove( libConfig );
				return;
			}

			if ( !libConfig.Exists )
			{
				libs.Remove( libConfig );
				Console.WriteLine( "Warning: library {0} does not exist", libConfig.Name );
				return;
			}

			/* first resolve dependencies */
			if ( depends != null )
			{
				queue[libConfig.Name] = 1;

				foreach ( string depend in depends )
				{
					/* if the depended library is already in the
					 * queue, there is a circular dependency */
					if ( queue.ContainsKey( depend ) )
					{
						Console.WriteLine( "Error: Circular library dependency {0} on {1}",
										libConfig.Name, depend );
						throw new ApplicationException();
					}

					Configuration.Library next = Environment.Config.GetLibrary( depend );
					if ( next == null || !next.Exists )
					{
						Console.WriteLine( "Error: Unresolved library dependency: {0} depends on {1}, which does not exist",
										libConfig.Name, depend );
						throw new ApplicationException();
					}

					if ( next.Disabled )
					{
						Console.WriteLine( "Error: Unresolved library dependency: {0} depends on {1}, which is disabled",
										libConfig.Name, depend );
						throw new ApplicationException();
					}

					if ( !dst.Contains( next ) )
						EnqueueLibrary( dst, libs, queue, next );
				}

				queue.Remove( libConfig.Name );
			}

			/* then add it to 'dst' */
			dst.Add( libConfig );
			libs.Remove( libConfig );
		}

		private static ArrayList SortLibrariesByDepends()
		{
			ArrayList libs = new ArrayList( Environment.Config.Libraries );
			Hashtable queue = new Hashtable();
			ArrayList dst = new ArrayList();

			// Handle distro first, for most compatibility.
			Configuration.Library libConfig = Environment.Config.GetLibrary( "distro" );

			if ( libConfig != null )
				EnqueueLibrary( dst, libs, queue, libConfig );

			while ( libs.Count > 0 )
				EnqueueLibrary( dst, libs, queue, (Configuration.Library) libs[0] );

			return dst;
		}

		public static bool Compile( bool debug )
		{
			if ( m_AdditionalReferences != null )
				throw new ApplicationException( "already compiled" );

			m_AdditionalReferences = new List<string>();

			m_Libraries = new List<Library>();
			m_Libraries.Add( new Library( Environment.Config.GetLibrary( "core" ), Environment.Assembly ) );

			// Prepare overlays.
			foreach ( Configuration.Library libConfig in Environment.Config.Libraries )
			{
				if ( libConfig.Overlays == null || !libConfig.Exists ||
					libConfig.Name == "core" )
					continue;

				if ( libConfig.SourcePath == null )
				{
					Console.WriteLine( "Error: Can't overlay the binary library {0}", libConfig.Name );
					throw new ApplicationException();
				}

				foreach ( string name in libConfig.Overlays )
				{
					Configuration.Library overlay = Environment.Config.GetLibrary( name );

					if ( overlay == null || !overlay.Exists )
					{
						Console.WriteLine( "Error: Can't overlay {0} with {1}, because it does not exist", libConfig.Name, name );
						throw new ApplicationException();
					}

					if ( overlay.SourcePath == null )
					{
						Console.WriteLine( "Error: Can't overlay {0} with {1}, because it is binary only", libConfig.Name, overlay.Name );
						throw new ApplicationException();
					}

					overlay.Disabled = true;
				}
			}

			foreach ( Configuration.Library libConfig in Environment.Config.Libraries )
			{
				if ( libConfig.Overlays != null && libConfig.Exists && libConfig.Name != "core" && libConfig.Disabled )
				{
					Console.WriteLine( "Error: Can't overlay library {0} which is already used as overlay for another library", libConfig.Name );
					throw new ApplicationException();
				}
			}

			// Collect Config.Library objects, sort them and compile.
			ArrayList libConfigs = SortLibrariesByDepends();

			foreach ( Configuration.Library libConfig in libConfigs )
			{
				bool result = Compile( libConfig, debug );

				if ( !result )
					return false;
			}

			// Delete unused cache directories.
			DirectoryInfo cacheDir = new DirectoryInfo( Environment.Config.CacheDirectory );

			foreach ( DirectoryInfo sub in cacheDir.GetDirectories() )
			{
				string libName = sub.Name.ToLower();

				if ( GetLibrary( libName ) == null )
					sub.Delete( true );
			}

			return true;
		}

		public static void Configure()
		{
			foreach ( Library library in m_Libraries )
				library.Configure();
		}

		public static void Initialize()
		{
			foreach ( Library library in m_Libraries )
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
				Library library = m_Libraries[i];

				Type type = library.TypeCache.GetTypeByFullName( fullName, ignoreCase );

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
				Library library = m_Libraries[i];

				Type type = library.TypeCache.GetTypeByName( name, ignoreCase );

				if ( type != null )
					return type;
			}

			return null;
		}

		private static Hashtable GetScripts( Configuration.Library libConfig, string type )
		{
			Hashtable list = new Hashtable();

			GetScripts( libConfig, list, libConfig.SourcePath.FullName, type );

			return list;
		}

		private static string[] m_IgnoreNames = new string[]
			{
				".svn", "_svn", "_darcs", ".git", ".hg", "cvs"
			};

		private static void GetScripts( Configuration.Library libConfig, Hashtable list, string path, string type )
		{
			foreach ( string dir in Directory.GetDirectories( path ) )
			{
				string baseName = Path.GetFileName( dir ).ToLower();

				for ( int i = 0; i < m_IgnoreNames.Length; i++ )
				{
					if ( baseName == m_IgnoreNames[i] )
						continue;
				}

				GetScripts( libConfig, list, dir, type );
			}

			foreach ( string filename in Directory.GetFiles( path, type ) )
			{
				// Pass relative filename only.
				if ( libConfig == null || !libConfig.GetIgnoreSource( filename ) )
					list[filename] = File.GetLastWriteTime( filename );
			}
		}

		private static int m_ItemCount, m_MobileCount;

		public static int ScriptItems { get { return m_ItemCount; } }
		public static int ScriptMobiles { get { return m_MobileCount; } }

		public static void VerifyLibraries()
		{
			m_ItemCount = 0;
			m_MobileCount = 0;

			foreach ( Library library in ScriptCompiler.Libraries )
			{
				int itemCount = 0, mobileCount = 0;
				library.Verify( ref itemCount, ref mobileCount );
				Console.WriteLine( "Scripts: Library {0} verified: {1} items, {2} mobiles",
					library.Name, itemCount, mobileCount );

				m_ItemCount += itemCount;
				m_MobileCount += mobileCount;
			}

			Console.WriteLine( "Scripts: All libraries verified: {0} items, {1} mobiles)", m_ItemCount, m_MobileCount );
		}
	}

	public class TypeCache
	{
		private Type[] m_Types;
		private TypeTable m_Names, m_FullNames;

		public Type[] Types { get { return m_Types; } }
		public TypeTable Names { get { return m_Names; } }
		public TypeTable FullNames { get { return m_FullNames; } }

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
