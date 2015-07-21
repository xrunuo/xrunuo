using System;
using Server;

namespace Server.Items
{
	public class SpikeColumnAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SpikeColumnDeed(); } }
		public override int LabelNumber { get { return 1076675; } } // Spike Column

		[Constructable]
		public SpikeColumnAddon()
		{
			AddComponent( new AddonComponent( 0x364C ), 0, 0, 0 );
		}

		public SpikeColumnAddon( Serial serial )
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

	public class SpikeColumnDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SpikeColumnAddon(); } }
		public override int LabelNumber { get { return 1076675; } } // Spike Column

		[Constructable]
		public SpikeColumnDeed()
		{
			Hue = 1908;
		}

		public SpikeColumnDeed( Serial serial )
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