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
	public class KeywordList
	{
		private int[] m_Keywords;
		private int m_Count;

		public KeywordList()
		{
			m_Keywords = new int[8];
			m_Count = 0;
		}

		public int Count
		{
			get
			{
				return m_Count;
			}
		}

		public bool Contains( int keyword )
		{
			bool contains = false;

			for ( int i = 0; !contains && i < m_Count; ++i )
				contains = ( keyword == m_Keywords[i] );

			return contains;
		}

		public void Add( int keyword )
		{
			if ( ( m_Count + 1 ) > m_Keywords.Length )
			{
				int[] old = m_Keywords;
				m_Keywords = new int[old.Length * 2];

				for ( int i = 0; i < old.Length; ++i )
					m_Keywords[i] = old[i];
			}

			m_Keywords[m_Count++] = keyword;
		}

		private static int[] m_EmptyInts = new int[0];

		public int[] ToArray()
		{
			if ( m_Count == 0 )
				return m_EmptyInts;

			int[] keywords = new int[m_Count];

			for ( int i = 0; i < m_Count; ++i )
				keywords[i] = m_Keywords[i];

			m_Count = 0;

			return keywords;
		}
	}
}