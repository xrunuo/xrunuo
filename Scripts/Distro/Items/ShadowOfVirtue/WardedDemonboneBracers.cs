using System;
using Server;

namespace Server.Items
{
	public class WardedDemonboneBracers : BoneArms
	{
		public override int LabelNumber { get { return 1115775; } } // Warded Demonbone Bracers

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public WardedDemonboneBracers()
		{
			Hue = 0x2E2;

			Attributes.CastingFocus = 2;
			Attributes.RegenMana = 1;
			Attributes.LowerManaCost = 6;
			Attributes.LowerRegCost = 12;
			ArmorAttributes.MageArmor = 1;
			Resistances.Physical = 6;
			Resistances.Fire = 8;
			Resistances.Cold = 4;
			Resistances.Poison = 5;
			Resistances.Energy = 5;
		}

		public WardedDemonboneBracers( Serial serial )
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
		}
	}
}
