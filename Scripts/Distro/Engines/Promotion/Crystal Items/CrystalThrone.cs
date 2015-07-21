using System;
using Server;

namespace Server.Items
{
	public class CrystalThroneAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalThroneDeed(); } }
		public override int LabelNumber { get { return 1076666; } } // Crystal Throne

		[Constructable]
		public CrystalThroneAddon()
		{
			AddComponent( new AddonComponent( 0x35EE ), 0, 0, 0 );
		}

		public CrystalThroneAddon( Serial serial )
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

	public class CrystalThroneDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalThroneAddon(); } }
		public override int LabelNumber { get { return 1076666; } } // Crystal Throne

		[Constructable]
		public CrystalThroneDeed()
		{
			Hue = 1173;
		}

		public CrystalThroneDeed( Serial serial )
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