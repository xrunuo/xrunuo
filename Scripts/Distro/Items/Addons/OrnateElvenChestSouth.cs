using System;
using Server;

namespace Server.Items
{
	public class OrnateElvenChestSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new OrnateElvenChestSouthDeed(); } }


		[Constructable]
		public OrnateElvenChestSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2DEA ), 0, 0, 0 );
		}

		public OrnateElvenChestSouthAddon( Serial serial )
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

	public class OrnateElvenChestSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new OrnateElvenChestSouthAddon(); } }
		public override int LabelNumber { get { return 1032440; } }

		[Constructable]
		public OrnateElvenChestSouthDeed()
		{
		}

		public OrnateElvenChestSouthDeed( Serial serial )
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