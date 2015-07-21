using System;
using Server;

namespace Server.Items
{
	public class HardwoodTableSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new HardwoodTableSouthDeed(); } }


		[Constructable]
		public HardwoodTableSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2DE8 ), 0, 0, 0 );
		}

		public HardwoodTableSouthAddon( Serial serial )
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

	public class HardwoodTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new HardwoodTableSouthAddon(); } }
		public override int LabelNumber { get { return 1073385; } }

		[Constructable]
		public HardwoodTableSouthDeed()
		{
		}

		public HardwoodTableSouthDeed( Serial serial )
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