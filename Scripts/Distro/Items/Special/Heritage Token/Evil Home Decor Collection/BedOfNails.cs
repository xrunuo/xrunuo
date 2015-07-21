using System;
using Server;
using Server.Multis;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class BedOfNailsComponent : AddonComponent
	{
		public override bool ForceShowProperties { get { return ObjectPropertyListPacket.Enabled; } }

		public BedOfNailsComponent( int itemID )
			: base( itemID )
		{
		}

		public BedOfNailsComponent( Serial serial )
			: base( serial )
		{
		}
		public override bool OnMoveOver( Mobile m )
		{
			bool allow = base.OnMoveOver( m );

			if ( allow && Addon is BedOfNailsAddon )
				( (BedOfNailsAddon) Addon ).OnMoveOver( m );

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


	public class BedOfNailsAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new BedOfNailsDeed(); } }

		private InternalTimer m_Timer;

		[Constructable]
		public BedOfNailsAddon()
			: this( true )
		{
		}

		[Constructable]
		public BedOfNailsAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new BedOfNailsComponent( 0x2A89 ), 0, 0, 0 );
				AddComponent( new BedOfNailsComponent( 0x2A8A ), -1, 0, 0 );
			}
			else
			{
				AddComponent( new BedOfNailsComponent( 0x2A81 ), 0, 0, 0 );
				AddComponent( new BedOfNailsComponent( 0x2A82 ), 0, -1, 0 );
			}
		}

		public BedOfNailsAddon( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m.Alive && ( m.AccessLevel == AccessLevel.Player || !m.Hidden ) )
			{
				if ( m.IsPlayer )
				{
					if ( m.Female )
						Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x53B, 0x53D ) );
					else
						Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x53E, 0x540 ) );
				}

				if ( m_Timer == null || !m_Timer.Running )
					( m_Timer = new InternalTimer( m ) ).Start();
			}

			return true;
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

	public class InternalTimer : Timer
	{
		private Mobile m_Mobile;
		private Point3D m_Location;

		public InternalTimer( Mobile m )
			: base( TimeSpan.Zero, TimeSpan.FromSeconds( 1 ), 5 )
		{
			m_Mobile = m;
			m_Location = Point3D.Zero;
		}

		protected override void OnTick()
		{
			if ( m_Location != m_Mobile.Location )
			{
				int amount = Utility.RandomMinMax( 0, 7 );

				for ( int i = 0; i < amount; i++ )
				{
					int x = m_Mobile.X + Utility.RandomMinMax( -1, 1 );
					int y = m_Mobile.Y + Utility.RandomMinMax( -1, 1 );
					int z = m_Mobile.Z;

					if ( !m_Mobile.Map.CanFit( x, y, z, 1, false, false, true ) )
					{
						z = m_Mobile.Map.GetAverageZ( x, y );

						if ( !m_Mobile.Map.CanFit( x, y, z, 1, false, false, true ) )
							continue;
					}

					Blood blood = new Blood( Utility.RandomMinMax( 0x122C, 0x122F ) );
					blood.MoveToWorld( new Point3D( x, y, z ), m_Mobile.Map );
				}

				m_Location = m_Mobile.Location;
			}
		}
	}


	public class BedOfNailsDeed : BaseAddonDeed
	{
		public override int LabelNumber { get { return 1074801; } } // Bed of Nails
		public override BaseAddon Addon { get { return new BedOfNailsAddon( m_East ); } }

		private bool m_East;


		[Constructable]
		public BedOfNailsDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public BedOfNailsDeed( Serial serial )
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
			private BedOfNailsDeed m_Deed;

			private enum Buttons
			{
				Cancel,
				South,
				East
			}

			public InternalGump( BedOfNailsDeed deed )
				: base( 150, 50 )
			{
				m_Deed = deed;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage( 0 );

				AddBackground( 0, 0, 360, 150, 0xA28 );

				AddItem( 94, 63, 0x2A81 );
				AddItem( 116, 25, 0x2A82 );
				AddButton( 50, 45, 0x867, 0x869, (int) Buttons.South, GumpButtonType.Reply, 0 ); // South

				AddItem( 230, 25, 0x2A8A );
				AddItem( 249, 63, 0x2A89 );
				AddButton( 190, 45, 0x867, 0x869, (int) Buttons.East, GumpButtonType.Reply, 0 ); // East
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