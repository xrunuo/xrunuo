using System;
using Server;

namespace Server.Items
{
	public class GargishCouchEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new GargishCouchEastDeed(); } }

		[Constructable]
		public GargishCouchEastAddon()
		{
			AddComponent( new AddonComponent( 0x4029 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x402A ), 0, 1, 0 );
		}

		public GargishCouchEastAddon( Serial serial )
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

	public class GargishCouchEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new GargishCouchEastAddon(); } }
		public override int LabelNumber { get { return 1111776; } } // gargish couch (east)

		[Constructable]
		public GargishCouchEastDeed()
		{
		}

		public GargishCouchEastDeed( Serial serial )
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