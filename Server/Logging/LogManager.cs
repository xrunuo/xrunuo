using System;
using System.Collections.Generic;

namespace Server
{
	public class LogManager
	{
		private static readonly Dictionary<Type, ILog> m_Loggers = new Dictionary<Type, ILog>();

		public static ILog GetLogger( Type declaringType )
		{
			if ( m_Loggers.ContainsKey( declaringType ) )
				return m_Loggers[declaringType];
			else
				return m_Loggers[declaringType] = CreateLogger( declaringType );
		}

		private static ILog CreateLogger( Type declaringType )
		{
			var category = declaringType.Name;

			return new CompositeLogger(
				new ConsoleLogger( category ),
				new ErrorFileLogger( category )
			);
		}
	}
}
