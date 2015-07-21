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

namespace Server.Persistence
{
	public sealed class QueuedMemoryWriter : BinaryFileWriter
	{
		private struct IndexInfo
		{
			public int Size;
			public int TypeCode;
			public int Serial;
		}

		private MemoryStream m_Stream;
		private List<IndexInfo> m_OrderedIndexInfo = new List<IndexInfo>();

		protected override int BufferSize
		{
			get { return 512; }
		}

		public QueuedMemoryWriter()
			: base( new MemoryStream( 1024 * 1024 ), true )
		{
			m_Stream = this.UnderlyingStream as MemoryStream;
		}

		public void QueueForIndex( ISerializable serializable, int size )
		{
			IndexInfo info;

			info.Size = size;

			info.TypeCode = serializable.TypeReference;	// For guilds, this will automagically be zero.
			info.Serial = serializable.SerialIdentity;

			m_OrderedIndexInfo.Add( info );
		}

		public void CommitTo( FileStream dataFile, FileStream indexFile )
		{
			this.Flush();

			int memLength = (int) m_Stream.Position;

			if ( memLength > 0 )
			{
				byte[] memBuffer = m_Stream.GetBuffer();

				long actualPosition = dataFile.Position;

				dataFile.Write( memBuffer, 0, memLength ); // The buffer contains the data from many items.

				byte[] indexBuffer = new byte[20];

				for ( int i = 0; i < m_OrderedIndexInfo.Count; i++ )
				{
					IndexInfo info = m_OrderedIndexInfo[i];

					indexBuffer[0] = (byte) ( info.TypeCode );
					indexBuffer[1] = (byte) ( info.TypeCode >> 8 );
					indexBuffer[2] = (byte) ( info.TypeCode >> 16 );
					indexBuffer[3] = (byte) ( info.TypeCode >> 24 );

					indexBuffer[4] = (byte) ( info.Serial );
					indexBuffer[5] = (byte) ( info.Serial >> 8 );
					indexBuffer[6] = (byte) ( info.Serial >> 16 );
					indexBuffer[7] = (byte) ( info.Serial >> 24 );

					indexBuffer[8] = (byte) ( actualPosition );
					indexBuffer[9] = (byte) ( actualPosition >> 8 );
					indexBuffer[10] = (byte) ( actualPosition >> 16 );
					indexBuffer[11] = (byte) ( actualPosition >> 24 );
					indexBuffer[12] = (byte) ( actualPosition >> 32 );
					indexBuffer[13] = (byte) ( actualPosition >> 40 );
					indexBuffer[14] = (byte) ( actualPosition >> 48 );
					indexBuffer[15] = (byte) ( actualPosition >> 56 );

					indexBuffer[16] = (byte) ( info.Size );
					indexBuffer[17] = (byte) ( info.Size >> 8 );
					indexBuffer[18] = (byte) ( info.Size >> 16 );
					indexBuffer[19] = (byte) ( info.Size >> 24 );

					indexFile.Write( indexBuffer, 0, indexBuffer.Length );

					actualPosition += info.Size;
				}
			}

			this.Close(); // We're done with this writer.
		}
	}
}