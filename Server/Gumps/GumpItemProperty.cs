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

namespace Server.Gumps
{
	public class GumpItemProperty : GumpEntry
	{
		private int m_Serial;

		public GumpItemProperty( int serial )
		{
			m_Serial = serial;
		}

		public int Serial
		{
			get
			{
				return m_Serial;
			}
			set
			{
				Delta( ref m_Serial, value );
			}
		}

		public override string Compile()
		{
			return String.Format( "{{ itemproperty {0} }}", m_Serial.ToString() );
		}

		private static byte[] m_LayoutName = Gump.StringToBuffer( "itemproperty" );

		public override void AppendTo( IGumpWriter disp )
		{
			disp.AppendLayout( m_LayoutName );
			disp.AppendLayout( m_Serial );
		}
	}
}