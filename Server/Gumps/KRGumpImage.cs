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
	public class KRGumpImage : GumpEntry
	{
		private int m_X, m_Y;
		private int m_GumpID;

		public KRGumpImage( int x, int y, int gumpID )
		{
			m_X = x;
			m_Y = y;
			m_GumpID = gumpID;
		}

		public int X
		{
			get
			{
				return m_X;
			}
			set
			{
				Delta( ref m_X, value );
			}
		}

		public int Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				Delta( ref m_Y, value );
			}
		}

		public int GumpID
		{
			get
			{
				return m_GumpID;
			}
			set
			{
				Delta( ref m_GumpID, value );
			}
		}


		public override string Compile()
		{
			return String.Format( "{{ kr_gumppic {0} {1} {2} }}", m_X, m_Y, m_GumpID );
		}

		private static byte[] m_LayoutName = Gump.StringToBuffer( "kr_gumppic" );

		public override void AppendTo( IGumpWriter disp )
		{
			disp.AppendLayout( m_LayoutName );
			disp.AppendLayout( m_X );
			disp.AppendLayout( m_Y );
			disp.AppendLayout( m_GumpID );
		}
	}
}