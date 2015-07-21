using System;
using Server;

namespace Server.Items
{
	public class Lavaliere : GoldNecklace
	{
		public override int LabelNumber { get { return 1114843; } } // Lavaliere

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public Lavaliere()
		{
			Hue = 1194;

			AbsorptionAttributes.KineticEater = 20;
			Attributes.DefendChance = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 20;
			Resistances.Physical = 15;
		}

		public Lavaliere( Serial serial )
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
