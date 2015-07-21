using System;
using Server;

namespace Server.Items
{
	public class ShadowBannerAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ShadowBannerDeed(); } }
		public override int LabelNumber { get { return 1076683; } } // Shadow Banner

		[Constructable]
		public ShadowBannerAddon()
		{
			AddComponent( new AddonComponent( 0x365C ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x365D ), -1, 0, 0 );
		}

		public ShadowBannerAddon( Serial serial )
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

	public class ShadowBannerDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ShadowBannerAddon(); } }
		public override int LabelNumber { get { return 1076683; } } // Shadow Banner

		[Constructable]
		public ShadowBannerDeed()
		{
			Hue = 1908;
		}

		public ShadowBannerDeed( Serial serial )
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