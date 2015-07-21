using System;
using Server;

namespace Server.Items
{
	public class TalismanTravelingMinstrel : BaseTalisman, ICollectionItem
	{
		public override int LabelNumber { get { return 1074891; } } // Library Talisman - The Life of a Traveling Minstrel Curse Remover

		[Constructable]
		public TalismanTravelingMinstrel()
			: base( 0x2F5B )
		{
			Weight = 1.0;
			ProtectionTalis = ProtectionKillerEntry.GetRandom();
			ProtectionValue = 1 + Utility.Random( 59 );
			SkillBonuses.SetValues( 0, SkillName.Provocation, 5.0 );
			SkillBonuses.SetValues( 1, SkillName.Musicianship, 5.0 );
			TalismanType = TalismanType.CurseRemoval;
			Charges = -1;
		}

		public TalismanTravelingMinstrel( Serial serial )
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
