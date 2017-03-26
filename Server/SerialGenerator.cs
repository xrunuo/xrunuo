using System;

namespace Server
{
	public class SerialGenerator
	{
		private static Serial m_LastMobile = 0x00000000;
		private static Serial m_LastItem = 0x40000000;

		public static Serial GetNewMobileSerial()
		{
			while ( World.FindMobile( ++m_LastMobile ) != null )
				;

			return m_LastMobile;
		}

		public static Serial GetNewItemSerial()
		{
			while ( World.FindItem( ++m_LastItem ) != null )
				;

			return m_LastItem;
		}
	}
}
