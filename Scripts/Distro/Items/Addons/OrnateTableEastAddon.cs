using System;
using Server;

namespace Server.Items
{
	public class OrnateTableEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new OrnateTableEastDeed(); } }


		[Constructable]
		public OrnateTableEastAddon()
		{
			AddComponent( new AddonComponent( 0x2DE1 ), 0, 0, 0 );
		}

		public OrnateTableEastAddon( Serial serial )
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

	public class OrnateTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new OrnateTableEastAddon(); } }
		public override int LabelNumber { get { return 1073384; } }

		[Constructable]
		public OrnateTableEastDeed()
		{
		}

		public OrnateTableEastDeed( Serial serial )
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