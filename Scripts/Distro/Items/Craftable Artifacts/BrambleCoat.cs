using System;
using Server.Items;

namespace Server.Items
{
	public class BrambleCoat : WoodlandChest
	{
		public override int LabelNumber { get { return 1072925; } } // Bramble Coat

		[Constructable]
		public BrambleCoat()
		{
			Hue = 0x1;

			ArmorAttributes.SelfRepair = 3;
			Attributes.BonusHits = 4;
			Attributes.Luck = 150;
			Attributes.ReflectPhysical = 25;
			Attributes.DefendChance = 15;

			Resistances.Physical = 5;
			Resistances.Fire = 5;
			Resistances.Cold = 5;
			Resistances.Poison = 5;
			Resistances.Energy = 5;
		}

		public BrambleCoat( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			if ( version < 1 )
			{
				Resistances.Physical = 5;
				Resistances.Fire = 5;
				Resistances.Cold = 5;
				Resistances.Poison = 5;
				Resistances.Energy = 5;
			}
		}
	}
}