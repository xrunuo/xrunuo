using System;
using Server;

namespace Server.Items
{
	public class ObsidianPillarAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ObsidianPillarDeed(); } }
		public override int LabelNumber { get { return 1076678; } } // Obsidian Pillar

		[Constructable]
		public ObsidianPillarAddon()
		{
			AddComponent( new AddonComponent( 0x364F ), 0, 0, 0 );
		}

		public ObsidianPillarAddon( Serial serial )
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

	public class ObsidianPillarDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ObsidianPillarAddon(); } }
		public override int LabelNumber { get { return 1076678; } } // Obsidian Pillar

		[Constructable]
		public ObsidianPillarDeed()
		{
			Hue = 1908;
		}

		public ObsidianPillarDeed( Serial serial )
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