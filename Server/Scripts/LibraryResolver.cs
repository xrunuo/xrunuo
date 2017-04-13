using System;
using System.Collections.Generic;
using System.IO;

namespace Server
{
	public class LibraryResolver
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		// Source libraries.
		private List<Configuration.Library> m_Libs;
		// Somewhat like a stack of libraries currently waiting.
		private ISet<string> m_Queue;
		// This array will receive the libraries in the correct order.
		private List<Configuration.Library> m_Dst;

		public LibraryResolver( IEnumerable<Configuration.Library> libs )
		{
			m_Libs = new List<Configuration.Library>( libs );
			m_Queue = new HashSet<string>();
			m_Dst = new List<Configuration.Library>();
		}

		public IEnumerable<Configuration.Library> SortLibrariesByDepends()
		{
			// Handle 'Distro' first, for most compatibility.
			var distroLibConfig = Core.LibraryConfig.GetLibrary( "Distro" );

			if ( distroLibConfig != null )
				EnqueueLibrary( distroLibConfig );

			while ( m_Libs.Count > 0 )
				EnqueueLibrary( m_Libs[0] );

			return m_Dst;
		}

		/// <summary>
		/// Enqueue a library for compilation, resolving all dependencies first.
		/// </summary>
		/// <param name="libConfig">The library to be added.</param>
		private void EnqueueLibrary( Configuration.Library libConfig )
		{
			var depends = libConfig.Depends;

			if ( libConfig.Name == "Core" || libConfig.Disabled )
			{
				m_Libs.Remove( libConfig );
				return;
			}

			if ( libConfig.IsRemote && ( !libConfig.Exists || Core.ForceUpdateDeps ) )
			{
				var srcPath = Dependencies.Fetch( libConfig );
				libConfig.SourcePath = new DirectoryInfo( srcPath );
			}

			if ( !libConfig.Exists )
			{
				m_Libs.Remove( libConfig );
				log.Warning( "Library {0} does not exist", libConfig.Name );
				return;
			}

			// First resolve dependencies.
			if ( depends != null )
			{
				m_Queue.Add( libConfig.Name );

				foreach ( var depend in depends )
				{
					// If the depended library is already in the queue, there is a circular dependency.
					if ( m_Queue.Contains( depend ) )
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

					if ( !m_Dst.Contains( next ) )
						EnqueueLibrary( next );
				}

				m_Queue.Remove( libConfig.Name );
			}

			// Then add it to 'dst'.
			m_Dst.Add( libConfig );
			m_Libs.Remove( libConfig );
		}
	}
}
