using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Loyalty
{
	[PropertyObject]
	public class LoyaltyInfo
	{
		private int[] m_Values;

		public int GetValue( LoyaltyGroup type )
		{
			return GetValue( (int) type );
		}

		public void SetValue( LoyaltyGroup type, int value )
		{
			SetValue( (int) type, value );
		}

		public void Award( LoyaltyGroup type, int value )
		{
			Award( (int) type, value );
		}

		public int GetValue( int index )
		{
			if ( index < 0 || index >= m_Values.Length )
				return 0;

			return m_Values[index];
		}

		public void SetValue( int index, int value )
		{
			if ( index < 0 || index >= m_Values.Length )
				return;

			m_Values[index] = value;
		}

		public void Award( int index, int value )
		{
			if ( index < 0 || index >= m_Values.Length || value <= 0 )
				return;

			m_Values[index] = Math.Min( m_Values[index] + value, LoyaltyGroupInfo.Table[index].MaxPoints );
		}

		public void Atrophy( int index, int value )
		{
			if ( index < 0 || index >= m_Values.Length || value <= 0 )
				return;

			m_Values[index] = Math.Max( m_Values[index] - value, LoyaltyGroupInfo.Table[index].MinPoints );
		}

		public override string ToString()
		{
			return "...";
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int GargoyleQueen { get { return GetValue( LoyaltyGroup.GargoyleQueen ); } set { SetValue( LoyaltyGroup.GargoyleQueen, value ); } }

		public LoyaltyInfo()
		{
			m_Values = new int[LoyaltyGroupInfo.Table.Length];
		}

		public LoyaltyInfo( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 0:
					{
						int length = reader.ReadEncodedInt();
						m_Values = new int[length];

						for ( int i = 0; i < length; i++ )
							m_Values[i] = reader.ReadEncodedInt();

						if ( m_Values.Length != LoyaltyGroupInfo.Table.Length )
						{
							int[] oldValues = m_Values;
							m_Values = new int[LoyaltyGroupInfo.Table.Length];

							for ( int i = 0; i < m_Values.Length && i < oldValues.Length; i++ )
								m_Values[i] = oldValues[i];
						}
						break;
					}
			}
		}

		public static void Serialize( GenericWriter writer, LoyaltyInfo info )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			int length = info.m_Values.Length;
			writer.WriteEncodedInt( length );

			for ( int i = 0; i < length; i++ )
				writer.WriteEncodedInt( info.m_Values[i] );
		}
	}
}