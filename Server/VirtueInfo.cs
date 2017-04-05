using System;

namespace Server
{
	[PropertyObject]
	public class VirtueInfo
	{
		public int[] Values { get; private set; }

		public int GetValue( int index )
		{
			if ( Values == null )
				return 0;
			else
				return Values[index];
		}

		public void SetValue( int index, int value )
		{
			if ( Values == null )
				Values = new int[8];

			Values[index] = value;
		}

		public override string ToString()
		{
			return "...";
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Humility
		{
			get { return GetValue( 0 ); }
			set { SetValue( 0, value ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Sacrifice
		{
			get { return GetValue( 1 ); }
			set { SetValue( 1, value ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Compassion
		{
			get { return GetValue( 2 ); }
			set { SetValue( 2, value ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Spirituality
		{
			get { return GetValue( 3 ); }
			set { SetValue( 3, value ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Valor
		{
			get { return GetValue( 4 ); }
			set { SetValue( 4, value ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Honor
		{
			get { return GetValue( 5 ); }
			set { SetValue( 5, value ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Justice
		{
			get { return GetValue( 6 ); }
			set { SetValue( 6, value ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Honesty
		{
			get { return GetValue( 7 ); }
			set { SetValue( 7, value ); }
		}

		public VirtueInfo()
		{
		}

		public VirtueInfo( GenericReader reader )
		{
			int version = reader.ReadByte();

			switch ( version )
			{
				case 1:	// Changed the values throughout the virtue system
				case 0:
					{
						int mask = reader.ReadByte();

						if ( mask != 0 )
						{
							Values = new int[8];

							for ( int i = 0; i < 8; ++i )
								if ( ( mask & ( 1 << i ) ) != 0 )
									Values[i] = reader.ReadInt();
						}

						break;
					}
			}

			if ( version == 0 )
			{
				Compassion *= 200;
				Sacrifice *= 250;
				Justice *= 500;
				Honor *= 500;
				Valor *= 400;
			}
		}

		public static void Serialize( GenericWriter writer, VirtueInfo info )
		{
			writer.Write( (byte) 1 ); // version

			if ( info.Values == null )
			{
				writer.Write( (byte) 0 );
			}
			else
			{
				int mask = 0;

				for ( int i = 0; i < 8; ++i )
					if ( info.Values[i] != 0 )
						mask |= 1 << i;

				writer.Write( (byte) mask );

				for ( int i = 0; i < 8; ++i )
					if ( info.Values[i] != 0 )
						writer.Write( (int) info.Values[i] );
			}
		}
	}
}