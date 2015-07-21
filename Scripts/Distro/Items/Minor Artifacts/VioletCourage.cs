using System;
using Server;

namespace Server.Items
{
	public class VioletCourage : FemalePlateChest
	{
		public override int LabelNumber { get { return 1063471; } } // Violet Courage

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public VioletCourage()
		{
			Hue = Utility.RandomBool() ? 0x486 : 1168;

			Attributes.Luck = 95;
			Attributes.DefendChance = 15;
			ArmorAttributes.LowerStatReq = 100;
			ArmorAttributes.MageArmor = 1;

			Resistances.Physical = 9;
			Resistances.Fire = 9;
			Resistances.Cold = 10;
			Resistances.Poison = 5;
			Resistances.Energy = 7;
		}

		public VioletCourage( Serial serial )
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
				Resistances.Physical = 9;
				Resistances.Fire = 9;
				Resistances.Cold = 10;
				Resistances.Poison = 5;
				Resistances.Energy = 7;
			}
		}
	}
}