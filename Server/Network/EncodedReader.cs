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

namespace Server.Network
{
	public class EncodedReader
	{
		private PacketReader m_Reader;

		public EncodedReader( PacketReader reader )
		{
			m_Reader = reader;
		}

		public byte[] Buffer
		{
			get
			{
				return m_Reader.Buffer;
			}
		}

		public int ReadInt32()
		{
			if ( m_Reader.ReadByte() != 0 )
				return 0;

			return m_Reader.ReadInt32();
		}

		// TODO: ReadPoint3D?

		/*
		public Point3D ReadPoint3D()
		{
			if ( m_Reader.ReadByte() != 3 )
				return Point3D.Zero;

			return new Point3D( m_Reader.ReadInt16(), m_Reader.ReadInt16(), m_Reader.ReadByte() );
		}
		*/

		public string ReadUnicodeStringSafe()
		{
			if ( m_Reader.ReadByte() != 2 )
				return "";

			int length = m_Reader.ReadUInt16();

			return m_Reader.ReadUnicodeStringSafe( length );
		}

		public string ReadUnicodeString()
		{
			if ( m_Reader.ReadByte() != 2 )
				return "";

			int length = m_Reader.ReadUInt16();

			return m_Reader.ReadUnicodeString( length );
		}
	}
}