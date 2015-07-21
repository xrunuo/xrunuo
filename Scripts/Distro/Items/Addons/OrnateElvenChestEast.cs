using System;
using Server;

namespace Server.Items
{
	public class OrnateElvenChestEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new OrnateElvenChestEastDeed(); } }


		[Constructable]
		public OrnateElvenChestEastAddon()
		{
			AddComponent( new AddonComponent( 0x2DE9 ), 0, 0, 0 );
		}

		public OrnateElvenChestEastAddon( Serial serial )
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

	public class OrnateElvenChestEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new OrnateElvenChestEastAddon(); } }
		public override int LabelNumber { get { return 1073383; } } // ornate elven chest (east)

		[Constructable]
		public OrnateElvenChestEastDeed()
		{
		}

		public OrnateElvenChestEastDeed( Serial serial )
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