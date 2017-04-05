using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
	public class MultiTextWriter : TextWriter
	{
		private readonly List<TextWriter> m_Streams;

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

		public override Encoding Encoding => Encoding.Default;
	}
}
