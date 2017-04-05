using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Network
{
	public class PacketReader
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static readonly Stack<PacketReader> m_Pool = new Stack<PacketReader>();

		public static PacketReader CreateInstance( byte[] data, int size, bool fixedSize )
		{
			PacketReader pr = null;

			lock ( m_Pool )
			{
				if ( m_Pool.Count > 0 )
				{
					pr = m_Pool.Pop();

					if ( pr != null )
					{
						pr.Buffer = data;
						pr.Size = size;
						pr.m_Index = fixedSize ? 1 : 3;
					}
				}
			}

			if ( pr == null )
				pr = new PacketReader( data, size, fixedSize );

			return pr;
		}

		public static void ReleaseInstance( PacketReader pr )
		{
			lock ( m_Pool )
			{
				if ( !m_Pool.Contains( pr ) )
				{
					m_Pool.Push( pr );
				}
				else
				{
					try
					{
						using ( var op = new StreamWriter( Path.Combine( Core.Config.LogDirectory, "neterr.log" ) ) )
							op.WriteLine( "{0}\tInstance pool contains reader", DateTime.UtcNow );
					}
					catch
					{
						log.Error( "net error" );
					}
				}
			}
		}

		private int m_Index;

		private PacketReader( byte[] data, int size, bool fixedSize )
		{
			Buffer = data;
			Size = size;
			m_Index = fixedSize ? 1 : 3;
		}

		public byte[] Buffer { get; private set; }

		public int Size { get; private set; }

		public int Seek( int offset, SeekOrigin origin )
		{
			switch ( origin )
			{
				case SeekOrigin.Begin: m_Index = offset; break;
				case SeekOrigin.Current: m_Index += offset; break;
				case SeekOrigin.End: m_Index = Size - offset; break;
			}

			return m_Index;
		}

		public int ReadInt32()
		{
			if ( ( m_Index + 4 ) > Size )
				return 0;

			return ( Buffer[m_Index++] << 24 )
				 | ( Buffer[m_Index++] << 16 )
				 | ( Buffer[m_Index++] << 8 )
				 | Buffer[m_Index++];
		}

		public short ReadInt16()
		{
			if ( ( m_Index + 2 ) > Size )
				return 0;

			return (short) ( ( Buffer[m_Index++] << 8 ) | Buffer[m_Index++] );
		}

		public byte ReadByte()
		{
			if ( ( m_Index + 1 ) > Size )
				return 0;

			return Buffer[m_Index++];
		}

		public uint ReadUInt32()
		{
			if ( ( m_Index + 4 ) > Size )
				return 0;

			return (uint) ( ( Buffer[m_Index++] << 24 ) | ( Buffer[m_Index++] << 16 ) | ( Buffer[m_Index++] << 8 ) | Buffer[m_Index++] );
		}

		public ushort ReadUInt16()
		{
			if ( ( m_Index + 2 ) > Size )
				return 0;

			return (ushort) ( ( Buffer[m_Index++] << 8 ) | Buffer[m_Index++] );
		}

		public sbyte ReadSByte()
		{
			if ( ( m_Index + 1 ) > Size )
				return 0;

			return (sbyte) Buffer[m_Index++];
		}

		public bool ReadBoolean()
		{
			if ( ( m_Index + 1 ) > Size )
				return false;

			return ( Buffer[m_Index++] != 0 );
		}

		public string ReadUnicodeStringLE()
		{
			var sb = new StringBuilder();

			int c;

			while ( ( m_Index + 1 ) < Size && ( c = ( Buffer[m_Index++] | ( Buffer[m_Index++] << 8 ) ) ) != 0 )
				sb.Append( (char) c );

			return sb.ToString();
		}

		public string ReadUnicodeStringLESafe( int fixedLength )
		{
			var bound = m_Index + ( fixedLength << 1 );
			var end = bound;

			if ( bound > Size )
				bound = Size;

			var sb = new StringBuilder();

			int c;

			while ( ( m_Index + 1 ) < bound && ( c = ( Buffer[m_Index++] | ( Buffer[m_Index++] << 8 ) ) ) != 0 )
			{
				if ( IsSafeChar( c ) )
					sb.Append( (char) c );
			}

			m_Index = end;

			return sb.ToString();
		}

		public string ReadUnicodeStringLESafe()
		{
			var sb = new StringBuilder();

			int c;

			while ( ( m_Index + 1 ) < Size && ( c = ( Buffer[m_Index++] | ( Buffer[m_Index++] << 8 ) ) ) != 0 )
			{
				if ( IsSafeChar( c ) )
					sb.Append( (char) c );
			}

			return sb.ToString();
		}

		public string ReadUnicodeStringSafe()
		{
			var sb = new StringBuilder();

			int c;

			while ( ( m_Index + 1 ) < Size && ( c = ( ( Buffer[m_Index++] << 8 ) | Buffer[m_Index++] ) ) != 0 )
			{
				if ( IsSafeChar( c ) )
					sb.Append( (char) c );
			}

			return sb.ToString();
		}

		public string ReadUnicodeString()
		{
			var sb = new StringBuilder();

			int c;

			while ( ( m_Index + 1 ) < Size && ( c = ( ( Buffer[m_Index++] << 8 ) | Buffer[m_Index++] ) ) != 0 )
				sb.Append( (char) c );

			return sb.ToString();
		}

		public bool IsSafeChar( int c )
		{
			return ( c >= 0x20 && c < 0xFFFE );
		}

		public string ReadUTF8StringSafe( int fixedLength )
		{
			if ( m_Index >= Size )
			{
				m_Index += fixedLength;
				return String.Empty;
			}

			var bound = m_Index + fixedLength;

			if ( bound > Size )
				bound = Size;

			var count = 0;
			var index = m_Index;
			var start = m_Index;

			while ( index < bound && Buffer[index++] != 0 )
				++count;

			index = 0;

			var buffer = new byte[count];
			var value = 0;

			while ( m_Index < bound && ( value = Buffer[m_Index++] ) != 0 )
				buffer[index++] = (byte) value;

			var s = Utility.UTF8.GetString( buffer );

			var isSafe = true;

			for ( var i = 0; isSafe && i < s.Length; ++i )
				isSafe = IsSafeChar( (int) s[i] );

			m_Index = start + fixedLength;

			if ( isSafe )
				return s;

			var sb = new StringBuilder( s.Length );

			for ( var i = 0; i < s.Length; ++i )
				if ( IsSafeChar( (int) s[i] ) )
					sb.Append( s[i] );

			return sb.ToString();
		}

		public string ReadUTF8StringSafe()
		{
			if ( m_Index >= Size )
				return String.Empty;

			var count = 0;
			var index = m_Index;

			while ( index < Size && Buffer[index++] != 0 )
				++count;

			index = 0;

			var buffer = new byte[count];
			var value = 0;

			while ( m_Index < Size && ( value = Buffer[m_Index++] ) != 0 )
				buffer[index++] = (byte) value;

			var s = Utility.UTF8.GetString( buffer );

			var isSafe = true;

			for ( var i = 0; isSafe && i < s.Length; ++i )
				isSafe = IsSafeChar( (int) s[i] );

			if ( isSafe )
				return s;

			var sb = new StringBuilder( s.Length );

			for ( var i = 0; i < s.Length; ++i )
			{
				if ( IsSafeChar( (int) s[i] ) )
					sb.Append( s[i] );
			}

			return sb.ToString();
		}

		public string ReadUTF8String()
		{
			if ( m_Index >= Size )
				return String.Empty;

			var count = 0;
			var index = m_Index;

			while ( index < Size && Buffer[index++] != 0 )
				++count;

			index = 0;

			var buffer = new byte[count];
			var value = 0;

			while ( m_Index < Size && ( value = Buffer[m_Index++] ) != 0 )
				buffer[index++] = (byte) value;

			return Utility.UTF8.GetString( buffer );
		}

		public string ReadString()
		{
			var sb = new StringBuilder();

			int c;

			while ( m_Index < Size && ( c = Buffer[m_Index++] ) != 0 )
				sb.Append( (char) c );

			return sb.ToString();
		}

		public string ReadStringSafe()
		{
			var sb = new StringBuilder();

			int c;

			while ( m_Index < Size && ( c = Buffer[m_Index++] ) != 0 )
			{
				if ( IsSafeChar( c ) )
					sb.Append( (char) c );
			}

			return sb.ToString();
		}

		public string ReadUnicodeStringSafe( int fixedLength )
		{
			var bound = m_Index + ( fixedLength << 1 );
			var end = bound;

			if ( bound > Size )
				bound = Size;

			var sb = new StringBuilder();

			int c;

			while ( ( m_Index + 1 ) < bound && ( c = ( ( Buffer[m_Index++] << 8 ) | Buffer[m_Index++] ) ) != 0 )
			{
				if ( IsSafeChar( c ) )
					sb.Append( (char) c );
			}

			m_Index = end;

			return sb.ToString();
		}

		public string ReadUnicodeString( int fixedLength )
		{
			var bound = m_Index + ( fixedLength << 1 );
			var end = bound;

			if ( bound > Size )
				bound = Size;

			var sb = new StringBuilder();

			int c;

			while ( ( m_Index + 1 ) < bound && ( c = ( ( Buffer[m_Index++] << 8 ) | Buffer[m_Index++] ) ) != 0 )
				sb.Append( (char) c );

			m_Index = end;

			return sb.ToString();
		}

		public string ReadStringSafe( int fixedLength )
		{
			var bound = m_Index + fixedLength;
			var end = bound;

			if ( bound > Size )
				bound = Size;

			var sb = new StringBuilder();

			int c;

			while ( m_Index < bound && ( c = Buffer[m_Index++] ) != 0 )
			{
				if ( IsSafeChar( c ) )
					sb.Append( (char) c );
			}

			m_Index = end;

			return sb.ToString();
		}

		public string ReadString( int fixedLength )
		{
			var bound = m_Index + fixedLength;
			var end = bound;

			if ( bound > Size )
				bound = Size;

			var sb = new StringBuilder();

			int c;

			while ( m_Index < bound && ( c = Buffer[m_Index++] ) != 0 )
				sb.Append( (char) c );

			m_Index = end;

			return sb.ToString();
		}
	}
}
