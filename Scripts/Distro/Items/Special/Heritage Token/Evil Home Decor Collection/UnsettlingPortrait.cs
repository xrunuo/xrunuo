using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Multis;

namespace Server.Items
{
	[Flipable( 0x2A65, 0x2A67 )]
	public class UnsettlingPortraitComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1074480; } } // Unsettling portrait

		private Timer m_Timer;

		public UnsettlingPortraitComponent( int itemID )
			: base( itemID )
		{
			m_Timer = Timer.DelayCall( TimeSpan.FromMinutes( 3 ), TimeSpan.FromMinutes( 3 ), new TimerCallback( ChangeDirection ) );
		}


		public UnsettlingPortraitComponent( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.InRange( from, 2 ) )
				Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x567, 0x568 ) );
			else
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if ( m_Timer != null )
				m_Timer.Stop();
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

			m_Timer = Timer.DelayCall( TimeSpan.FromMinutes( 3 ), TimeSpan.FromMinutes( 3 ), new TimerCallback( ChangeDirection ) );
		}

		private void ChangeDirection()
		{
			if ( ItemID == 0x2A65 )
				ItemID += 1;
			else if ( ItemID == 0x2A66 )
				ItemID -= 1;
			else if ( ItemID == 0x2A67 )
				ItemID += 1;
			else if ( ItemID == 0x2A68 )
				ItemID -= 1;
		}
	}

	public class UnsettlingPortraitAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new UnsettlingPortraitDeed(); } }

		[Constructable]
		public UnsettlingPortraitAddon()
			: this( true )
		{
		}

		[Constructable]
		public UnsettlingPortraitAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new UnsettlingPortraitComponent( 0x2A67 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new UnsettlingPortraitComponent( 0x2A65 ), 0, 0, 0 );
			}
		}

		public UnsettlingPortraitAddon( Serial serial )
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

	public class UnsettlingPortraitDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new UnsettlingPortraitAddon( m_East ); } }
		public override int LabelNumber { get { return 1074480; } } // Unsettling portrait

		private bool m_East;

		[Constructable]
		public UnsettlingPortraitDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public UnsettlingPortraitDeed( Serial serial )
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
			private UnsettlingPortraitDeed m_Deed;

			public InternalGump( UnsettlingPortraitDeed deed )
				: base( 60, 36 )
			{
				m_Deed = deed;

				AddPage( 0 );

				AddBackground( 0, 0, 273, 324, 0x13BE );
				AddImageTiled( 10, 10, 253, 20, 0xA40 );
				AddImageTiled( 10, 40, 253, 244, 0xA40 );
				AddImageTiled( 10, 294, 253, 20, 0xA40 );
				AddAlphaRegion( 10, 10, 253, 304 );
				AddButton( 10, 294, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 45, 296, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL
				AddHtml( 14, 12, 273, 20, @"<CENTER><basefont color=#FFFFFF>Select your Unsettling Portrait position</basefont></CENTER>", false, false ); // Please select your Unsettling Portrait position

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