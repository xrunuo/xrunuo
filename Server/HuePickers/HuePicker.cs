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

		public void SendTo( GameClient state )
		{
			state.Send( new DisplayHuePicker( this ) );
			state.AddHuePicker( this );
		}
	}
}