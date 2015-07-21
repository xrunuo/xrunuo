using System;
using System.Linq;

using Server;
using Server.Factions;

namespace Server.Items
{
	public class MorphEarrings : GoldEarrings
	{
		public override int LabelNumber { get { return 1094746; } } // Morph Earrings

		[Constructable]
		public MorphEarrings()
		{
			Hue = 250;
		}

		public MorphEarrings( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1041350 ); // faction item
		}

		public override bool CanEquip( Mobile m )
		{
			if ( Faction.Find( m ) == null )
			{
				m.SendLocalizedMessage( 1010371 ); // You cannot equip a faction item!
				return false;
			}

			return base.CanEquip( m );
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
				( (Mobile) parent ).SendLocalizedMessage( 1094747, "", 0x35 ); // You may now equip Elven items.
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
			{
				Mobile m = (Mobile) parent;

				m.SendLocalizedMessage( 1094748, "", 0x23 ); // You may no longer equip Elven items.

				if ( m.Race != Race.Elf )
				{
					var dropped = m.GetEquippedItems().Where( ( item ) => item.RequiredRace == Race.Elf );

					foreach ( var item in dropped )
					{
						m.AddToBackpack( item );
					}

					if ( dropped.Any() )
						m.SendLocalizedMessage( 1094749 ); // The Elven items you were wearing have been unequipped.
				}
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