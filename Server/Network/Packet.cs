using System;
using System.Diagnostics;
using System.IO;

namespace Server.Network
{
	public abstract class Packet
	{
		[Flags]
		private enum State
		{
			Inactive = 0x00,
			Static = 0x01,
			Acquired = 0x02,
			Accessed = 0x04,
			Buffered = 0x08,
			Warned = 0x10
		}

		protected PacketWriter m_Stream;
		private int m_PacketID;
		private int m_Length;
		private State m_State;

		public int Length
		{
			get { return m_Length; }
		}

		public int PacketID
		{
			get { return m_PacketID; }
		}

		public void EnsureCapacity( int length )
		{
			m_Stream = PacketWriter.CreateInstance( length );// new PacketWriter( length );
			m_Stream.Write( (byte) m_PacketID );
			m_Stream.Write( (short) 0 );
		}

		public Packet()
		{
		}

		public Packet( int packetID )
		{
			Initialize( packetID, 0 );
		}

		public Packet( int packetID, int length )
		{
			Initialize( packetID, length );
		}

		protected void Initialize( int packetID, int length )
		{
			m_PacketID = packetID;
			m_Length = length;
			m_State = State.Inactive;

			if ( m_Length > 0 )
			{
				m_Stream = PacketWriter.CreateInstance( length );
				m_Stream.Write( (byte) packetID );
			}

			// TODO: raise an event OnInitialize, in order to do profiling stuff.
		}

		public PacketWriter Stream
		{
			get
			{
				return m_Stream;
			}
		}

		private const int BufferSize = 4096;
		private static BufferPool m_Buffers = new BufferPool( "Compressed", 16, BufferSize );

		public static Packet SetStatic( Packet p )
		{
			p.SetStatic();
			return p;
		}

		public static Packet Acquire( Packet p )
		{
			p.Acquire();
			return p;
		}

		public static void Release( ref Packet p )
		{
			if ( p != null )
				p.Release();

			p = null;
		}

		public static void Release( Packet p )
		{
			if ( p != null )
				p.Release();
		}

		public void SetStatic()
		{
			m_State |= State.Static | State.Acquired;
		}

		public void Acquire()
		{
			m_State |= State.Acquired;
		}

		public void OnSend()
		{
			if ( ( m_State & ( State.Acquired | State.Static ) ) == 0 )
				Free();
		}

		protected virtual void Free()
		{
			if ( m_CompiledBuffer == null )
				return;

			if ( ( m_State & State.Buffered ) != 0 )
				m_Buffers.ReleaseBuffer( m_CompiledBuffer );

			m_State = ( (State) ( (int) m_State ) ) & ( (State) ( -12 ) );

			m_CompiledBuffer = null;
		}

		public void Release()
		{
			if ( ( m_State & State.Acquired ) != 0 )
				Free();
		}

		private byte[] m_CompiledBuffer;
		private int m_CompiledLength;

		public byte[] Compile( bool compress, out int length )
		{
			if ( m_CompiledBuffer == null )
			{
				if ( ( m_State & State.Accessed ) == 0 )
				{
					m_State |= State.Accessed;
				}
				else
				{
					if ( ( m_State & State.Warned ) == 0 )
					{
						m_State |= State.Warned;

						try
						{
							using ( StreamWriter writer = new StreamWriter( Path.Combine( Core.Config.LogDirectory, "net_opt.log" ), true ) )
							{
								writer.WriteLine( "Redundant compile for packet {0}, use Acquire() and Release()", base.GetType() );
								writer.WriteLine( new StackTrace() );
							}
						}
						catch
						{
						}
					}

					m_CompiledBuffer = new byte[0];
					m_CompiledLength = 0;

					length = m_CompiledLength;
					return m_CompiledBuffer;
				}

				InternalCompile( compress );
			}

			length = m_CompiledLength;
			return m_CompiledBuffer;
		}

		private void InternalCompile( bool compress )
		{
			if ( m_Length == 0 )
			{
				long streamLen = m_Stream.Length;

				m_Stream.Seek( 1, SeekOrigin.Begin );
				m_Stream.Write( (ushort) streamLen );
			}
			else if ( m_Stream.Length != m_Length )
			{
				int diff = (int) m_Stream.Length - m_Length;

				Console.WriteLine( "Packet: 0x{0:X2}: Bad packet length! ({1}{2} bytes)", m_PacketID, diff >= 0 ? "+" : "", diff );
			}

			MemoryStream ms = m_Stream.UnderlyingStream;

			m_CompiledBuffer = ms.GetBuffer();
			int length = (int) ms.Length;

			if ( compress )
			{
				try
				{
					Compression.Compress( m_CompiledBuffer, length, out m_CompiledBuffer, out length );
				}
				catch ( IndexOutOfRangeException )
				{
					Console.WriteLine( "Warning: Compression buffer overflowed on packet 0x{0:X2} ('{1}') (length={2})", m_PacketID, GetType().Name, length );

					m_CompiledBuffer = null;
				}
			}

			if ( m_CompiledBuffer != null )
			{
				m_CompiledLength = length;

				byte[] old = m_CompiledBuffer;

				if ( length > BufferSize || ( m_State & State.Static ) != 0 )
				{
					m_CompiledBuffer = new byte[length];
				}
				else
				{
					m_CompiledBuffer = m_Buffers.AcquireBuffer();
					m_State |= State.Buffered;
				}

				Buffer.BlockCopy( old, 0, m_CompiledBuffer, 0, length );
			}

			PacketWriter.ReleaseInstance( m_Stream );
			m_Stream = null;
		}
	}
}
