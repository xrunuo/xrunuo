using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class EmptyVenomVial : Item
	{
		public override int LabelNumber { get { return 1112215; } } // empty venom vial

		[Constructable]
		public EmptyVenomVial()
			: this( 1 )
		{
		}

		[Constructable]
		public EmptyVenomVial( int amount )
			: base( 0xE24 )
		{
			Weight = 1.0;
			Amount = amount;
			Stackable = true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 1112222 ); // Which creature do you wish to extract resources from?
			from.Target = new InternalTarget( this );
		}

		private class InternalTarget : Target
		{
			private EmptyVenomVial m_Vial;

			public InternalTarget( EmptyVenomVial vial )
				: base( 12, false, TargetFlags.None )
			{
				m_Vial = vial;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				SilverSerpent serpent = targeted as SilverSerpent;

				if ( serpent == null )
				{
					// You may only use this on a silver serpent.
					from.SendLocalizedMessage( 1112221 );
				}
				else if ( !from.InRange( serpent, 1 ) )
				{
					// That is too far away.
					from.SendLocalizedMessage( 500446 );
				}
				else if ( serpent.CharmMaster == null )
				{
					// You seem to anger the beast!
					serpent.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 502805, from.NetState );
				}
				else if ( serpent.Venom == SilverSerpent.VenomLeft.None )
				{
					// This serpent has already been drained of all its venom.
					from.SendLocalizedMessage( 1112223 );
				}
				else
				{
					if ( Utility.RandomBool() )
					{
						from.AddToBackpack( new SilverSerpentVenom() );

						if ( serpent.Venom == SilverSerpent.VenomLeft.All )
							from.SendLocalizedMessage( 1112220 ); // You manage to extract some resources from the creature.
						else
							from.SendLocalizedMessage( 1112219 ); // You skillfully extract additional resources from the creature.

						if ( serpent.Venom == SilverSerpent.VenomLeft.Half || Utility.RandomBool() )
							serpent.Venom = SilverSerpent.VenomLeft.None;
						else
							serpent.Venom = SilverSerpent.VenomLeft.Half;

						m_Vial.Consume();
					}
					else
					{
						// You handle the creature but fail to harvest any resources from it.
						from.SendLocalizedMessage( 1112218 );
					}

					if ( Utility.RandomBool() )
						from.ApplyPoison( serpent, serpent.Poison );
				}
			}
		}

		public EmptyVenomVial( Serial serial )
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
