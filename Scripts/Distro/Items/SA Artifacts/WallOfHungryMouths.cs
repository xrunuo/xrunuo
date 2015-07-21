using System;
using Server;

namespace Server.Items
{
	public class WallOfHungryMouths : HeaterShield
	{
		public override int LabelNumber { get { return 1113722; } } // Wall of Hungry Mouths

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public WallOfHungryMouths()
		{
			Hue = 1034;

			AbsorptionAttributes.EnergyEater = 20;
			AbsorptionAttributes.PoisonEater = 20;
			AbsorptionAttributes.ColdEater = 20;
			AbsorptionAttributes.FireEater = 20;
			Resistances.Physical = 5;
		}

		public WallOfHungryMouths( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}