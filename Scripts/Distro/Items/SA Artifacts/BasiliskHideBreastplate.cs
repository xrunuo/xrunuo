using System;
using Server;

namespace Server.Items
{
	public class BasiliskHideBreastplate : DragonChest
	{
		public override int LabelNumber { get { return 1115444; } } // Basilisk Hide Breastplate

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		public override CraftResource DefaultResource { get { return CraftResource.None; } }

		[Constructable]
		public BasiliskHideBreastplate()
		{
			Hue = 0x368;

			AbsorptionAttributes.DamageEater = 10;
			Attributes.BonusDex = 5;
			Attributes.RegenHits = 2;
			Attributes.RegenStam = 2;
			Attributes.RegenMana = 1;
			Attributes.DefendChance = 5;
			Attributes.LowerManaCost = 5;
			Resistances.Physical = 9;
			Resistances.Fire = 11;
			Resistances.Cold = 3;
			Resistances.Poison = 8;
			Resistances.Energy = 2;
		}

		public BasiliskHideBreastplate( Serial serial )
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
