using System;
using Server;

namespace Server.Items
{
	public class LargeElvenBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LargeElvenBedEastDeed(); } }

		[Constructable]
		public LargeElvenBedEastAddon()
		{
			AddComponent( new AddonComponent( 0x3054 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x3053 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0x3055 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 0x3052 ), 2, 1, 0 );
		}

		public LargeElvenBedEastAddon( Serial serial )
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

	public class LargeElvenBedEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LargeElvenBedEastAddon(); } }
		public override int LabelNumber { get { return 1044324; } } // large elven bed (east)

		[Constructable]
		public LargeElvenBedEastDeed()
		{
		}

		public LargeElvenBedEastDeed( Serial serial )
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