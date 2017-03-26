using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Threading;

using Server.Configuration;
using Server.Events;
using Server.Network;
using Server.Profiler;

using GameServer = Server.Network.GameServer;

namespace Server
{
	public static class Core
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static Assembly m_Assembly;
		private static Process m_Process;
		private static Thread m_Thread;
		private static bool m_Service;
		private static bool m_Debug;

		private static RootConfig m_Config;
		private static LibraryConfig m_LibraryConfig;

		private static string m_BaseDirectory;
		private static string m_ExePath;

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
		private static readonly int ProcessorCount = Environment.ProcessorCount;

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

		public static bool ForceUpdateDeps { get; set; }
		#endregion

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
					if ( !Directory.Exists( m_Config.LogDirectory ) )
						Directory.CreateDirectory( m_Config.LogDirectory );

					Console.SetOut( new MultiTextWriter( Console.Out,
						new FileLogger( Path.Combine( m_Config.LogDirectory, "Console.log" ) ) ) );
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

		private static bool m_Crashed;
		private static bool m_Closing;

		public static bool Closing { get { return m_Closing; } }

		private static TimerThread m_TimerThread;

		/* current time */
		private static DateTime m_Now = DateTime.UtcNow;
		public static DateTime Now
		{
			get
			{
				return m_Now;
			}
		}

		/* main loop profiler */
		private static MainProfile m_TotalProfile;
		private static MainProfile m_CurrentProfile;

		public static MainProfile TotalProfile
		{
			get { return m_TotalProfile; }
		}

		public static MainProfile CurrentProfile
		{
			get { return m_CurrentProfile; }
		}

		public static void ResetCurrentProfile()
		{
			m_CurrentProfile = new MainProfile( m_Now );
		}

		private static void ClockProfile( MainProfile.TimerId id )
		{
			DateTime prev = m_Now;
			m_Now = DateTime.UtcNow;

			TimeSpan diff = m_Now - prev;
			m_TotalProfile.Add( id, diff );
			m_CurrentProfile.Add( id, diff );
		}

		public static void Main( string[] args )
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

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

			var platform = (int) Environment.OSVersion.Platform;
			if ( platform == 4 || platform == 128 )
				m_Unix = true;

			GCSettings.LatencyMode = GCLatencyMode.LowLatency;

			log.Info( "X-RunUO Server - Version {0}.{1}.{2}, Build {3}", version.Major, version.Minor, version.Build, version.Revision );
			log.Info( "Running on OS {0}", Environment.OSVersion );
			log.Info( "Running on {0} {1}", Unix ? "Mono" : ".NET Framework", Environment.Version );

			if ( MultiProcessor || Is64Bit )
				log.Info( "Optimizing for {0} {2}processor{1}", ProcessorCount, ProcessorCount == 1 ? "" : "s", Is64Bit ? "64-bit " : "" );

			log.Info( "Using GC {0} {1} mode", GCSettings.IsServerGC ? "Server" : "Workstation", GCSettings.LatencyMode );

			m_Config = new RootConfig( BaseDirectory, "x-runuo.xml" );

			#region Dependency management
			m_LibraryConfig = new LibraryConfig( BaseDirectory, "libraries.xml" );

			if ( ForceUpdateDeps )
				Directory.Delete( Path.Combine( BaseDirectory, "deps" ), recursive: true );
			#endregion

			if ( !ScriptCompiler.Compile( m_Debug ) )
			{
				log.Fatal( "Fatal: Compilation failed. Press any key to exit." );
				Console.ReadLine();
				return;
			}

			ScriptCompiler.VerifyLibraries();

			// This instance is shared among timer scheduler and timer executor,
			// and accessed from both core & timer threads.
			var timerQueue = new Queue<Timer>();

			// Timer scheduler must be set up before world load, since world load
			// could schedule timers on entity deserialization.
			var timerScheduler = TimerScheduler.Instance = new TimerScheduler( timerQueue );
			m_TimerThread = new TimerThread( timerScheduler );

			var timerExecutor = new TimerExecutor( timerQueue );

			try
			{
				ScriptCompiler.Configure();

				TileData.Configure();
			}
			catch ( TargetInvocationException e )
			{
				log.Fatal( "Fatal: Configure exception: {0}", e.InnerException );
				return;
			}

			SaveConfig();

			Region.Load();
			World.Load();

			try
			{
				ScriptCompiler.Initialize();
			}
			catch ( TargetInvocationException e )
			{
				log.Fatal( "Initialize exception: {0}", e.InnerException );
				return;
			}

			m_TimerThread.Start();

			NetServer netServer = new NetServer( new Listener( Listener.Port ) );
			netServer.Initialize();

			GameServer.Instance = new GameServer( netServer );
			GameServer.Instance.Initialize();

			EventSink.InvokeServerStarted();

			PacketDispatcher.Initialize();

			m_Now = DateTime.UtcNow;
			m_TotalProfile = new MainProfile( m_Now );
			m_CurrentProfile = new MainProfile( m_Now );

			try
			{
				while ( !m_Closing )
				{
					m_Now = DateTime.UtcNow;

					Thread.Sleep( 1 );

					ClockProfile( MainProfile.TimerId.Idle );

					Mobile.ProcessDeltaQueue();

					ClockProfile( MainProfile.TimerId.MobileDelta );

					Item.ProcessDeltaQueue();

					ClockProfile( MainProfile.TimerId.ItemDelta );

					timerExecutor.Slice();

					ClockProfile( MainProfile.TimerId.Timers );

					netServer.Slice();

					ClockProfile( MainProfile.TimerId.Network );

					// Done with this iteration.
					m_TotalProfile.Next();
					m_CurrentProfile.Next();
				}
			}
			catch ( Exception e )
			{
				HandleCrashed( e );
			}

			m_TimerThread.Stop();
		}

		private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
		{
			HandleCrashed( e.ExceptionObject as Exception );
		}

		private static void CurrentDomain_ProcessExit( object sender, EventArgs e )
		{
			HandleClosed();
		}

		private static void HandleCrashed( Exception e )
		{
			log.Error( "Error: {0}", e );

			m_Crashed = true;

			bool close = false;

			try
			{
				CrashedEventArgs args = new CrashedEventArgs( e );

				EventSink.InvokeCrashed( args );

				close = args.Close;
			}
			catch
			{
			}

			if ( !close && !m_Service )
			{
				log.Error( "This exception is fatal, press return to exit" );
				Console.ReadLine();
			}

			m_Closing = true;
		}

		public static void Kill( bool restart = false )
		{
			HandleClosed();

			if ( restart )
				Process.Start( m_ExePath );

			m_Process.Kill();
		}

		private static void HandleClosed()
		{
			if ( m_Closing )
				return;

			m_Closing = true;

			log.Info( "Exiting..." );

			if ( !m_Crashed )
				EventSink.InvokeShutdown( new ShutdownEventArgs() );

			if ( m_TimerThread != null )
				m_TimerThread.Stop();

			log.Info( "done" );
		}
	}
}
