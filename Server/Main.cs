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

		private static Thread m_Thread;

		private static string m_BaseDirectory;
		private static string m_ExePath;

		private static bool m_Profiling;
		private static DateTime m_ProfileStart;
		private static TimeSpan m_ProfileTime;

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

		public static bool Unix { get; private set; }

		public static readonly bool Is64Bit = IntPtr.Size == 8;
		private static readonly int ProcessorCount = Environment.ProcessorCount;

		private static bool MultiProcessor => ProcessorCount > 1;

		public static bool Logging { get; set; }

		public static bool Service { get; private set; }

		public static bool Debug { get; private set; }

		public static List<string> DataDirectories => Config.DataDirectories;

		public static Assembly Assembly { get; set; }

		public static Process Process { get; private set; }

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

		public static RootConfig Config { get; private set; }

		#region Dependency management
		public static LibraryConfig LibraryConfig { get; private set; }

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
					Debug = true;
				else if ( Insensitive.Equals( args[i], "--service" ) )
					Service = true;
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
				if ( Service )
				{
					if ( !Directory.Exists( Config.LogDirectory ) )
						Directory.CreateDirectory( Config.LogDirectory );

					Console.SetOut( new MultiTextWriter( Console.Out,
						new FileLogger( Path.Combine( Config.LogDirectory, "Console.log" ) ) ) );
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
			if ( !Config.Exists )
				Config.Save();

			if ( !LibraryConfig.Exists )
				LibraryConfig.Save();
		}

		private static bool m_Crashed;

		public static bool Closing { get; private set; }

		private static TimerThread m_TimerThread;

		/* current time */
		public static DateTime Now { get; private set; } = DateTime.UtcNow;

		/* main loop profiler */
		private static MainProfile m_TotalProfile;
		private static MainProfile m_CurrentProfile;

		public static MainProfile TotalProfile => m_TotalProfile;

		public static MainProfile CurrentProfile => m_CurrentProfile;

		public static void ResetCurrentProfile()
		{
			m_CurrentProfile = new MainProfile( Now );
		}

		private static void ClockProfile( MainProfile.TimerId id )
		{
			DateTime prev = Now;
			Now = DateTime.UtcNow;

			TimeSpan diff = Now - prev;
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
			Process = Process.GetCurrentProcess();
			Assembly = Assembly.GetEntryAssembly();

			if ( m_Thread != null )
				m_Thread.Name = "Core Thread";

			if ( BaseDirectory.Length > 0 )
				Directory.SetCurrentDirectory( BaseDirectory );

			var version = Assembly.GetName().Version;
			CoreVersion = version;

			var platform = (int) Environment.OSVersion.Platform;
			if ( platform == 4 || platform == 128 )
				Unix = true;

			GCSettings.LatencyMode = GCLatencyMode.LowLatency;

			log.Info( "X-RunUO Server - Version {0}.{1}.{2}, Build {3}", version.Major, version.Minor, version.Build, version.Revision );
			log.Info( "Running on OS {0}", Environment.OSVersion );
			log.Info( "Running on {0} {1}", Unix ? "Mono" : ".NET Framework", Environment.Version );

			if ( MultiProcessor || Is64Bit )
				log.Info( "Optimizing for {0} {2}processor{1}", ProcessorCount, ProcessorCount == 1 ? "" : "s", Is64Bit ? "64-bit " : "" );

			log.Info( "Using GC {0} {1} mode", GCSettings.IsServerGC ? "Server" : "Workstation", GCSettings.LatencyMode );

			Config = new RootConfig( BaseDirectory, "x-runuo.xml" );

			Server.Config.Load();

			#region Dependency management
			LibraryConfig = new LibraryConfig( BaseDirectory, "libraries.xml" );

			if ( ForceUpdateDeps )
				Directory.Delete( Path.Combine( BaseDirectory, "deps" ), recursive: true );
			#endregion

			if ( !ScriptCompiler.Compile( Debug ) )
			{
				log.Fatal( "Compilation failed. Press any key to exit." );
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
				log.Fatal( "Configure exception: {0}", e.InnerException );
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

			Now = DateTime.UtcNow;
			m_TotalProfile = new MainProfile( Now );
			m_CurrentProfile = new MainProfile( Now );

			try
			{
				while ( !Closing )
				{
					Now = DateTime.UtcNow;

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

			if ( !close && !Service )
			{
				log.Error( "This exception is fatal, press return to exit" );
				Console.ReadLine();
			}

			Closing = true;
		}

		public static void Kill( bool restart = false )
		{
			HandleClosed();

			if ( restart )
				Process.Start( m_ExePath );

			Process.Kill();
		}

		private static void HandleClosed()
		{
			if ( Closing )
				return;

			Closing = true;

			log.Info( "Exiting..." );

			if ( !m_Crashed )
				EventSink.InvokeShutdown( new ShutdownEventArgs() );

			if ( m_TimerThread != null )
				m_TimerThread.Stop();

			log.Info( "done" );
		}
	}
}
