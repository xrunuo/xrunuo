using System;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class KeyFragmentSpawner : Item
	{
		public abstract Type KeyType { get; }

		public KeyFragmentSpawner( int hue )
			: base( 0x2002 )
		{
			Hue = hue;
			LootType = LootType.Blessed;

			Movable = false;
			Visible = false;
		}

		public KeyFragmentSpawner( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( Location, 2 ) )
			{
				// I can't reach that.
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 );
			}
			else if ( from is PlayerMobile && ( (PlayerMobile) from ).SacredQuest )
			{
				// You cannot think of any reason to want to do this.
				from.SendLocalizedMessage( 1080538 );
			}
			else if ( from.Backpack != null )
			{
				from.DropHolding();

				Item key = from.Backpack.FindItemByType( KeyType );

				if ( key != null )
				{
					// You are already carrying a copy of this key fragment.
					from.SendLocalizedMessage( 1111653 );
				}
				else
				{
					from.PlaceInBackpack( (Item) Activator.CreateInstance( KeyType ) );

					// You reach for the key and receive a glowing copy that you place in your bag.
					from.SendLocalizedMessage( 1111652 );
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