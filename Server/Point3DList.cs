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
	public class Point3DList
	{
		private Point3D[] m_List;
		private int m_Count;

		public Point3DList()
		{
			m_List = new Point3D[8];
			m_Count = 0;
		}

		public int Count
		{
			get
			{
				return m_Count;
			}
		}

		public void Clear()
		{
			m_Count = 0;
		}

		public Point3D Last
		{
			get { return m_List[m_Count - 1]; }
		}

		public Point3D this[int index]
		{
			get
			{
				return m_List[index];
			}
		}

		public void Add( int x, int y, int z )
		{
			if ( ( m_Count + 1 ) > m_List.Length )
			{
				Point3D[] old = m_List;
				m_List = new Point3D[old.Length * 2];

				for ( int i = 0; i < old.Length; ++i )
					m_List[i] = old[i];
			}

			m_List[m_Count].X = x;
			m_List[m_Count].Y = y;
			m_List[m_Count].Z = z;
			++m_Count;
		}

		public void Add( Point3D p )
		{
			if ( ( m_Count + 1 ) > m_List.Length )
			{
				Point3D[] old = m_List;
				m_List = new Point3D[old.Length * 2];

				for ( int i = 0; i < old.Length; ++i )
					m_List[i] = old[i];
			}

			m_List[m_Count].X = p.X;
			m_List[m_Count].Y = p.Y;
			m_List[m_Count].Z = p.Z;
			++m_Count;
		}

		private static Point3D[] m_EmptyList = new Point3D[0];

		public Point3D[] ToArray()
		{
			if ( m_Count == 0 )
				return m_EmptyList;

			Point3D[] list = new Point3D[m_Count];

			for ( int i = 0; i < m_Count; ++i )
				list[i] = m_List[i];

			m_Count = 0;

			return list;
		}
	}
}