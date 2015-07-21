using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardLordBritishsThroneAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new MuseumRewardLordBritishsThroneDeed(); } }
		public override int LabelNumber { get { return 1073243; } } // Replica of Lord British's Throne - Museum of Vesper

		[Constructable]
		public MuseumRewardLordBritishsThroneAddon()
		{
			AddComponent( new AddonComponent( 0x1526 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x1527 ), 0, -1, 0 );
		}

		public MuseumRewardLordBritishsThroneAddon( Serial serial )
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

	public class MuseumRewardLordBritishsThroneDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new MuseumRewardLordBritishsThroneAddon(); } }
		public override int LabelNumber { get { return 1073243; } } // Replica of Lord British's Throne - Museum of Vesper

		[Constructable]
		public MuseumRewardLordBritishsThroneDeed()
		{
		}

		public MuseumRewardLordBritishsThroneDeed( Serial serial )
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