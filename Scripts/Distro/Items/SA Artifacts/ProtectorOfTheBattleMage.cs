using System;
using Server;

namespace Server.Items
{
	public class ProtectorOfTheBattleMage : LeatherChest
	{
		public override int LabelNumber { get { return 1113761; } } // Protector of the Battle Mage

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public ProtectorOfTheBattleMage()
		{
			Hue = 532;

			Attributes.CastingFocus = 3;
			Attributes.RegenMana = 2;
			Attributes.SpellDamage = 5;
			Attributes.LowerManaCost = 8;
			Attributes.LowerRegCost = 10;

			Resistances.Physical = 8;
			Resistances.Fire = 12;
			Resistances.Cold = 7;
			Resistances.Poison = 5;
			Resistances.Energy = 5;
		}

		public ProtectorOfTheBattleMage( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
