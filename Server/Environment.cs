using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Threading;
using Server.Configuration;

namespace Server
{
	public static class Environment
	{
		private static string m_BaseDirectory;
		private static string m_ExePath;

		private static Assembly m_Assembly;
		private static Process m_Process;
		private static Thread m_Thread;
		private static bool m_Service;
		private static bool m_Debug;

		private static RootConfig m_Config;
		private static LibraryConfig m_LibraryConfig;

		private static bool m_Profiling;
		private static DateTime m_ProfileStart;
		private static TimeSpan m_ProfileTime;

		private static bool m_Logging;

		public static bool Profiling
		{
			get { return m_Profiling; }
			set
			{
				if ( m_Profiling == value )
					return;

				m_Profiling = value;

				if ( m_ProfileStart > DateTime.MinValue )
					m_ProfileTime += DateTime.UtcNow - m_ProfileStart;

				m_ProfileStart = ( m_Profiling ? DateTime.UtcNow : DateTime.MinValue );
			}
		}

		public static TimeSpan ProfileTime
		{
			get
			{
				if ( m_ProfileStart > DateTime.MinValue )
					return m_ProfileTime + ( DateTime.UtcNow - m_ProfileStart );

				return m_ProfileTime;
			}
		}

		private static bool m_Unix;

		public static bool Unix
		{
			get { return m_Unix; }
		}

		public static readonly bool Is64Bit = IntPtr.Size == 8;
		private static readonly int ProcessorCount = System.Environment.ProcessorCount;

		private static bool MultiProcessor
		{
			get { return ProcessorCount > 1; }
		}

		public static bool Logging
		{
			get { return m_Logging; }
			set { m_Logging = value; }
		}

		public static bool Service
		{
			get { return m_Service; }
		}

		public static bool Debug
		{
			get { return m_Debug; }
		}

		public static List<string> DataDirectories
		{
			get { return m_Config.DataDirectories; }
		}

		public static Assembly Assembly
		{
			get { return m_Assembly; }
			set { m_Assembly = value; }
		}

		public static Process Process
		{
			get { return m_Process; }
		}

		public static string FindDataFile( string path )
		{
			if ( DataDirectories.Count == 0 )
				throw new InvalidOperationException( "Attempted to FindDataFile before DataDirectories list has been filled." );

			string fullPath = null;

			foreach ( var directory in DataDirectories )
			{
				fullPath = Path.Combine( directory, path );

				if ( File.Exists( fullPath ) )
					break;

				fullPath = null;
			}

			return fullPath;
		}

		public static string FindDataFile( string format, params object[] args )
		{
			return FindDataFile( String.Format( format, args ) );
		}

		public static string ExePath
		{
			get
			{
				if ( m_ExePath == null )
					m_ExePath = Assembly.Location;

				return m_ExePath;
			}
		}

		public static RootConfig Config
		{
			get { return m_Config; }
		}

		#region Dependency management
		public static LibraryConfig LibraryConfig
		{
			get { return m_LibraryConfig; }
		}
		#endregion

		public static bool ForceUpdateDeps { get; set; }

		public static string BaseDirectory
		{
			get
			{
				if ( m_BaseDirectory == null )
				{
					try
					{
						m_BaseDirectory = ExePath;

						if ( m_BaseDirectory.Length > 0 )
							m_BaseDirectory = Path.GetDirectoryName( m_BaseDirectory );
					}
					catch
					{
						m_BaseDirectory = "";
					}
				}

				return m_BaseDirectory;
			}
		}

		public static Version CoreVersion;

		public static void Initialize( string[] args )
		{
			ParseArguments( args );

			SetupConsoleLogging();

			m_Thread = Thread.CurrentThread;
			m_Process = Process.GetCurrentProcess();
			m_Assembly = Assembly.GetEntryAssembly();

			if ( m_Thread != null )
				m_Thread.Name = "Core Thread";

			if ( BaseDirectory.Length > 0 )
				Directory.SetCurrentDirectory( BaseDirectory );

			var version = m_Assembly.GetName().Version;
			CoreVersion = version;

			var platform = (int) System.Environment.OSVersion.Platform;
			if ( platform == 4 || platform == 128 )
				m_Unix = true;

			GCSettings.LatencyMode = GCLatencyMode.LowLatency;

			Console.WriteLine( "X-RunUO Server - Version {0}.{1}.{2}, Build {3}", version.Major, version.Minor, version.Build,
				version.Revision );
			Console.WriteLine( "Core: Running on OS {0}", System.Environment.OSVersion );
			Console.WriteLine( "Core: Running on {0} {1}", Unix ? "Mono" : ".NET Framework", System.Environment.Version );

			if ( MultiProcessor || Is64Bit )
				Console.WriteLine( "Core: Optimizing for {0} {2}processor{1}", ProcessorCount, ProcessorCount == 1 ? "" : "s",
					Is64Bit ? "64-bit " : "" );

			Console.WriteLine( "Core: Using GC {0} {1} mode", GCSettings.IsServerGC ? "Server" : "Workstation",
				GCSettings.LatencyMode );

			m_Config = new RootConfig( BaseDirectory, "x-runuo.xml" );

			#region Dependency management
			m_LibraryConfig = new LibraryConfig( BaseDirectory, "libraries.xml" );

			if ( ForceUpdateDeps )
				Directory.Delete( Path.Combine( BaseDirectory, "deps" ), recursive: true );
			#endregion
		}

		private static void ParseArguments( string[] args )
		{
			for ( int i = 0; i < args.Length; ++i )
			{
				if ( Insensitive.Equals( args[i], "--debug" ) )
					m_Debug = true;
				else if ( Insensitive.Equals( args[i], "--service" ) )
					m_Service = true;
				else if ( Insensitive.Equals( args[i], "--profile" ) )
					Profiling = true;
				#region Dependency management
				else if ( Insensitive.Equals( args[i], "--update-deps" ) )
					ForceUpdateDeps = true;
				#endregion
			}
		}

		private static void SetupConsoleLogging()
		{
			try
			{
				if ( m_Service )
				{
					if ( !Directory.Exists( Environment.Config.LogDirectory ) )
						Directory.CreateDirectory( Environment.Config.LogDirectory );

					Console.SetOut( new MultiTextWriter( Console.Out,
						new FileLogger( Path.Combine( Environment.Config.LogDirectory, "Console.log" ) ) ) );
				}
				else
				{
					Console.SetOut( new MultiTextWriter( Console.Out ) );
				}
			}
			catch
			{
			}
		}

		public static void SaveConfig()
		{
			if ( !m_Config.Exists )
				m_Config.Save();

			if ( !m_LibraryConfig.Exists )
				m_LibraryConfig.Save();
		}
	}
}
