using System;
using Server.Network;

namespace Server.HuePickers
{
	public class HuePicker
	{
		private static int m_NextSerial = 1;

		private int m_Serial;
		private int m_ItemID;

		public int Serial
		{
			get
			{
				return m_Serial;
			}
		}

		public int ItemID
		{
			get
			{
				return m_ItemID;
			}
		}

		public HuePicker( int itemID )
		{
			do
			{
				m_Serial = m_NextSerial++;
			} while ( m_Serial == 0 );

			m_ItemID = itemID;
		}

		public virtual void OnResponse( int hue )
		{
		}

		public void SendTo( NetState state )
		{
			state.Send( new DisplayHuePicker( this ) );
			state.AddHuePicker( this );
		}
	}
}