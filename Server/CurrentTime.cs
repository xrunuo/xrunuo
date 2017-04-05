using System;

namespace Server
{
	public class CurrentTime
	{
		private static DateTime m_Override;

		public static DateTime Now
		{
			get
			{
				if ( m_Override != default( DateTime ) )
					return m_Override;

				return DateTime.UtcNow;
			}
			set { m_Override = value; }
		}
	}
}
