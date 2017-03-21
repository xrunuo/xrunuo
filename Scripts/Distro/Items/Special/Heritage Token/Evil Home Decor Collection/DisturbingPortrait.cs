using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Multis;

namespace Server.Items
{
	[Flipable( 0x2A5D, 0x2A61 )]
	public class DisturbingPortraitComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1074479; } } // Disturbing portrait

		private Timer m_Timer;

		public DisturbingPortraitComponent( int itemID )
			: base( itemID )
		{
			m_Timer = Timer.DelayCall( TimeSpan.FromMinutes( 1 ), TimeSpan.FromMinutes( 1 ), new TimerCallback( Change ) );
		}

		public DisturbingPortraitComponent( Serial serial )
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

			if ( m_Timer != null && m_Timer.Running )
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

			m_Timer = Timer.DelayCall( TimeSpan.FromMinutes( 1 ), TimeSpan.FromMinutes( 1 ), new TimerCallback( Change ) );
		}

		private void Change()
		{
			if ( ItemID < 0x2A61 )
				ItemID = Utility.RandomMinMax( 0x2A5D, 0x2A60 );
			else
				ItemID = Utility.RandomMinMax( 0x2A61, 0x2A64 );
		}
	}

	public class DisturbingPortraitAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new DisturbingPortraitDeed(); } }

		[Constructable]
		public DisturbingPortraitAddon()
			: this( true )
		{
		}

		[Constructable]
		public DisturbingPortraitAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new DisturbingPortraitComponent( 0x2A61 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new DisturbingPortraitComponent( 0x2A5D ), 0, 0, 0 );
			}
		}

		public DisturbingPortraitAddon( Serial serial )
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

	public class DisturbingPortraitDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new DisturbingPortraitAddon( m_East ); } }
		public override int LabelNumber { get { return 1074479; } } // Disturbing portrait

		private bool m_East;

		[Constructable]
		public DisturbingPortraitDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public DisturbingPortraitDeed( Serial serial )
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
			private DisturbingPortraitDeed m_Deed;

			public InternalGump( DisturbingPortraitDeed deed )
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
				AddHtml( 14, 12, 273, 20, @"<CENTER><basefont color=#FFFFFF>Select your Disturbing Portrait position</basefont></CENTER>", false, false ); // Please select your Disturbing Portrait position

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