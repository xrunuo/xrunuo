using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Server
{
	public class ScriptCompiler
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static List<Library> m_Libraries;

		public static Library[] Libraries => m_Libraries.ToArray();

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

		private static bool Compile( Configuration.Library libConfig, bool debug )
		{
			if ( libConfig.BinaryPath == null )
			{
				log.Error( "Library {0} does not exist. Make sure it has been pre-compiled (X-RunUO no longer supports on-the-fly compilation of script libraries)", libConfig.Name );
				return false;
			}
			else
			{
				log.Info( "Loading library {0}", libConfig.Name );

				m_Libraries.Add( new Library( libConfig, Assembly.LoadFrom( libConfig.BinaryPath.FullName ) ) );
				m_AdditionalReferences.Add( libConfig.BinaryPath.FullName );

				return true;
			}
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

			if ( libConfig.Name == "Core"  )
			{
				libs.Remove( libConfig );
				return;
			}

			if ( libConfig.Disabled )
			{
				log.Warning( "Library {0} is disabled, it won't be loaded", libConfig.Name );
				libs.Remove( libConfig );
				return;
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

			return true;
		}

		private static bool AlreadyCompiled => m_AdditionalReferences != null;

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

		private static readonly Hashtable m_TypeCaches = new Hashtable();
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

		public static int ScriptItems { get; private set; }

		public static int ScriptMobiles { get; private set; }

		public static void VerifyLibraries()
		{
			ScriptItems = 0;
			ScriptMobiles = 0;

			foreach ( var library in ScriptCompiler.Libraries )
			{
				int itemCount = 0, mobileCount = 0;
				library.Verify( ref itemCount, ref mobileCount );
				log.Info( "Library {0} verified: {1} items, {2} mobiles", library.Name, itemCount, mobileCount );

				ScriptItems += itemCount;
				ScriptMobiles += mobileCount;
			}

			log.Info( "All libraries verified: {0} items, {1} mobiles)", ScriptItems, ScriptMobiles );
		}
	}

	public class TypeCache
	{
		public Type[] Types { get; }

		public TypeTable Names { get; }

		public TypeTable FullNames { get; }

		public Type GetTypeByName( string name, bool ignoreCase )
		{
			return Names.Get( name, ignoreCase );
		}

		public Type GetTypeByFullName( string fullName, bool ignoreCase )
		{
			return FullNames.Get( fullName, ignoreCase );
		}

		public TypeCache( Type[] types, TypeTable names, TypeTable fullNames )
		{
			Types = types;
			Names = names;
			FullNames = fullNames;
		}
	}

	public class TypeTable
	{
		private readonly Dictionary<string, Type> m_Sensitive;
		private readonly Dictionary<string, Type> m_Insensitive;

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
