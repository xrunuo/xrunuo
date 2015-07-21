using System;
using Server;

namespace Server.Items
{
	public class ElvenWashBasinSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ElvenWashBasinSouthDeed(); } }


		[Constructable]
		public ElvenWashBasinSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2D0C ), 0, 0, 0 );
		}

		public ElvenWashBasinSouthAddon( Serial serial )
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

	public class ElvenWashBasinSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ElvenWashBasinSouthAddon(); } }
		public override int LabelNumber { get { return 1072865; } } // Elven Wash Basin (South)

		[Constructable]
		public ElvenWashBasinSouthDeed()
		{
		}

		public ElvenWashBasinSouthDeed( Serial serial )
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