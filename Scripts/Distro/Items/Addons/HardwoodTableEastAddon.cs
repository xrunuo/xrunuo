using System;
using Server;

namespace Server.Items
{
	public class HardwoodTableEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new HardwoodTableEastDeed(); } }


		[Constructable]
		public HardwoodTableEastAddon()
		{
			AddComponent( new AddonComponent( 0x2DE7 ), 0, 0, 0 );
		}

		public HardwoodTableEastAddon( Serial serial )
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

	public class HardwoodTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new HardwoodTableEastAddon(); } }
		public override int LabelNumber { get { return 1073386; } }

		[Constructable]
		public HardwoodTableEastDeed()
		{
		}

		public HardwoodTableEastDeed( Serial serial )
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