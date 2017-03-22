using System;

namespace Server
{
	public static class Program
	{
		public static void Main( string[] args )
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

			Environment.Initialize( args );

			Core.Run();
		}

		private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
		{
			Core.HandleCrashed( e.ExceptionObject as Exception );
		}

		private static void CurrentDomain_ProcessExit( object sender, EventArgs e )
		{
			Core.HandleClosed();
		}
	}
}
