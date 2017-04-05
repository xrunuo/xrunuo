using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class BoneCouchComponent : AddonComponent
	{
		public BoneCouchComponent( int itemID )
			: base( itemID )
		{
		}

		public BoneCouchComponent( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			bool allow = base.OnMoveOver( m );


			if ( allow && m.Alive && m.Player && ( m.AccessLevel == AccessLevel.Player || !m.Hidden ) )
				Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x547, 0x54A ) );

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

	public class BoneCouchAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new BoneCouchDeed(); } }


		[Constructable]
		public BoneCouchAddon()
			: this( true )
		{
		}

		[Constructable]
		public BoneCouchAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new BoneCouchComponent( 0x2A80 ), 0, 0, 0 );
				AddComponent( new BoneCouchComponent( 0x2A7F ), 0, 1, 0 );
			}
			else
			{
				AddComponent( new BoneCouchComponent( 0x2A5A ), 0, 0, 0 );
				AddComponent( new BoneCouchComponent( 0x2A5B ), -1, 0, 0 );
			}
		}

		public BoneCouchAddon( Serial serial )
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

	public class BoneCouchDeed : BaseAddonDeed
	{
		public override int LabelNumber { get { return 1074477; } } // Bone couch
		public override BaseAddon Addon { get { return new BoneCouchAddon( m_East ); } }

		private bool m_East;


		[Constructable]
		public BoneCouchDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public BoneCouchDeed( Serial serial )
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
			private BoneCouchDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( BoneCouchDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 300, 140, 0xA28 );

				AddItem( 94, 25, 0x2A5B );
				AddItem( 112, 48, 0x2A5A );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 210, 38, 0x2A7F );
				AddItem( 228, 25, 0x2A80 );
				AddButton( 170, 35, 0x867, 0x869, (int) Buttons.East, GumpButtonType.Reply, 0 ); // East
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