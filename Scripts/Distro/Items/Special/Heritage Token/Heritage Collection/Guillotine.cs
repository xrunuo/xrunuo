using System;

using Server;
using Server.Spells;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x125E, 0x1245 )]
	public class GuillotineComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1024656; } } // Guillotine

		public GuillotineComponent( int itemID )
			: base( itemID )
		{
		}

		public GuillotineComponent( Serial serial )
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


	public class GuillotineAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new GuillotineDeed(); } }

		[Constructable]
		public GuillotineAddon()
			: this( true )
		{
		}

		[Constructable]
		public GuillotineAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new GuillotineComponent( 0x125E ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new GuillotineComponent( 0x1247 ), 0, 0, 0 );
			}
		}

		public GuillotineAddon( Serial serial )
			: base( serial )
		{
		}

		public override void OnComponentUsed( AddonComponent c, Mobile from )
		{
			if ( from.InRange( Location, 2 ) )
			{
				if ( Utility.RandomBool() )
				{
					from.Location = Location;

					Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( Activate ), new object[] { c, from } );
				}
				else
					from.LocalOverheadMessage( MessageType.Regular, 0, 501777 ); // Hmm... you suspect that if you used this again, it might hurt.
			}
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

		private void Activate( object obj )
		{
			object[] param = (object[]) obj;

			if ( param[0] is AddonComponent && param[1] is Mobile )
				Activate( (AddonComponent) param[0], (Mobile) param[1] );
		}

		public virtual void Activate( AddonComponent c, Mobile from )
		{
			if ( c.ItemID == 0x125E )
				c.ItemID = 0x1269;
			else
				c.ItemID = 0x1245;

			// blood
			int amount = Utility.RandomMinMax( 3, 7 );

			for ( int i = 0; i < amount; i++ )
			{
				int x = c.X + Utility.RandomMinMax( -1, 1 );
				int y = c.Y + Utility.RandomMinMax( -1, 1 );
				int z = c.Z;

				if ( !c.Map.CanFit( x, y, z, 1, false, false, true ) )
				{
					z = c.Map.GetAverageZ( x, y );

					if ( !c.Map.CanFit( x, y, z, 1, false, false, true ) )
						continue;
				}

				Blood blood = new Blood( Utility.RandomMinMax( 0x122C, 0x122F ) );
				blood.MoveToWorld( new Point3D( x, y, z ), c.Map );
			}

			if ( from.Female )
				from.PlaySound( Utility.RandomMinMax( 0x150, 0x153 ) );
			else
				from.PlaySound( Utility.RandomMinMax( 0x15A, 0x15D ) );

			from.LocalOverheadMessage( MessageType.Regular, 0, 501777 ); // Hmm... you suspect that if you used this again, it might hurt.
			SpellHelper.Damage( TimeSpan.Zero, from, Utility.Dice( 2, 10, 5 ) );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.5 ), 2, new TimerStateCallback( Deactivate ), c );
		}

		private void Deactivate( object obj )
		{
			if ( obj is AddonComponent )
			{
				AddonComponent c = (AddonComponent) obj;


				if ( c.ItemID == 0x1269 )
					c.ItemID = 0x1260;
				else if ( c.ItemID == 0x1260 )
					c.ItemID = 0x125E;
				else if ( c.ItemID == 0x1245 )
					c.ItemID = 0x1246;
				else if ( c.ItemID == 0x1246 )
					c.ItemID = 0x1247;
			}
		}
	}

	public class GuillotineDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new GuillotineAddon( m_East ); } }
		public override int LabelNumber { get { return 1024656; } } // Guillotine

		private bool m_East;

		[Constructable]
		public GuillotineDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public GuillotineDeed( Serial serial )
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
			private GuillotineDeed m_Deed;

			public InternalGump( GuillotineDeed deed )
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
				AddHtml( 14, 12, 273, 20, @"<CENTER><basefont color=#FFFFFF>Select your Guillotine position</basefont></CENTER>", false, false ); // Please select your Guillotine position

				AddPage( 1 );

				AddButton( 19, 49, 0x845, 0x846, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 47, 213, 20, 1075386, 0x7FFF, false, false ); // South
				AddButton( 19, 73, 0x845, 0x846, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 71, 213, 20, 1075387, 0x7FFF, false, false ); // East
			}

			public override void OnResponse( GameClient sender, RelayInfo info )
			{
				if ( m_Deed == null || m_Deed.Deleted || info.ButtonID == 0 )
					return;

				m_Deed.m_East = ( info.ButtonID != 1 );
				m_Deed.SendTarget( sender.Mobile );
			}
		}
	}
}