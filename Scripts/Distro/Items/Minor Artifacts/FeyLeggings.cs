using System;
using Server;

namespace Server.Items
{
	public class FeyLeggings : ChainLegs
	{
		public override int LabelNumber { get { return 1075041; } } // Fey Leggings

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public FeyLeggings()
		{
			Attributes.BonusHits = 6;
			Attributes.DefendChance = 20;
			ArmorAttributes.MageArmor = 1;

			Resistances.Physical = 8;
			Resistances.Fire = 4;
			Resistances.Cold = 3;
			Resistances.Poison = 3;
			Resistances.Energy = 17;
		}

		public FeyLeggings( Serial serial )
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
				Resistances.Physical = 8;
				Resistances.Fire = 4;
				Resistances.Cold = 3;
				Resistances.Poison = 3;
				Resistances.Energy = 17;
			}
		}
	}
}