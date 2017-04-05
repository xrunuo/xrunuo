using System;

namespace Server
{
	public struct Serial : IComparable
	{
		public static readonly Serial MinusOne = new Serial( -1 );
		public static readonly Serial Zero = new Serial( 0 );

		public Serial( int serial )
		{
			Value = serial;
		}

		public int Value { get; }

		public bool IsMobile => ( Value > 0 && Value < 0x40000000 );

		public bool IsItem => ( Value >= 0x40000000 && Value <= 0x7FFFFFFF );

		public bool IsValid => ( Value > 0 );

		public override int GetHashCode()
		{
			return Value;
		}

		public int CompareTo( object o )
		{
			if ( o == null )
				return 1;
			else if ( !( o is Serial ) )
				throw new ArgumentException();

			int ser = ( (Serial) o ).Value;

			if ( Value > ser )
				return 1;
			else if ( Value < ser )
				return -1;
			else
				return 0;
		}

		public override bool Equals( object o )
		{
			if ( o == null || !( o is Serial ) )
				return false;

			return ( (Serial) o ).Value == Value;
		}

		public static bool operator ==( Serial l, Serial r )
		{
			return l.Value == r.Value;
		}

		public static bool operator !=( Serial l, Serial r )
		{
			return l.Value != r.Value;
		}

		public static bool operator >( Serial l, Serial r )
		{
			return l.Value > r.Value;
		}

		public static bool operator <( Serial l, Serial r )
		{
			return l.Value < r.Value;
		}

		public static bool operator >=( Serial l, Serial r )
		{
			return l.Value >= r.Value;
		}

		public static bool operator <=( Serial l, Serial r )
		{
			return l.Value <= r.Value;
		}

		public override string ToString()
		{
			return String.Format( "0x{0:X8}", Value );
		}

		public static implicit operator int( Serial a )
		{
			return a.Value;
		}

		public static implicit operator Serial( int a )
		{
			return new Serial( a );
		}
	}
}
