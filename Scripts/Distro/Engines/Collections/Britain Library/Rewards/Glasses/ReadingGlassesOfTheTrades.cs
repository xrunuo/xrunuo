using System;
using Server;

namespace Server.Items
{
	public class ReadingGlassesOfTheTrades : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073362; } } // Reading Glasses of the Trades

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public ReadingGlassesOfTheTrades()
		{
			Hue = 0;
			Attributes.BonusInt = 10;
			Attributes.BonusStr = 10;

			Resistances.Physical = 8;
			Resistances.Fire = 6;
			Resistances.Cold = 7;
			Resistances.Poison = 7;
			Resistances.Energy = 7;
		}

		public ReadingGlassesOfTheTrades( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Physical = 8;
				Resistances.Fire = 6;
				Resistances.Cold = 7;
				Resistances.Poison = 7;
				Resistances.Energy = 7;
			}
		}
	}
}
