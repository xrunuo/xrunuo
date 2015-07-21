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
using System.Text;

namespace Server
{
	public class MultiTextWriter : TextWriter
	{
		private List<TextWriter> m_Streams;

		public MultiTextWriter( params TextWriter[] streams )
		{
			m_Streams = new List<TextWriter>( streams );

			if ( m_Streams.Count < 0 )
				throw new ArgumentException( "You must specify at least one stream." );
		}

		public void Add( TextWriter tw )
		{
			m_Streams.Add( tw );
		}

		public void Remove( TextWriter tw )
		{
			m_Streams.Remove( tw );
		}

		public override void Write( char ch )
		{
			for ( int i = 0; i < m_Streams.Count; i++ )
				m_Streams[i].Write( ch );
		}

		public override void WriteLine( string line )
		{
			for ( int i = 0; i < m_Streams.Count; i++ )
				m_Streams[i].WriteLine( line );
		}

		public override void WriteLine( string line, params object[] args )
		{
			WriteLine( String.Format( line, args ) );
		}

		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}
	}
}
