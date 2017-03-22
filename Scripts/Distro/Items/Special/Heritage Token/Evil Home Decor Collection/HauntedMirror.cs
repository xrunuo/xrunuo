using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x2A7B, 0x2A7D )]
	public class HauntedMirrorComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1074800; } } // Haunted Mirror
		public override bool HandlesOnMovement { get { return true; } }

		public HauntedMirrorComponent( int itemID )
			: base( itemID )
		{
		}

		public HauntedMirrorComponent( Serial serial )
			: base( serial )
		{
		}

		public override void OnMovement( Mobile m, Point3D old )
		{
			base.OnMovement( m, old );

			if ( m.Alive && m.Player && ( m.AccessLevel == AccessLevel.Player || !m.Hidden ) )
			{
				if ( !this.InRange( old, 2 ) && this.InRange( m, 2 ) )
				{
					if ( ItemID == 0x2A7B || ItemID == 0x2A7D )
					{
						Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x551, 0x553 ) );
						ItemID += 1;
					}
				}
				else if ( this.InRange( old, 2 ) && !this.InRange( m, 2 ) )
				{
					if ( ItemID == 0x2A7C || ItemID == 0x2A7E )
						ItemID -= 1;
				}
			}
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

	public class HauntedMirrorAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new HauntedMirrorDeed(); } }


		[Constructable]
		public HauntedMirrorAddon()
			: this( true )
		{
		}

		[Constructable]
		public HauntedMirrorAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new HauntedMirrorComponent( 0x2A7D ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new HauntedMirrorComponent( 0x2A7B ), 0, 0, 0 );
			}
		}

		public HauntedMirrorAddon( Serial serial )
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

	public class HauntedMirrorDeed : BaseAddonDeed
	{
		public override int LabelNumber { get { return 1074800; } } // Haunted Mirror
		public override BaseAddon Addon { get { return new HauntedMirrorAddon( m_East ); } }

		private bool m_East;


		[Constructable]
		public HauntedMirrorDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public HauntedMirrorDeed( Serial serial )
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
			private HauntedMirrorDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( HauntedMirrorDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 280, 130, 0xA28 );

				AddItem( 90, 30, 0x2A7B );
				AddButton( 50, 35, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 170, 30, 0x2A7D );
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