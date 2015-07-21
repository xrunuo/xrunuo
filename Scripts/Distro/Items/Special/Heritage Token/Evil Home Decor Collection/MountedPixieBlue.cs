using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x2A75, 0x2A76 )]
	public class MountedPixieBlueComponent : AddonComponent
	{
		public override bool ForceShowProperties { get { return ObjectPropertyListPacket.Enabled; } }

		public MountedPixieBlueComponent( int itemID )
			: base( itemID )
		{
			Name = "A Mounted Blue Pixie";
			Weight = 1.0;
		}

		public MountedPixieBlueComponent( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.InRange( from, 2 ) )
				Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x55C, 0x55E ) );
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

	public class MountedPixieBlueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new MountedPixieBlueDeed(); } }


		[Constructable]
		public MountedPixieBlueAddon()
			: this( true )
		{
		}

		[Constructable]
		public MountedPixieBlueAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new MountedPixieBlueComponent( 0x2A75 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new MountedPixieBlueComponent( 0x2A76 ), 0, 0, 0 );
			}
		}

		public MountedPixieBlueAddon( Serial serial )
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

	public class MountedPixieBlueDeed : BaseAddonDeed
	{
		public override int LabelNumber { get { return 1074482; } } // Mounted pixie
		public override BaseAddon Addon { get { return new MountedPixieBlueAddon( m_East ); } }

		private bool m_East;


		[Constructable]
		public MountedPixieBlueDeed()
			: base()
		{
			Name = "A Mounted Blue Pixie Deed";
			LootType = LootType.Blessed;
		}

		public MountedPixieBlueDeed( Serial serial )
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
			private MountedPixieBlueDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( MountedPixieBlueDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 280, 130, 0xA28 );

				AddItem( 90, 30, 0x2A76 );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 170, 30, 0x2A75 );
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