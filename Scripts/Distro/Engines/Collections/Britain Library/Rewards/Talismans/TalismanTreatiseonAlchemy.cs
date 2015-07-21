using System;
using Server;

namespace Server.Items
{
	public class TalismanTreatiseonAlchemy : BaseTalisman, ICollectionItem
	{
		public override int LabelNumber { get { return 1073353; } } // Library Talisman - Treatise on Alchemy

		[Constructable]
		public TalismanTreatiseonAlchemy()
			: base( 0x2F58 )
		{
			Weight = 1.0;
			SkillBonuses.SetValues( 0, SkillName.Magery, 5.0 );
			Attributes.EnhancePotions = 15;
			CraftBonusRegular = CraftList.Alchemy;
			CraftBonusRegularValue = 1 + Utility.Random( 30 );
		}

		public TalismanTreatiseonAlchemy( Serial serial )
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
