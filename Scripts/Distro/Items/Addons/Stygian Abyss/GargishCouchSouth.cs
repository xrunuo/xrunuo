using System;
using Server;

namespace Server.Items
{
	public class GargishCouchSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new GargishCouchSouthDeed(); } }

		[Constructable]
		public GargishCouchSouthAddon()
		{
			AddComponent( new AddonComponent( 0x4027 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x4028 ), 1, 0, 0 );
		}

		public GargishCouchSouthAddon( Serial serial )
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

	public class GargishCouchSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new GargishCouchSouthAddon(); } }
		public override int LabelNumber { get { return 1111775; } } // gargish couch (south)

		[Constructable]
		public GargishCouchSouthDeed()
		{
		}

		public GargishCouchSouthDeed( Serial serial )
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