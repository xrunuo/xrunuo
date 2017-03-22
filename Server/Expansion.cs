using System;

namespace Server
{
	public class Expansion
	{
		private static bool m_HS;

		public static bool HS
		{
			get { return m_HS; }
			set { m_HS = value; }
		}
	}
}
