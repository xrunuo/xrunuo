using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.Plants;

namespace Server.Items
{
	public class ScouringToxin : Item, ICommodity
	{
		public override int LabelNumber { get { return 1112292; } } // scouring toxin

		[Constructable]
		public ScouringToxin()
			: this( 1 )
		{
		}

		[Constructable]
		public ScouringToxin( int amount )
			: base( 0x1848 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public ScouringToxin( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				// This must be in your backpack to use it.
				from.SendLocalizedMessage( 1080063 );
			}
			else
			{
				// Which item do you wish to scour?
				from.SendLocalizedMessage( 1112348 );

				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( Target ) );
			}
		}

		protected void Target( Mobile from, object targeted )
		{
			if ( from.Backpack == null )
				return;

			if ( !IsChildOf( from.Backpack ) )
			{
				// This must be in your backpack to use it.
				from.SendLocalizedMessage( 1080063 );
			}
			else if ( targeted is Item )
			{
				Item item = targeted as Item;

				if ( item.Parent is Mobile )
				{
					// You cannot scour items that are being worn!
					from.SendLocalizedMessage( 1112350 );
				}
				else if ( !item.IsChildOf( from.Backpack ) )
				{
					// That must be in your pack for you to use it.
					from.SendLocalizedMessage( 1042001 );
				}
				else if ( !item.Movable )
				{
					// You may not scour items which are locked down.
					from.SendLocalizedMessage( 1112351 );
				}
				else if ( item is DryReeds )
				{
					if ( !( from is PlayerMobile ) || !( (PlayerMobile) from ).BasketWeaving )
					{
						// You haven't learned basket weaving. Perhaps studying a book would help!
						from.SendLocalizedMessage( 1112253 );
					}
					else
					{
						PlantHue hue = ( (DryReeds) item ).PlantHue;

						if ( DryReeds.ConsumeReeds( from.Backpack, hue, 2 ) )
						{
							Consume();
							from.AddToBackpack( new SoftenedReeds( hue ) );
						}
						else
						{
							// You don't have enough of this type of dry reeds to make that.
							from.SendLocalizedMessage( 1112250 );
						}
					}
				}
				else if ( NaturalDye.IsValidItem( item ) )
				{
					Consume();

					item.Hue = 0;
					from.PlaySound( 0x23E );
				}
				else
				{
					// You cannot scour that!
					from.SendLocalizedMessage( 1112349 );
				}
			}
			else
			{
				// You cannot scour that!
				from.SendLocalizedMessage( 1112349 );
			}
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