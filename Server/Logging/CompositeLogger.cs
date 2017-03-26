using System;
using System.Collections.Generic;

namespace Server
{
	public class CompositeLogger : BaseLogger
	{
		private IEnumerable<ILog> m_Loggers;

		public CompositeLogger( params ILog[] loggers )
		{
			m_Loggers = loggers;
		}

		public override void Log( LogLevel level, string msg, params object[] args )
		{
			foreach ( var logger in m_Loggers )
			{
				logger.Log( level, msg, args );
			}
		}
	}
}
