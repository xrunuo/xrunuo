using System;
using Server;

namespace Server.Items
{
	public class ElvenSpinningWheelSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ElvenSpinningWheelSouthDeed(); } }


		[Constructable]
		public ElvenSpinningWheelSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2DDA ), 0, 0, 0 );
		}

		public ElvenSpinningWheelSouthAddon( Serial serial )
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

	public class ElvenSpinningWheelSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ElvenSpinningWheelSouthAddon(); } }
		public override int LabelNumber { get { return 1072878; } }

		[Constructable]
		public ElvenSpinningWheelSouthDeed()
		{
		}

		public ElvenSpinningWheelSouthDeed( Serial serial )
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