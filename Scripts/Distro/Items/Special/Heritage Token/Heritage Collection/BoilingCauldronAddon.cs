
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Items
{

	public class BoilingCauldronAddon : BaseAddon
	{
		private static int[,] m_AddOnSimpleComponents;

		public override BaseAddonDeed Deed { get { return new BoilingCauldronAddonDeed(); } }

		[Constructable]
		public BoilingCauldronAddon()
			: this( true )
		{
		}

		[Constructable]
		public BoilingCauldronAddon( bool east )
			: base()
		{
			if ( east )
			{
				m_AddOnSimpleComponents = new int[,] { { 2416, 0, 0, 8 }, { 2421, 0, 0, 0 } };
				for ( int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++ )
					AddComponent( new AddonComponent( m_AddOnSimpleComponents[i, 0] ), m_AddOnSimpleComponents[i, 1], m_AddOnSimpleComponents[i, 2], m_AddOnSimpleComponents[i, 3] );
				AddComplexComponent( (BaseAddon) this, 4012, 0, 0, 0, 0, 0, "", 1 );// 3
			}
			else
			{
				m_AddOnSimpleComponents = new int[,] { { 2416, 0, 0, 8 }, { 2420, 0, 0, 0 } };
				for ( int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++ )
					AddComponent( new AddonComponent( m_AddOnSimpleComponents[i, 0] ), m_AddOnSimpleComponents[i, 1], m_AddOnSimpleComponents[i, 2], m_AddOnSimpleComponents[i, 3] );
				AddComplexComponent( (BaseAddon) this, 4012, 0, 0, 0, 0, 0, "", 1 );// 3
			}

		}

		public BoilingCauldronAddon( Serial serial )
			: base( serial )
		{
		}

		private static void AddComplexComponent( BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource )
		{
			AddComplexComponent( addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1 );
		}

		private static void AddComplexComponent( BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount )
		{
			AddonComponent ac;
			ac = new AddonComponent( item );
			if ( name != null && name.Length > 0 )
				ac.Name = name;
			if ( hue != 0 )
				ac.Hue = hue;
			if ( amount > 1 )
			{
				ac.Stackable = true;
				ac.Amount = amount;
			}
			if ( lightsource != -1 )
				ac.Light = (LightType) lightsource;
			addon.AddComponent( ac, xoffset, yoffset, zoffset );
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

	public class BoilingCauldronAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new BoilingCauldronAddon( m_East ); } }

		private bool m_East;

		[Constructable]
		public BoilingCauldronAddonDeed()
			: base()
		{
			Name = "A Boiling Cauldron";
			LootType = LootType.Blessed;
		}

		public BoilingCauldronAddonDeed( Serial serial )
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

			/*int version = */
			reader.ReadEncodedInt();
		}

		private class InternalGump : Gump
		{
			private BoilingCauldronAddonDeed m_Deed;

			public InternalGump( BoilingCauldronAddonDeed deed )
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
				AddHtml( 14, 12, 273, 20, @"<CENTER><basefont color=#FFFFFF>Select your boiling cauldron position</basefont></CENTER>", false, false ); // Please select your boiling cauldron position

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
