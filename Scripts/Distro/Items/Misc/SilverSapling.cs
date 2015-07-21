using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class SilverSapling : Item
	{
		public override int LabelNumber { get { return 1113052; } } // The Silver Sapling

		public SilverSapling()
			: base( 0xCE3 )
		{
			Movable = false;
			Hue = 0x47E;
		}

		public SilverSapling( Serial serial )
			: base( serial )
		{
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( !m.Alive && m.IsPlayer && this.InRange( m, 1 ) && !this.InRange( oldLocation, 1 ) )
				OfferResurrection( m );
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			Ankhs.GetContextMenuEntries( from, this, list );
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm != null && pm.Backpack != null )
			{
				if ( pm.NextSilverSaplingUse > DateTime.Now )
				{
					// You must wait a full day before receiving another Seed of the Silver Sapling
					pm.SendLocalizedMessage( 1113042 );
				}
				else
				{
					pm.PlaceInBackpack( new SilverSaplingSeed() );

					// The Silver Sapling pulses with light, and a shining seed appears in your hands.
					pm.SendLocalizedMessage( 1113043 );

					pm.NextSilverSaplingUse = DateTime.Now + TimeSpan.FromDays( 1.0 );
				}
			}
		}

		public static void OfferResurrection( Mobile m )
		{
			m.SendGump( new ResurrectGump( m, ResurrectMessage.Generic ) );
		}

		public override void OnDoubleClickDead( Mobile m )
		{
			OfferResurrection( m );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}
}