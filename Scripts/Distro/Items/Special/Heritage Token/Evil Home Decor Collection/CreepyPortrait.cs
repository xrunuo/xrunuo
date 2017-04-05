using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Multis;

namespace Server.Items
{
	[Flipable( 0x2A69, 0x2A6D )]
	public class CreepyPortraitComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1074481; } } // Creepy portrait

		public override bool HandlesOnMovement { get { return true; } }

		private bool m_IsChanging = false;

		public CreepyPortraitComponent( int itemID )
			: base( itemID )
		{
		}

		public CreepyPortraitComponent( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.InRange( from, 2 ) )
				Effects.PlaySound( Location, Map, Utility.RandomMinMax( 0x565, 0x566 ) );
			else
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
		}

		public override void OnMovement( Mobile m, Point3D old )
		{
			if ( m.Alive && m.Player && ( m.AccessLevel == AccessLevel.Player || !m.Hidden ) )
			{
				if ( !this.InRange( old, 2 ) && this.InRange( m, 2 ) )
				{
					if ( ( !m_IsChanging ) && ( ItemID == 0x2A69 || ItemID == 0x2A6D ) )
					{
						m_IsChanging = true;
						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ), 3, new TimerCallback( Up ) );
					}
				}
				else if ( this.InRange( old, 2 ) && !this.InRange( m, 2 ) )
				{
					if ( ( !m_IsChanging ) && ( ItemID == 0x2A6C || ItemID == 0x2A70 ) )
					{
						m_IsChanging = true;
						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ), 3, new TimerCallback( Down ) );
					}
				}
			}
		}

		private void Up()
		{
			ItemID += 1;
			if ( ItemID == 0x2A6C || ItemID == 0x2A70 )
				m_IsChanging = false;
		}

		private void Down()
		{
			ItemID -= 1;
			if ( ItemID == 0x2A69 || ItemID == 0x2A6D )
				m_IsChanging = false;
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

	public class CreepyPortraitAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CreepyPortraitDeed(); } }

		[Constructable]
		public CreepyPortraitAddon()
			: this( true )
		{
		}

		[Constructable]
		public CreepyPortraitAddon( bool east )
			: base()
		{
			if ( east )
				AddComponent( new CreepyPortraitComponent( 0x2A6D ), 0, 0, 0 );
			else
				AddComponent( new CreepyPortraitComponent( 0x2A69 ), 0, 0, 0 );
		}

		public CreepyPortraitAddon( Serial serial )
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

	public class CreepyPortraitDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CreepyPortraitAddon( m_East ); } }
		public override int LabelNumber { get { return 1074481; } } // Creepy portrait

		private bool m_East;

		[Constructable]
		public CreepyPortraitDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public CreepyPortraitDeed( Serial serial )
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

			/*int version = */
			reader.ReadEncodedInt();
		}

		private class InternalGump : Gump
		{
			private CreepyPortraitDeed m_Deed;

			public InternalGump( CreepyPortraitDeed deed )
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
				AddHtml( 14, 12, 273, 20, @"<CENTER><basefont color=#FFFFFF>Select your Creepy Portrait position</basefont></CENTER>", false, false ); // Please select your Creepy Portrait position

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