using System;
using Server;

namespace Server.Items
{
	public class SpikePostAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SpikePostDeed(); } }
		public override int LabelNumber { get { return 1076676; } } // Spike Post

		[Constructable]
		public SpikePostAddon()
		{
			AddComponent( new AddonComponent( 0x364D ), 0, 0, 0 );
		}

		public SpikePostAddon( Serial serial )
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

	public class SpikePostDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SpikePostAddon(); } }
		public override int LabelNumber { get { return 1076676; } } // Spike Post

		[Constructable]
		public SpikePostDeed()
		{
			Hue = 1908;
		}

		public SpikePostDeed( Serial serial )
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