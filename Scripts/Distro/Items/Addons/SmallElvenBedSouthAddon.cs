using System;
using Server;

namespace Server.Items
{
	public class SmallElvenBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SmallElvenBedSouthDeed(); } }

		[Constructable]
		public SmallElvenBedSouthAddon()
		{
			AddComponent( new AddonComponent( 0x3051 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3050 ), 0, 1, 0 );
		}

		public SmallElvenBedSouthAddon( Serial serial )
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

	public class SmallElvenBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SmallElvenBedSouthAddon(); } }
		public override int LabelNumber { get { return 1044321; } } // small elven bed (south)

		[Constructable]
		public SmallElvenBedSouthDeed()
		{
		}

		public SmallElvenBedSouthDeed( Serial serial )
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