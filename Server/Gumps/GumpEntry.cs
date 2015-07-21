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
	public abstract class GumpEntry
	{
		private Gump m_Parent;

		public GumpEntry()
		{
		}

		protected void Delta( ref int var, int val )
		{
			if ( var != val )
				var = val;
		}

		protected void Delta( ref bool var, bool val )
		{
			if ( var != val )
				var = val;
		}

		protected void Delta( ref string var, string val )
		{
			if ( var != val )
				var = val;
		}

		public Gump Parent
		{
			get
			{
				return m_Parent;
			}
			set
			{
				if ( m_Parent != value )
				{
					if ( m_Parent != null )
						m_Parent.Remove( this );

					m_Parent = value;

					m_Parent.Add( this );
				}
			}
		}

		public abstract string Compile();
		public abstract void AppendTo( IGumpWriter disp );
	}
}