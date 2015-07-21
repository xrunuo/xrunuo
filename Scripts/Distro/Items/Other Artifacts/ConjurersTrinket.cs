using System;
using Server;

namespace Server.Items
{
	public class ConjurersTrinket : BaseTalisman
	{
		public override int LabelNumber { get { return 1094800; } } // Conjurer's Trinket

		[Constructable]
		public ConjurersTrinket()
			: base( 0x2F58 )
		{
			Weight = 1.0;
			Hue = 1157;

			Slayer = SlayerName.Undead;
			Attributes.BonusStr = 1;
			Attributes.RegenHits = 2;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 20;
		}

		public ConjurersTrinket( Serial serial )
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
