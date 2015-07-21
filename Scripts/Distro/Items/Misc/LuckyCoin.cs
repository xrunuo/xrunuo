using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class LuckyCoin : Item
	{
		public override int LabelNumber { get { return 1113366; } } // lucky coin

		[Constructable]
		public LuckyCoin()
			: this( 1 )
		{
		}

		[Constructable]
		public LuckyCoin( int amount )
			: base( 0xF87 )
		{
			Weight = 1.0;
			Amount = amount;
			Stackable = true;
			Hue = 0x496;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				// You must have the object in your backpack to use it.
				from.SendLocalizedMessage( 1042010 );
			}
			else
			{
				from.Target = new InternalTarget( this );
				from.SendLocalizedMessage( 1113367 ); // Make a wish then toss me into sacred waters!!
			}
		}

		public LuckyCoin( Serial serial )
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

		private class InternalTarget : Target
		{
			private LuckyCoin m_Coin;

			public InternalTarget( LuckyCoin coin )
				: base( 3, false, TargetFlags.None )
			{
				m_Coin = coin;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Coin.Deleted )
					return;

				if ( !m_Coin.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1042664 ); // You must have the object in your backpack to use it.
					return;
				}

				if ( targeted is FontOfFortuneAddonComponent )
				{
					PlayerMobile pm = from as PlayerMobile;

					if ( pm == null )
						return;

					if ( pm.NextLuckyCoinWish > DateTime.Now )
					{
						// You already made a wish today. Try again tomorrow!
						from.SendLocalizedMessage( 1113368 );
					}
					else
					{
						if ( 0.2 > Utility.RandomDouble() )
						{
							// 20% Reward
							FontOfFortune.Award( from );
						}
						else
						{
							// 80% Blessing
							FontOfFortune.Bless( from );
						}

						pm.NextLuckyCoinWish = DateTime.Now + TimeSpan.FromHours( 12.0 );

						m_Coin.Consume();
					}
				}
				else
				{
					// That is not sacred waters. Try looking in the Underworld.
					from.SendLocalizedMessage( 1113369 );
				}
			}
		}
	}
}