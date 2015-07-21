using System;
using Server;

namespace Server.Items
{
	public class CrystalAltarAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalAltarDeed(); } }
		public override int LabelNumber { get { return 1076672; } } // Crystal Altar

		[Constructable]
		public CrystalAltarAddon()
		{
			AddComponent( new AddonComponent( 0x3602 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3603 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0x3604 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0x3DA2 ), -1, -1, 0 );
		}

		public CrystalAltarAddon( Serial serial )
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

	public class CrystalAltarDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalAltarAddon(); } }
		public override int LabelNumber { get { return 1076672; } } // Crystal Altar

		[Constructable]
		public CrystalAltarDeed()
		{
			Hue = 1173;
		}

		public CrystalAltarDeed( Serial serial )
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