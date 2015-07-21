using System;
using Server;

namespace Server.Items
{
	public class CrystalTableAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalTableDeed(); } }
		public override int LabelNumber { get { return 1076673; } } // Crystal Table

		[Constructable]
		public CrystalTableAddon()
		{
			AddComponent( new AddonComponent( 0x3605 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3606 ), 0, -1, 0 );
		}

		public CrystalTableAddon( Serial serial )
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

	public class CrystalTableDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalTableAddon(); } }
		public override int LabelNumber { get { return 1076673; } } // Crystal Table

		[Constructable]
		public CrystalTableDeed()
		{
			Hue = 1173;
		}

		public CrystalTableDeed( Serial serial )
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