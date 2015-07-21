using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0xC14, 0xC15 )]
	public class BrokenBookcaseComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1076258; } } // Broken Bookcase

		public BrokenBookcaseComponent( int itemID )
			: base( itemID )
		{
		}

		public BrokenBookcaseComponent( Serial serial )
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

	public class BrokenBookcaseAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new BrokenBookcaseDeed(); } }

		[Constructable]
		public BrokenBookcaseAddon()
			: this( true )
		{
		}
		[Constructable]
		public BrokenBookcaseAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new BrokenBookcaseComponent( 0xC14 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new BrokenBookcaseComponent( 0xC15 ), 0, 0, 0 );
			}
		}

		public BrokenBookcaseAddon( Serial serial )
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

	public class BrokenBookcaseDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new BrokenBookcaseAddon( m_East ); } }
		public override int LabelNumber { get { return 1076258; } } // Broken Bookcase

		private bool m_East;

		[Constructable]
		public BrokenBookcaseDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public BrokenBookcaseDeed( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{

			if ( IsChildOf( from.Backpack ) )
			{
				from.CloseGump( typeof( InternalGump ) );
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
			private BrokenBookcaseDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( BrokenBookcaseDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 280, 130, 0xA28 );

				AddItem( 80, 30, 0xC15 );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 180, 30, 0xC14 );
				AddButton( 145, 35, 0x867, 0x869, (int) Buttons.East, GumpButtonType.Reply, 0 ); // East
			}
			public override void OnResponse( GameClient sender, RelayInfo info )
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