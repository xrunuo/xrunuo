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
using System.Collections.Generic;
using System.IO;

namespace Server
{
	public class UopIndex
	{
		private class UopEntry : IComparable<UopEntry>
		{
			public int m_Offset;
			public int m_Length;
			public int m_Order;

			public UopEntry( int offset, int length )
			{
				m_Offset = offset;
				m_Length = length;
				m_Order = 0;
			}

			public int CompareTo( UopEntry other )
			{
				return m_Order.CompareTo( other.m_Order );
			}
		}

		private class OffsetComparer : IComparer<UopEntry>
		{
			public static readonly IComparer<UopEntry> Instance = new OffsetComparer();

			public OffsetComparer()
			{
			}

			public int Compare( UopEntry x, UopEntry y )
			{
				return x.m_Offset.CompareTo( y.m_Offset );
			}
		}

		private BinaryReader m_Reader;
		private int m_Length;
		private int m_Version;
		private UopEntry[] m_Entries;

		public int Version
		{
			get { return m_Version; }
		}

		public UopIndex( FileStream stream )
		{
			m_Reader = new BinaryReader( stream );
			m_Length = (int) stream.Length;

			if ( m_Reader.ReadInt32() != 0x50594D )
				throw new ArgumentException( "Invalid UOP file." );

			m_Version = m_Reader.ReadInt32();
			m_Reader.ReadInt32();
			int nextTable = m_Reader.ReadInt32();

			List<UopEntry> entries = new List<UopEntry>();

			do
			{
				stream.Seek( nextTable, SeekOrigin.Begin );
				int count = m_Reader.ReadInt32();
				nextTable = m_Reader.ReadInt32();
				m_Reader.ReadInt32();

				for ( int i = 0; i < count; ++i )
				{
					int offset = m_Reader.ReadInt32();

					if ( offset == 0 )
					{
						stream.Seek( 30, SeekOrigin.Current );
						continue;
					}

					m_Reader.ReadInt64();
					int length = m_Reader.ReadInt32();

					entries.Add( new UopEntry( offset, length ) );

					stream.Seek( 18, SeekOrigin.Current );
				}
			}
			while ( nextTable != 0 && nextTable < m_Length );

			entries.Sort( OffsetComparer.Instance );

			for ( int i = 0; i < entries.Count; ++i )
			{
				stream.Seek( entries[i].m_Offset + 2, SeekOrigin.Begin );

				int dataOffset = m_Reader.ReadInt16();
				entries[i].m_Offset += 4 + dataOffset;

				stream.Seek( dataOffset, SeekOrigin.Current );
				entries[i].m_Order = m_Reader.ReadInt32();
			}

			entries.Sort();
			m_Entries = entries.ToArray();
		}

		public int Lookup( int offset )
		{
			int total = 0;

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				int newTotal = total + m_Entries[i].m_Length;

				if ( offset < newTotal )
					return m_Entries[i].m_Offset + ( offset - total );

				total = newTotal;
			}

			return m_Length;
		}

		public void Close()
		{
			m_Reader.Close();
		}
	}
}
