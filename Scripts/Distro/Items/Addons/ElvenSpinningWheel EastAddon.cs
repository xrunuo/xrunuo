using System;
using Server;

namespace Server.Items
{
	public class ElvenSpinningWheelEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ElvenSpinningWheelEastDeed(); } }


		[Constructable]
		public ElvenSpinningWheelEastAddon()
		{
			AddComponent( new AddonComponent( 0x2DD9 ), 0, 0, 0 );
		}

		public ElvenSpinningWheelEastAddon( Serial serial )
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

	public class ElvenSpinningWheelEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ElvenSpinningWheelEastAddon(); } }
		public override int LabelNumber { get { return 1073393; } }

		[Constructable]
		public ElvenSpinningWheelEastDeed()
		{
		}

		public ElvenSpinningWheelEastDeed( Serial serial )
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