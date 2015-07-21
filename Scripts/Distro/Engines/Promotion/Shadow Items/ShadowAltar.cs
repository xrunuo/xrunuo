using System;
using Server;

namespace Server.Items
{
	public class ShadowAltarAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ShadowAltarDeed(); } }
		public override int LabelNumber { get { return 1076682; } } // Shadow Altar

		[Constructable]
		public ShadowAltarAddon()
		{
			AddComponent( new AddonComponent( 0x365A ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x365B ), -1, 0, 0 );
		}

		public ShadowAltarAddon( Serial serial )
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
		}
	}

	public class ShadowAltarDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ShadowAltarAddon(); } }
		public override int LabelNumber { get { return 1076682; } } // Shadow Altar

		[Constructable]
		public ShadowAltarDeed()
		{
			Hue = 1908;
		}

		public ShadowAltarDeed( Serial serial )
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
		}
	}
}