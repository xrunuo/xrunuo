using System;
using Server;

namespace Server.Items
{
	public class ObsidianRockAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ObsidianRockDeed(); } }
		public override int LabelNumber { get { return 1076677; } } // Obsidian Rock

		[Constructable]
		public ObsidianRockAddon()
		{
			AddComponent( new AddonComponent( 0x364E ), 0, 0, 0 );
		}

		public ObsidianRockAddon( Serial serial )
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

	public class ObsidianRockDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ObsidianRockAddon(); } }
		public override int LabelNumber { get { return 1076677; } } // Obsidian Rock

		[Constructable]
		public ObsidianRockDeed()
		{
			Hue = 1908;
		}

		public ObsidianRockDeed( Serial serial )
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