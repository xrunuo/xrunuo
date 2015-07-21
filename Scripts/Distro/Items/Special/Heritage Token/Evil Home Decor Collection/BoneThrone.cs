using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x2A58, 0x2A59 )]
	public class BoneThroneComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1074476; } } // Bone throne

		public BoneThroneComponent( int itemID )
			: base( itemID )
		{
		}

		public BoneThroneComponent( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			bool allow = base.OnMoveOver( m );

			if ( allow && m.Alive && m.IsPlayer && ( m.AccessLevel == AccessLevel.Player || !m.Hidden ) )
				Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x54B, 0x54D ) );

			return allow;
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

	public class BoneThroneAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new BoneThroneDeed(); } }


		[Constructable]
		public BoneThroneAddon()
			: this( true )
		{
		}

		[Constructable]
		public BoneThroneAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new BoneThroneComponent( 0x2A59 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new BoneThroneComponent( 0x2A58 ), 0, 0, 0 );
			}
		}

		public BoneThroneAddon( Serial serial )
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

	public class BoneThroneDeed : BaseAddonDeed
	{
		public override int LabelNumber { get { return 1074476; } } // Bone throne
		public override BaseAddon Addon { get { return new BoneThroneAddon( m_East ); } }

		private bool m_East;


		[Constructable]
		public BoneThroneDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public BoneThroneDeed( Serial serial )
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
			private BoneThroneDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( BoneThroneDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 280, 130, 0xA28 );

				AddItem( 80, 30, 0x2A58 );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 180, 30, 0x2A59 );
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