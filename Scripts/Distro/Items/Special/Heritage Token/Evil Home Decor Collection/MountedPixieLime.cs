using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x2A77, 0x2A78 )]
	public class MountedPixieLimeComponent : AddonComponent
	{
		public override bool ForceShowProperties { get { return ObjectPropertyListPacket.Enabled; } }

		public MountedPixieLimeComponent( int itemID )
			: base( itemID )
		{
			Name = "A Mounted Lime Pixie";
			Weight = 1.0;
		}

		public MountedPixieLimeComponent( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.InRange( from, 2 ) )
				Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x55F, 0x561 ) );
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

	public class MountedPixieLimeAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new MountedPixieLimeDeed(); } }


		[Constructable]
		public MountedPixieLimeAddon()
			: this( true )
		{
		}

		[Constructable]
		public MountedPixieLimeAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new MountedPixieLimeComponent( 0x2A77 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new MountedPixieLimeComponent( 0x2A78 ), 0, 0, 0 );
			}
		}

		public MountedPixieLimeAddon( Serial serial )
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

	public class MountedPixieLimeDeed : BaseAddonDeed
	{
		public override int LabelNumber { get { return 1074482; } } // Mounted pixie
		public override BaseAddon Addon { get { return new MountedPixieLimeAddon( m_East ); } }

		private bool m_East;


		[Constructable]
		public MountedPixieLimeDeed()
			: base()
		{
			Name = "A Mounted Lime Pixie Deed";
			LootType = LootType.Blessed;
		}

		public MountedPixieLimeDeed( Serial serial )
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
			private MountedPixieLimeDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( MountedPixieLimeDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 280, 130, 0xA28 );

				AddItem( 90, 30, 0x2A78 );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 170, 30, 0x2A77 );
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