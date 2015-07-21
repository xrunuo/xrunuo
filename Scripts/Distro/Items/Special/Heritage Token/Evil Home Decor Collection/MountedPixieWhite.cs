using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x2A79, 0x2A7A )]
	public class MountedPixieWhiteComponent : AddonComponent
	{
		public override bool ForceShowProperties { get { return ObjectPropertyListPacket.Enabled; } }

		public MountedPixieWhiteComponent( int itemID )
			: base( itemID )
		{
			Name = "A Mounted White Pixie";
			Weight = 1.0;
		}

		public MountedPixieWhiteComponent( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.InRange( from, 2 ) )
				Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x562, 0x564 ) );
			else
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class MountedPixieWhiteAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new MountedPixieWhiteDeed(); } }


		[Constructable]
		public MountedPixieWhiteAddon()
			: this( true )
		{
		}

		[Constructable]
		public MountedPixieWhiteAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new MountedPixieWhiteComponent( 0x2A79 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new MountedPixieWhiteComponent( 0x2A7A ), 0, 0, 0 );
			}
		}

		public MountedPixieWhiteAddon( Serial serial )
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

			int version = reader.ReadEncodedInt();

		}
	}

	public class MountedPixieWhiteDeed : BaseAddonDeed
	{
		public override int LabelNumber { get { return 1074482; } } // Mounted pixie
		public override BaseAddon Addon { get { return new MountedPixieWhiteAddon( m_East ); } }

		private bool m_East;


		[Constructable]
		public MountedPixieWhiteDeed()
			: base()
		{
			Name = "A Mounted White Pixie Deed";
			LootType = LootType.Blessed;
		}

		public MountedPixieWhiteDeed( Serial serial )
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

			int version = reader.ReadEncodedInt();
		}

		private class InternalGump : Gump
		{
			private MountedPixieWhiteDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( MountedPixieWhiteDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 280, 130, 0xA28 );

				AddItem( 90, 30, 0x2A7A );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 170, 30, 0x2A79 );
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