using System;
using Server;

namespace Server.Items
{
	public class FleshRipper : AssassinSpike
	{
		public override int LabelNumber { get { return 1075045; } } // Flesh Ripper

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public FleshRipper()
		{
			Hue = 833;
			Attributes.WeaponSpeed = 40;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.BonusStr = 5;
			SkillBonuses.SetValues( 0, SkillName.Anatomy, 10.0 );
			Attributes.AttackChance = 15;
			Weight = 4.0;
			Slayer3 = TalisSlayerName.Mage;
		}

		public FleshRipper( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( Slayer3 != TalisSlayerName.Mage )
				Slayer3 = TalisSlayerName.Mage;
		}
	}
}