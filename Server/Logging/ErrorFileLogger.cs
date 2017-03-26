using System;
using System.IO;

namespace Server
{
	public class ErrorFileLogger : BaseLogger
	{
		private string m_Category;

		public ErrorFileLogger( string category )
		{
			m_Category = category;
		}

		public override void Log( LogLevel level, string msg, params object[] args )
		{
			if ( level < LogLevel.Error )
				return;

			try
			{
				using ( var op = new StreamWriter( Path.Combine( Core.Config.LogDirectory, "Exceptions.log" ), true ) )
				{
					op.WriteLine( "{0}\t{1}\t{2}\t{3}", DateTime.UtcNow, m_Category, level.ToString().ToUpper(), string.Format( msg, args ) );
				}
			}
			catch
			{
			}
		}
	}
}
