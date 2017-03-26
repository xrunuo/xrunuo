using System;

namespace Server
{
	public class ConsoleLogger : BaseLogger
	{
		private string m_Category;

		public ConsoleLogger( string category )
		{
			m_Category = category;
		}

		public override void Log( LogLevel level, string msg, params object[] args )
		{
			var color = GetColorForLevel( level );

			if ( color != null )
				Utility.PushColor( color.Value );

			Console.WriteLine( "{0}: {1}", m_Category, string.Format( msg, args ) );

			if ( color != null )
				Utility.PopColor();
		}

		private static ConsoleColor? GetColorForLevel( LogLevel level )
		{
			switch ( level )
			{
				case LogLevel.Debug:
					return ConsoleColor.Yellow;

				case LogLevel.Warn:
					return ConsoleColor.DarkYellow;

				case LogLevel.Error:
					return ConsoleColor.DarkRed;

				case LogLevel.Fatal:
					return ConsoleColor.Red;
			}

			return null;
		}
	}
}
