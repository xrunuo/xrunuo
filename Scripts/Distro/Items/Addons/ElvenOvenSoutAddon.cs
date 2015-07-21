using System;
using Server;

namespace Server.Items
{
	public class ElvenOvenSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ElvenOvenSouthDeed(); } }


		[Constructable]
		public ElvenOvenSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2DDB ), 0, 0, 0 );
		}

		public ElvenOvenSouthAddon( Serial serial )
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

	public class ElvenOvenSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ElvenOvenSouthAddon(); } }
		public override int LabelNumber { get { return 1073394; } }

		[Constructable]
		public ElvenOvenSouthDeed()
		{
		}

		public ElvenOvenSouthDeed( Serial serial )
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