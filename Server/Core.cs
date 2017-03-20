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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using Server.Events;
using Server.Network;
using Server.Profiler;

namespace Server
{
	public static class Core
	{
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

		public static void Run()
		{
			EventSink.Instance = new EventSink();

			if ( !ScriptCompiler.Compile( Environment.Debug ) )
			{
				Console.WriteLine( "Fatal: Compilation failed. Press any key to exit." );
				Console.ReadLine();
				return;
			}

			ScriptCompiler.VerifyLibraries();

			// This instance is shared among timer scheduler and timer executor,
			// and accessed from both core & timer threads.
			Queue<Timer> timerQueue = new Queue<Timer>();

			// Timer scheduler must be set up before world load, since world load
			// could schedule timers on entity deserialization.
			var timerScheduler = TimerScheduler.Instance = new TimerScheduler( timerQueue );
			m_TimerThread = new TimerThread( timerScheduler );

			TimerExecutor timerExecutor = new TimerExecutor( timerQueue );

			PacketHandlers.Instance = new PacketHandlers();

			try
			{
				ScriptCompiler.Configure();

				TileData.Configure();
			}
			catch ( TargetInvocationException e )
			{
				Console.WriteLine( "Fatal: Configure exception: {0}", e.InnerException );
				return;
			}

			Environment.SaveConfig();

			Region.Load();
			World.Instance.Load();

			try
			{
				ScriptCompiler.Initialize();
			}
			catch ( TargetInvocationException e )
			{
				Logger.Error( "Initialize exception: {0}", e.InnerException );
				return;
			}

			m_TimerThread.Start();

			NetServer netServer = new NetServer( new Listener( Listener.Port ) );
			netServer.Initialize();

			GameServer.Instance = new GameServer( netServer, PacketHandlers.Instance );
			GameServer.Instance.Initialize();

			EventSink.Instance.InvokeServerStarted();

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

		public static void HandleCrashed( Exception e )
		{
			Console.WriteLine( "Error:" );
			Console.WriteLine( e );

			m_Crashed = true;

			bool close = false;

			try
			{
				CrashedEventArgs args = new CrashedEventArgs( e );

				EventSink.Instance.InvokeCrashed( args );

				close = args.Close;
			}
			catch
			{
			}

			if ( !close && !Environment.Service )
			{
				Console.WriteLine( "This exception is fatal, press return to exit" );
				Console.ReadLine();
			}

			m_Closing = true;
		}

		public static void Kill( bool restart = false )
		{
			HandleClosed();

			if ( restart )
				Process.Start( Environment.ExePath );

			Environment.Process.Kill();
		}

		public static void HandleClosed()
		{
			if ( m_Closing )
				return;

			m_Closing = true;

			Console.Write( "Exiting..." );

			if ( !m_Crashed )
				EventSink.Instance.InvokeShutdown( new ShutdownEventArgs() );

			if ( m_TimerThread != null )
				m_TimerThread.Stop();

			Console.WriteLine( "done" );
		}
	}
}
