using System;
using System.Collections;
using System.Collections.Generic;
using Server.Gumps;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
	public class SacrificialAltarAddon : BaseAddonContainer
	{
		public override BaseAddonContainerDeed Deed { get { return new SacrificialAltarDeed(); } }

		public override int LabelNumber { get { return 1074818; } } // Sacrificial Altar

		public override int DefaultMaxWeight { get { return 0; } } // A value of 0 signals unlimited weight

		public override int DefaultGumpID { get { return 0x107; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override bool IsDecoContainer { get { return false; } }

		[Constructable]
		public SacrificialAltarAddon( bool east )
			: base( east ? 0x2A9C : 0x2A9B )
		{
			if ( east ) // east
			{
				AddComponent( new AddonContainerComponent( 0x2A9D ), 0, -1, 0 );

			}
			else		// south
			{
				AddComponent( new AddonContainerComponent( 0x2A9A ), 1, 0, 0 );
			}
		}

		public SacrificialAltarAddon( Serial serial )
			: base( serial )
		{
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

			if ( Items.Count > 0 )
			{
				m_Timer = new EmptyTimer( this );
				m_Timer.Start();
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( !base.OnDragDrop( from, dropped ) )
			{
				return false;
			}

			if ( TotalItems >= 50 )
			{
				Empty( 501478 ); // The trash is full!  Emptying!
			}
			else
			{
				SendLocalizedMessageTo( from, 1010442 ); // The item will be deleted in three minutes

				if ( m_Timer != null )
				{
					m_Timer.Stop();
				}
				else
				{
					m_Timer = new EmptyTimer( this );
				}

				m_Timer.Start();
			}

			return true;
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p, byte gridloc )
		{
			if ( !base.OnDragDropInto( from, item, p, gridloc ) )
			{
				return false;
			}

			if ( TotalItems >= 50 )
			{
				Empty( 501478 ); // The trash is full!  Emptying!
			}
			else
			{
				SendLocalizedMessageTo( from, 1010442 ); // The item will be deleted in three minutes

				if ( m_Timer != null )
				{
					m_Timer.Stop();
				}
				else
				{
					m_Timer = new EmptyTimer( this );
				}

				m_Timer.Start();
			}

			return true;
		}



		public void Empty( int message )
		{
			List<Item> items = this.Items;

			if ( items.Count > 0 )
			{
				PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, message, "" );

				Point3D location = Location;
				location.Z += 10;

				Effects.SendLocationEffect( location, Map, 0x3709, 10, 10, 0x356, 0 );
				Effects.PlaySound( location, Map, Utility.RandomList( 0x32E, 0x208 ) );

				for ( int i = items.Count - 1; i >= 0; --i )
				{
					if ( i >= items.Count )
						continue;

					items[i].Delete();
				}
			}

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;
		}

		private Timer m_Timer;

		private class EmptyTimer : Timer
		{
			private SacrificialAltarAddon m_Barrel;

			public EmptyTimer( SacrificialAltarAddon barrel )
				: base( TimeSpan.FromMinutes( 3.0 ) )
			{
				m_Barrel = barrel;
			}

			protected override void OnTick()
			{
				m_Barrel.Empty( 501479 ); // Emptying the trashcan!
			}
		}
	}
	public class SacrificialAltarDeed : BaseAddonContainerDeed
	{
		public override BaseAddonContainer Addon { get { return new SacrificialAltarAddon( m_East ); } }
		public override int LabelNumber { get { return 1074818; } } // Sacrificial Altar

		private bool m_East;

		[Constructable]
		public SacrificialAltarDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public SacrificialAltarDeed( Serial serial )
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
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
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
			private SacrificialAltarDeed m_Deed;

			public InternalGump( SacrificialAltarDeed deed )
				: base( 60, 36 )
			{
				m_Deed = deed;

				AddPage( 0 );

				AddBackground( 0, 0, 273, 230, 0x13BE );
				AddImageTiled( 10, 10, 253, 20, 0xA40 );
				AddImageTiled( 10, 40, 253, 150, 0xA40 );
				AddImageTiled( 10, 200, 253, 20, 0xA40 );
				AddAlphaRegion( 10, 10, 253, 210 );
				AddButton( 10, 200, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 45, 200, 200, 20, 1060051, 0x7FFF, false, false ); // CANCEL
				AddHtmlLocalized( 14, 12, 273, 20, 1074818, 0x7FFF, false, false ); // Sacrificial Altar.

				AddPage( 1 );

				AddButton( 19, 49, 0x845, 0x846, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 47, 213, 20, 1075386, 0x7FFF, false, false ); // South
				AddButton( 19, 73, 0x845, 0x846, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 71, 213, 20, 1075387, 0x7FFF, false, false ); // East
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( m_Deed == null || m_Deed.Deleted || info.ButtonID == 0 )
					return;

				m_Deed.m_East = ( info.ButtonID != 1 );
				m_Deed.SendTarget( sender.Mobile );
			}
		}
	}
}