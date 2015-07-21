using System;
using Server;

namespace Server.Items
{
	public class RandomTalisman : BaseTalisman
	{
		private static int[] m_IDs = new int[]
		{
			0x2F5B,
			0x2F5A,
			0x2F59,
			0x2F58
		};

		[Constructable]
		public RandomTalisman()
			: this( 0 )
		{
		}

		[Constructable]
		public RandomTalisman( int luck )
			: base( m_IDs[Utility.Random( 4 )] )
		{
			Weight = 1.0;

			int n_props = Utility.Random( 4 );

			if ( luck > Utility.Random( 10000 ) && n_props < 4 )
				n_props++;

			for ( int i = 0; i <= n_props; i++ )
			{
				int prop = 1 + Utility.Random( 6 );

				switch ( prop )
				{
					case 1: // Killer
						{
							if ( KillersValue == 0 )
							{
								KillersTalis = ProtectionKillerEntry.GetRandom();
								KillersValue = 1 + Utility.Random( 100 );

								if ( luck > Utility.Random( 10000 ) )
									KillersValue += 1 + Utility.Random( 10 );

								if ( KillersValue > 100 )
									KillersValue = 100;

								break;
							}
							else
							{
								goto case 2;
							}
						}
					case 2: // Protection
						{
							if ( ProtectionValue == 0 )
							{
								ProtectionTalis = ProtectionKillerEntry.GetRandom();
								ProtectionValue = 1 + Utility.Random( 60 );

								if ( luck > Utility.Random( 10000 ) )
									ProtectionValue += 1 + Utility.Random( 10 );

								if ( ProtectionValue > 60 )
									ProtectionValue = 60;

								break;
							}
							else
							{
								goto case 3;
							}
						}
					case 3: // Slayer
						{
							if ( TalisSlayer == TalisSlayerName.None )
							{
								TalisSlayer = TalisSlayerEntry.GetRandom();
								break;
							}
							else
							{
								goto case 4;
							}
						}
					case 4: // Craft Bonus Regular
						{
							if ( CraftBonusRegularValue == 0 )
							{
								CraftBonusRegular = (CraftList) ( 1 + Utility.Random( 9 ) );
								CraftBonusRegularValue = 1 + Utility.Random( 30 );

								if ( luck > Utility.Random( 10000 ) )
									CraftBonusRegularValue += 1 + Utility.Random( 5 );

								if ( CraftBonusRegularValue > 30 )
									CraftBonusRegularValue = 30;

								break;
							}
							else
							{
								goto case 5;
							}
						}
					case 5: // Craft Bonus Excep
						{
							if ( CraftBonusExcepValue == 0 )
							{
								if ( CraftBonusRegularValue == 0 )
								{
									goto case 4;
								}
								else
								{
									CraftBonusExcep = CraftBonusRegular;
									CraftBonusExcepValue = 1 + Utility.Random( 30 );

									if ( luck > Utility.Random( 10000 ) )
										CraftBonusExcepValue += 1 + Utility.Random( 5 );

									if ( CraftBonusExcepValue > 30 )
										CraftBonusExcepValue = 30;

									break;
								}
							}
							else
							{
								goto case 6;
							}
						}
					case 6: // Removal Summon
						{
							TalismanType = (TalismanType) ( 1 + Utility.Random( 25 ) );
							break;
						}
					default: // Removal Summon
						{
							TalismanType = (TalismanType) ( 1 + Utility.Random( 25 ) );
							break;
						}
				}
			}
		}

		public RandomTalisman( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}