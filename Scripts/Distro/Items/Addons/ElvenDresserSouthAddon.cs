using System;
using Server;

namespace Server.Items
{
	public class ElvenDresserSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ElvenDresserSouthDeed(); } }


		[Constructable]
		public ElvenDresserSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2D0A ), 0, 0, 0 );
		}

		public ElvenDresserSouthAddon( Serial serial )
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

	public class ElvenDresserSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ElvenDresserSouthAddon(); } }
		public override int LabelNumber { get { return 1072864; } } // ElvenDresser (South)

		[Constructable]
		public ElvenDresserSouthDeed()
		{
		}

		public ElvenDresserSouthDeed( Serial serial )
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