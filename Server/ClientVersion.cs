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

		public static ClientVersion Required { get; set; } = new ClientVersion( "7.0.9.0" );

		public static TimeSpan KickDelay { get; set; } = TimeSpan.FromSeconds( 10.0 );

		public int Major { get; }

		public int Minor { get; }

		public int Revision { get; }

		public int Patch { get; }

		public ClientType Type { get; set; }

		public bool IsEnhanced => Type == ClientType.Enhanced;

		public string SourceString { get; }

		public ClientVersion( int maj, int min, int rev, int pat )
			: this( maj, min, rev, pat, ClientType.Classic )
		{
		}

		public ClientVersion( int maj, int min, int rev, int pat, ClientType type )
		{
			Major = maj;
			Minor = min;
			Revision = rev;
			Patch = pat;
			Type = type;

			SourceString = ToString();
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
			return Major ^ Minor ^ Revision ^ Patch ^ (int) Type;
		}

		public override bool Equals( object obj )
		{
			if ( obj == null )
				return false;

			ClientVersion v = obj as ClientVersion;

			if ( v == null )
				return false;

			return Major == v.Major
				&& Minor == v.Minor
				&& Revision == v.Revision
				&& Patch == v.Patch
				&& Type == v.Type;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder( 16 );

			builder.Append( Major );
			builder.Append( '.' );
			builder.Append( Minor );
			builder.Append( '.' );
			builder.Append( Revision );
			builder.Append( '.' );
			builder.Append( Patch );

			if ( Type != ClientType.Classic )
			{
				builder.Append( ' ' );
				builder.Append( Type.ToString() );
			}

			return builder.ToString();
		}

		public ClientVersion( string fmt )
		{
			SourceString = fmt;

			try
			{
				string[] format = fmt.Split( ' ' );

				string[] version = format[0].Split( '.' );

				Major = Utility.ToInt32( version[0] );
				Minor = Utility.ToInt32( version[1] );
				Revision = Utility.ToInt32( version[2] );
				Patch = Utility.ToInt32( version[3] );

				if ( format.Length > 1 )
				{
					string type = format[1].ToLower();

					foreach ( object o in Enum.GetValues( typeof( ClientType ) ) )
					{
						ClientType cType = (ClientType) o;

						if ( type == cType.ToString().ToLower() )
						{
							Type = cType;
							break;
						}
					}
				}
			}
			catch
			{
				Major = 0;
				Minor = 0;
				Revision = 0;
				Patch = 0;

				Type = ClientType.Classic;
			}
		}

		public int CompareTo( object obj )
		{
			if ( obj == null )
				return 1;

			ClientVersion o = obj as ClientVersion;

			if ( o == null )
				throw new ArgumentException();

			if ( Major > o.Major )
				return 1;
			else if ( Major < o.Major )
				return -1;
			else if ( Minor > o.Minor )
				return 1;
			else if ( Minor < o.Minor )
				return -1;
			else if ( Revision > o.Revision )
				return 1;
			else if ( Revision < o.Revision )
				return -1;
			else if ( Patch > o.Patch )
				return 1;
			else if ( Patch < o.Patch )
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