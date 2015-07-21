using System;
using Server;

namespace Server.Items
{
	public class HeartOfTheLion : PlateChest
	{
		public override int LabelNumber { get { return 1070817; } } // Heart of the Lion

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public HeartOfTheLion()
		{
			Hue = 0x501;
			Attributes.Luck = 95;
			Attributes.DefendChance = 15;
			ArmorAttributes.LowerStatReq = 100;
			ArmorAttributes.MageArmor = 1;

			Resistances.Physical = 10;
			Resistances.Fire = 7;
			Resistances.Cold = 8;
			Resistances.Poison = 7;
			Resistances.Energy = 8;
		}

		public HeartOfTheLion( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Physical = 10;
				Resistances.Fire = 7;
				Resistances.Cold = 8;
				Resistances.Poison = 7;
				Resistances.Energy = 8;
			}
		}
	}
}