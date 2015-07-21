using System;
using Server;

namespace Server.Items
{
	public class TalismanBirdsofBritannia : BaseTalisman, ICollectionItem
	{
		public override int LabelNumber { get { return 1074892; } } // Library Talisman - Birds of Britannia Random Summoner

		[Constructable]
		public TalismanBirdsofBritannia()
			: base( 0x2F5A )
		{
			Name = "Library Talisman - Birds of Britannia Random Summoner";
			Weight = 1.0;
			TalismanType = TalismanType.SummonRandom;
			SkillBonuses.SetValues( 0, SkillName.AnimalTaming, 5.0 );
			SkillBonuses.SetValues( 1, SkillName.AnimalLore, 5.0 );
			TalisSlayer = TalisSlayerName.Bird;
			Charges = -1;
		}

		public TalismanBirdsofBritannia( Serial serial )
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
