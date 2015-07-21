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
using System.Collections;

namespace Server.Configuration
{
	public class Features
	{
		private Hashtable m_Table = new Hashtable();

		public bool this[string name]
		{
			get
			{
				return m_Table.Contains( name );
			}
			set
			{
				if ( value )
					m_Table[name] = true;
				else
					m_Table.Remove( name );
			}
		}
	}
}