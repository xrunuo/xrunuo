using System;

namespace Server.Items
{
	public class ManaPhasingOrb : BaseTalisman
	{
		public override int LabelNumber { get { return 1116230; } } // Mana Phasing Orb

		public override int InitMinHits { get { return 75; } }
		public override int InitMaxHits { get { return 75; } }

		public override bool Brittle { get { return true; } }

		[Constructable]
		public ManaPhasingOrb()
			: base( 0x1096 )
		{
			Weight = 1.0;
			Hue = 0x48D;

			TalismanType = TalismanType.ManaPhase;
			Charges = 50;

			Attributes.LowerManaCost = 6;

			switch ( Utility.Random( 3 ) )
			{
				case 0:
					Attributes.RegenHits = 1;
					break;
				case 1:
					Attributes.RegenStam = 1;
					break;
				case 2:
					Attributes.RegenMana = 1;
					break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0:
					Attributes.AttackChance = 5;
					break;
				case 1:
					Attributes.DefendChance = 5;
					break;
			}

			switch ( Utility.Random( 3 ) )
			{
				case 0:
					Attributes.LowerRegCost = 10;
					break;
				case 1:
					Attributes.WeaponDamage = 15;
					break;
				case 2:
					Attributes.SpellDamage = 5;
					break;
			}
		}

		public ManaPhasingOrb( Serial serial )
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
