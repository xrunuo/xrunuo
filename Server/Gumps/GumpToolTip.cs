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
	public class GumpTooltip : GumpEntry
	{
		private int m_Number;
		private string m_Args;

		public GumpTooltip( int number )
			: this( number, null )
		{
		}

		public GumpTooltip( int number, string args )
		{
			m_Number = number;
			m_Args = args;
		}

		public int Number
		{
			get
			{
				return m_Number;
			}
			set
			{
				base.Delta( ref m_Number, value );
			}
		}

		public string Args
		{
			get
			{
				return m_Args;
			}
			set
			{
				Delta( ref m_Args, value );
			}
		}

		public override string Compile()
		{
			if ( string.IsNullOrEmpty( m_Args ) )
				return string.Format( "{{ tooltip {0} }}", m_Number );
			else
				return string.Format( "{{ tooltip {0} @{1}@ }}", m_Number, m_Args );
		}

		private static byte[] m_LayoutName = Gump.StringToBuffer( "tooltip" );

		public override void AppendTo( IGumpWriter disp )
		{
			disp.AppendLayout( m_LayoutName );
			disp.AppendLayout( m_Number );

			if ( !string.IsNullOrEmpty( m_Args ) )
				disp.AppendLayout( m_Args );
		}
	}
}
