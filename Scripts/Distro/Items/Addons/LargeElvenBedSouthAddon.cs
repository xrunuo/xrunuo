using System;
using Server;

namespace Server.Items
{
	public class LargeElvenBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LargeElvenBedSouthDeed(); } }

		[Constructable]
		public LargeElvenBedSouthAddon()
		{
			AddComponent( new AddonComponent( 0x3059 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0x3058 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3057 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0x3056 ), 0, 1, 0 );
		}

		public LargeElvenBedSouthAddon( Serial serial )
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

	public class LargeElvenBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LargeElvenBedSouthAddon(); } }
		public override int LabelNumber { get { return 1044323; } } // large elven bed (south)

		[Constructable]
		public LargeElvenBedSouthDeed()
		{
		}

		public LargeElvenBedSouthDeed( Serial serial )
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