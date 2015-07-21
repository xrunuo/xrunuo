using System;
using Server;

namespace Server.Items
{
	public class CrystalBeggarStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalBeggarStatueDeed(); } }
		public override int LabelNumber { get { return 1076668; } } // Crystal BeggarStatue

		[Constructable]
		public CrystalBeggarStatueAddon()
		{
			AddComponent( new AddonComponent( 0x35F9 ), 0, 0, 0 );
		}

		public CrystalBeggarStatueAddon( Serial serial )
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

	public class CrystalBeggarStatueDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalBeggarStatueAddon(); } }
		public override int LabelNumber { get { return 1076668; } } // Crystal BeggarStatue

		[Constructable]
		public CrystalBeggarStatueDeed()
		{
			Hue = 1173;
		}

		public CrystalBeggarStatueDeed( Serial serial )
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