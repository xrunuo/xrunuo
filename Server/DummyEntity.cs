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

namespace Server
{
	public class DummyEntity : IEntity
	{
		private Serial m_Serial;
		private IPoint3D m_Location;
		private IMap m_Map;

		public DummyEntity( Serial serial, IPoint3D loc, IMap map )
		{
			m_Serial = serial;
			m_Location = loc;
			m_Map = map;
		}

		public Serial Serial
		{
			get
			{
				return m_Serial;
			}
		}

		public IPoint3D Location
		{
			get
			{
				return m_Location;
			}
		}

		public IMap Map
		{
			get
			{
				return m_Map;
			}
		}

		public int X
		{
			get
			{
				return m_Location.X;
			}
		}

		public int Y
		{
			get
			{
				return m_Location.Y;
			}
		}

		public int Z
		{
			get
			{
				return m_Location.Z;
			}
		}
	}
}