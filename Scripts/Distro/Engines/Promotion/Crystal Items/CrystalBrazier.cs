using System;
using Server;

namespace Server.Items
{
	public class CrystalBrazierAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalBrazierDeed(); } }
		public override int LabelNumber { get { return 1076667; } } // Crystal Brazier

		[Constructable]
		public CrystalBrazierAddon()
		{
			AddComponent( new AddonComponent( 0x35EF ), 0, 0, 0 );
		}

		public CrystalBrazierAddon( Serial serial )
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

	public class CrystalBrazierDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalBrazierAddon(); } }
		public override int LabelNumber { get { return 1076667; } } // Crystal Brazier

		[Constructable]
		public CrystalBrazierDeed()
		{
			Hue = 1173;
		}

		public CrystalBrazierDeed( Serial serial )
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