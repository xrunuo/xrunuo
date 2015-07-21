using System;
using Server;

namespace Server.Items
{
	public class ShadowPillarAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ShadowPillarDeed(); } }
		public override int LabelNumber { get { return 1076679; } } // Shadow Pillar

		[Constructable]
		public ShadowPillarAddon()
		{
			AddComponent( new AddonComponent( 0x3650 ), 0, 0, 0 );
		}

		public ShadowPillarAddon( Serial serial )
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

	public class ShadowPillarDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ShadowPillarAddon(); } }
		public override int LabelNumber { get { return 1076679; } } // Shadow Pillar

		[Constructable]
		public ShadowPillarDeed()
		{
			Hue = 1908;
		}

		public ShadowPillarDeed( Serial serial )
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