using System;
using System.Text;
using System.Collections;

namespace Server
{
	public enum ClientType
	{
		Classic,
		Enhanced
	}

	public class ClientVersion : IComparable, IComparer
	{
		public static readonly ClientVersion Zero = new ClientVersion( 0, 0, 0, 0, ClientType.Classic );

		public static readonly ClientVersion Client70130 = new ClientVersion( "7.0.13.0" );
		public static readonly ClientVersion Client70330 = new ClientVersion( "7.0.33.0" );

		private static TimeSpan m_KickDelay = TimeSpan.FromSeconds( 10.0 );

		private static ClientVersion m_Required = new ClientVersion( "7.0.9.0" );

		public static ClientVersion Required
		{
			get { return m_Required; }
			set { m_Required = value; }
		}

		public static TimeSpan KickDelay
		{
			get { return m_KickDelay; }
			set { m_KickDelay = value; }
		}

		private int m_Major, m_Minor, m_Revision, m_Patch;
		private ClientType m_Type;
		private string m_SourceString;

		public int Major { get { return m_Major; } }
		public int Minor { get { return m_Minor; } }
		public int Revision { get { return m_Revision; } }
		public int Patch { get { return m_Patch; } }

		public ClientType Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		public bool IsEnhanced
		{
			get { return m_Type == ClientType.Enhanced; }
		}

		public string SourceString { get { return m_SourceString; } }

		public ClientVersion( int maj, int min, int rev, int pat )
			: this( maj, min, rev, pat, ClientType.Classic )
		{
		}

		public ClientVersion( int maj, int min, int rev, int pat, ClientType type )
		{
			m_Major = maj;
			m_Minor = min;
			m_Revision = rev;
			m_Patch = pat;
			m_Type = type;

			m_SourceString = ToString();
		}

		public static bool operator ==( ClientVersion l, ClientVersion r )
		{
			return ( Compare( l, r ) == 0 );
		}

		public static bool operator !=( ClientVersion l, ClientVersion r )
		{
			return ( Compare( l, r ) != 0 );
		}

		public static bool operator >=( ClientVersion l, ClientVersion r )
		{
			return ( Compare( l, r ) >= 0 );
		}

		public static bool operator >( ClientVersion l, ClientVersion r )
		{
			return ( Compare( l, r ) > 0 );
		}

		public static bool operator <=( ClientVersion l, ClientVersion r )
		{
			return ( Compare( l, r ) <= 0 );
		}

		public static bool operator <( ClientVersion l, ClientVersion r )
		{
			return ( Compare( l, r ) < 0 );
		}

		public override int GetHashCode()
		{
			return m_Major ^ m_Minor ^ m_Revision ^ m_Patch ^ (int) m_Type;
		}

		public override bool Equals( object obj )
		{
			if ( obj == null )
				return false;

			ClientVersion v = obj as ClientVersion;

			if ( v == null )
				return false;

			return m_Major == v.m_Major
				&& m_Minor == v.m_Minor
				&& m_Revision == v.m_Revision
				&& m_Patch == v.m_Patch
				&& m_Type == v.m_Type;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder( 16 );

			builder.Append( m_Major );
			builder.Append( '.' );
			builder.Append( m_Minor );
			builder.Append( '.' );
			builder.Append( m_Revision );
			builder.Append( '.' );
			builder.Append( m_Patch );

			if ( m_Type != ClientType.Classic )
			{
				builder.Append( ' ' );
				builder.Append( m_Type.ToString() );
			}

			return builder.ToString();
		}

		public ClientVersion( string fmt )
		{
			m_SourceString = fmt;

			try
			{
				string[] format = fmt.Split( ' ' );

				string[] version = format[0].Split( '.' );

				m_Major = Utility.ToInt32( version[0] );
				m_Minor = Utility.ToInt32( version[1] );
				m_Revision = Utility.ToInt32( version[2] );
				m_Patch = Utility.ToInt32( version[3] );

				if ( format.Length > 1 )
				{
					string type = format[1].ToLower();

					foreach ( object o in Enum.GetValues( typeof( ClientType ) ) )
					{
						ClientType cType = (ClientType) o;

						if ( type == cType.ToString().ToLower() )
						{
							m_Type = cType;
							break;
						}
					}
				}
			}
			catch
			{
				m_Major = 0;
				m_Minor = 0;
				m_Revision = 0;
				m_Patch = 0;

				m_Type = ClientType.Classic;
			}
		}

		public int CompareTo( object obj )
		{
			if ( obj == null )
				return 1;

			ClientVersion o = obj as ClientVersion;

			if ( o == null )
				throw new ArgumentException();

			if ( m_Major > o.m_Major )
				return 1;
			else if ( m_Major < o.m_Major )
				return -1;
			else if ( m_Minor > o.m_Minor )
				return 1;
			else if ( m_Minor < o.m_Minor )
				return -1;
			else if ( m_Revision > o.m_Revision )
				return 1;
			else if ( m_Revision < o.m_Revision )
				return -1;
			else if ( m_Patch > o.m_Patch )
				return 1;
			else if ( m_Patch < o.m_Patch )
				return -1;
			else
				return 0;
		}

		public static bool IsNull( object x )
		{
			return Object.ReferenceEquals( x, null );
		}

		public int Compare( object x, object y )
		{
			if ( IsNull( x ) && IsNull( y ) )
				return 0;
			else if ( IsNull( x ) )
				return -1;
			else if ( IsNull( y ) )
				return 1;

			ClientVersion a = x as ClientVersion;
			ClientVersion b = y as ClientVersion;

			if ( IsNull( a ) || IsNull( b ) )
				throw new ArgumentException();

			return a.CompareTo( b );
		}

		public static int Compare( ClientVersion a, ClientVersion b )
		{
			if ( IsNull( a ) && IsNull( b ) )
				return 0;
			else if ( IsNull( a ) )
				return -1;
			else if ( IsNull( b ) )
				return 1;

			return a.CompareTo( b );
		}
	}
}