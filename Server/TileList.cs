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
	public class TileList
	{
		private Tile[] m_Tiles;
		private int m_Count;

		public TileList()
		{
			m_Tiles = new Tile[8];
			m_Count = 0;
		}

		public int Count
		{
			get
			{
				return m_Count;
			}
		}

		public void AddRange( Tile[] tiles )
		{
			if ( ( m_Count + tiles.Length ) > m_Tiles.Length )
			{
				Tile[] old = m_Tiles;
				m_Tiles = new Tile[( m_Count + tiles.Length ) * 2];

				for ( int i = 0; i < old.Length; ++i )
					m_Tiles[i] = old[i];
			}

			for ( int i = 0; i < tiles.Length; ++i )
				m_Tiles[m_Count++] = tiles[i];
		}

		public void Add( ushort id, sbyte z )
		{
			if ( ( m_Count + 1 ) > m_Tiles.Length )
			{
				Tile[] old = m_Tiles;
				m_Tiles = new Tile[old.Length * 2];

				for ( int i = 0; i < old.Length; ++i )
					m_Tiles[i] = old[i];
			}

			m_Tiles[m_Count].m_ID = id;
			m_Tiles[m_Count].Z = z;
			++m_Count;
		}

		private static Tile[] m_EmptyTiles = new Tile[0];

		public Tile[] ToArray()
		{
			if ( m_Count == 0 )
				return m_EmptyTiles;

			Tile[] tiles = new Tile[m_Count];

			for ( int i = 0; i < m_Count; ++i )
				tiles[i] = m_Tiles[i];

			m_Count = 0;

			return tiles;
		}
	}
}