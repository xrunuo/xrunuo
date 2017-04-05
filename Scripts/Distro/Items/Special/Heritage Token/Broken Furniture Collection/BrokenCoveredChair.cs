using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0xC17, 0xC18 )]
	public class BrokenCoveredChairComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1076257; } } // Broken Covered Chair

		public BrokenCoveredChairComponent( int itemID )
			: base( itemID )
		{
		}

		public BrokenCoveredChairComponent( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class BrokenCoveredChairAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new BrokenCoveredChairDeed(); } }

		[Constructable]
		public BrokenCoveredChairAddon()
			: this( true )
		{
		}

		[Constructable]
		public BrokenCoveredChairAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new BrokenCoveredChairComponent( 0xC17 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new BrokenCoveredChairComponent( 0xC18 ), 0, 0, 0 );
			}
		}

		public BrokenCoveredChairAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class BrokenCoveredChairDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new BrokenCoveredChairAddon( m_East ); } }
		public override int LabelNumber { get { return 1076257; } } // Broken Covered Chair

		private bool m_East;

		[Constructable]
		public BrokenCoveredChairDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public BrokenCoveredChairDeed( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{

			if ( IsChildOf( from.Backpack ) )
			{
				from.CloseGump<InternalGump>();
				from.SendGump( new InternalGump( this ) );
			}
			else
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.    
		}

		private void SendTarget( Mobile m )
		{
			base.OnDoubleClick( m );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}

		private class InternalGump : Gump
		{
			private BrokenCoveredChairDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( BrokenCoveredChairDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 280, 130, 0xA28 );

				AddItem( 80, 30, 0xC18 );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 180, 30, 0xC17 );
				AddButton( 145, 35, 0x867, 0x869, (int) Buttons.East, GumpButtonType.Reply, 0 ); // East
			}
			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( m_Deed == null || m_Deed.Deleted )
					return;

				if ( info.ButtonID != (int) Buttons.Cancel )
				{
					m_Deed.m_East = ( info.ButtonID == (int) Buttons.East );
					m_Deed.SendTarget( sender.Mobile );
				}
			}
		}
	}
}