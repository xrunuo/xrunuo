using System;
using Server;

namespace Server.Items
{
	public class ReadingGlassesOfTheArts : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073363; } } // Reading Glasses of the Arts

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public ReadingGlassesOfTheArts()
		{
			Hue = 115;

			Attributes.BonusInt = 5;
			Attributes.BonusStr = 5;
			Attributes.BonusHits = 15;

			Resistances.Physical = 8;
			Resistances.Fire = 4;
			Resistances.Cold = 5;
			Resistances.Poison = 1;
			Resistances.Energy = 7;
		}

		public ReadingGlassesOfTheArts( Serial serial )
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
				Resistances.Fire = 4;
				Resistances.Cold = 5;
				Resistances.Poison = 1;
				Resistances.Energy = 7;
			}
		}
	}
}
