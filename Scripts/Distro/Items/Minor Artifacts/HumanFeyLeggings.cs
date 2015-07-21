using System;
using Server;

namespace Server.Items
{
	public class HumanFeyLeggings : ChainLegs
	{
		public override int LabelNumber { get { return 1075041; } } // Fey Leggings

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public HumanFeyLeggings()
		{
			Attributes.BonusHits = 6;
			Attributes.DefendChance = 20;
			ArmorAttributes.MageArmor = 1;

			Resistances.Physical = 10;
			Resistances.Fire = 5;
			Resistances.Cold = 5;
			Resistances.Energy = 15;
		}

		public HumanFeyLeggings( Serial serial )
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
		}
	}
}