using System;
using Server;

namespace Server.Items
{
	public class VoidInfusedKilt : GargishPlatemailKilt
	{
		public override int LabelNumber { get { return 1113868; } } // Void Infused Kilt

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public VoidInfusedKilt()
		{
			Hue = 1482;

			AbsorptionAttributes.DamageEater = 10;
			Attributes.BonusStr = 5;
			Attributes.BonusDex = 5;
			Attributes.RegenStam = 1;
			Attributes.RegenMana = 1;
			Attributes.AttackChance = 5;
			Resistances.Physical = 5;
			Resistances.Fire = 6;
			Resistances.Cold = 3;
			Resistances.Poison = 3;
			Resistances.Energy = 4;
		}

		public VoidInfusedKilt( Serial serial )
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
