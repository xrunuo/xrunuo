using System;
using Server;

namespace Server.Items
{
	public class SmallElvenBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SmallElvenBedEastDeed(); } }

		[Constructable]
		public SmallElvenBedEastAddon()
		{
			AddComponent( new AddonComponent( 0x304D ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x304C ), 1, 0, 0 );
		}

		public SmallElvenBedEastAddon( Serial serial )
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

	public class SmallElvenBedEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SmallElvenBedEastAddon(); } }
		public override int LabelNumber { get { return 1044322; } } // small elven bed (east)

		[Constructable]
		public SmallElvenBedEastDeed()
		{
		}

		public SmallElvenBedEastDeed( Serial serial )
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