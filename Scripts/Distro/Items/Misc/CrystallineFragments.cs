using System;
using Server;

namespace Server.Items
{
	public class CrystallineFragments : Item, ICrystal
	{
		public override int LabelNumber { get { return 1073160; } } // Crystalline Fragments

		[Constructable]
		public CrystallineFragments()
			: base( 0x223B )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 1150;
		}

		public CrystallineFragments( Serial serial )
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

			LootType = LootType.Blessed;
		}
	}
}