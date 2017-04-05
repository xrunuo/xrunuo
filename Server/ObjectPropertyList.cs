using System;
using System.Text;
using Server;
using Server.Network;

namespace Server
{
	public interface ObjectPropertyList
	{
		void Add( int number );
		void Add( int number, string arguments );
		void Add( int number, string format, params object[] args );
		void Add( string text );
		void Add( string format, params object[] args );
	}

	public class ObjectPropertyListPacket : Packet, ObjectPropertyList
	{
		private int m_Hash;

		public IEntity Entity { get; }

		public int Hash => 0x40000000 + m_Hash;

		public int Header { get; set; }

		public string HeaderArgs { get; set; }

		public static bool Enabled { get; set; }

		public ObjectPropertyListPacket( IEntity e )
			: base( 0xD6 )
		{
			EnsureCapacity( 128 );

			Entity = e;

			m_Stream.Write( (short) 1 );
			m_Stream.Write( (int) e.Serial );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (int) e.Serial );
		}

		public void Add( int number )
		{
			if ( number == 0 )
				return;

			AddHash( number );

			if ( Header == 0 )
			{
				Header = number;
				HeaderArgs = "";
			}

			m_Stream.Write( number );
			m_Stream.Write( (short) 0 );
		}

		public void Terminate()
		{
			m_Stream.Write( (int) 0 );

			m_Stream.Seek( 11, System.IO.SeekOrigin.Begin );
			m_Stream.Write( (int) m_Hash );
		}

		private static byte[] m_Buffer = new byte[1024];
		private static readonly Encoding m_Encoding = Encoding.Unicode;

		public void AddHash( int val )
		{
			m_Hash ^= ( val & 0x3FFFFFF );
			m_Hash ^= ( val >> 26 ) & 0x3F;
		}

		public void Add( int number, string arguments )
		{
			if ( number == 0 )
				return;

			if ( arguments == null )
				arguments = "";

			if ( Header == 0 )
			{
				Header = number;
				HeaderArgs = arguments;
			}

			AddHash( number );
			AddHash( arguments.GetHashCode() );

			m_Stream.Write( number );

			int byteCount = m_Encoding.GetByteCount( arguments );

			if ( byteCount > m_Buffer.Length )
				m_Buffer = new byte[byteCount];

			byteCount = m_Encoding.GetBytes( arguments, 0, arguments.Length, m_Buffer, 0 );

			m_Stream.Write( (short) byteCount );
			m_Stream.Write( m_Buffer, 0, byteCount );
		}

		public void Add( int number, string format, params object[] args )
		{
			Add( number, String.Format( format, args ) );
		}

		public void Add( string text )
		{
			Add( 1042971, text );
		}

		public void Add( string format, params object[] args )
		{
			Add( 1042971, String.Format( format, args ) );
		}
	}

	public class OPLInfo : Packet
	{
		public OPLInfo( ObjectPropertyListPacket list )
			: base( 0xDC, 9 )
		{
			m_Stream.Write( (int) list.Entity.Serial );
			m_Stream.Write( (int) list.Hash );
		}
	}
}