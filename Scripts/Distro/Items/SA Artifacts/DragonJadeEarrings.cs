using System;
using Server;

namespace Server.Items
{
	[TypeAlias( "Server.Items.DragonJadeEarringsArmor" )]
	public class DragonJadeEarrings : GargishEarrings
	{
		public override int LabelNumber { get { return 1113720; } } // Dragon Jade Earrings

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DragonJadeEarrings()
		{
			Hue = 683;

			AbsorptionAttributes.FireEater = 10;
			Attributes.BonusStr = 5;
			Attributes.BonusDex = 5;
			Attributes.RegenHits = 2;
			Attributes.RegenStam = 3;
			Attributes.LowerManaCost = 5;
			Resistances.Physical = 8;
			Resistances.Fire = 14;
			Resistances.Cold = 3;
			Resistances.Poison = 11;
		}

		public DragonJadeEarrings( Serial serial )
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
