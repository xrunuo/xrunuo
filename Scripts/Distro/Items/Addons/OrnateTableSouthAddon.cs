using System;
using Server;

namespace Server.Items
{
	public class OrnateTableSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new OrnateTableSouthDeed(); } }


		[Constructable]
		public OrnateTableSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2DE2 ), 0, 0, 0 );
		}

		public OrnateTableSouthAddon( Serial serial )
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

	public class OrnateTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new OrnateTableSouthAddon(); } }
		public override int LabelNumber { get { return 1072869; } }

		[Constructable]
		public OrnateTableSouthDeed()
		{
		}

		public OrnateTableSouthDeed( Serial serial )
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