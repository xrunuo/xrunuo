using System;
using Server;

namespace Server.Items
{
	public class MaceAndShieldReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073381; } } // Mace And Shield Reading Glasses

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public MaceAndShieldReadingGlasses()
		{
			Hue = 477;

			Attributes.BonusStr = 10;
			Attributes.BonusDex = 5;
			HitLowerDefend = 30;

			Resistances.Physical = 23;
			Resistances.Fire = 6;
			Resistances.Cold = 7;
			Resistances.Poison = 7;
			Resistances.Energy = 7;
		}

		public MaceAndShieldReadingGlasses( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Physical = 23;
				Resistances.Fire = 6;
				Resistances.Cold = 7;
				Resistances.Poison = 7;
				Resistances.Energy = 7;
			}
		}
	}
}
