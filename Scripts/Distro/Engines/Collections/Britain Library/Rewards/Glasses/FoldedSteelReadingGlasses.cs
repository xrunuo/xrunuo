using System;
using Server;

namespace Server.Items
{
	public class FoldedSteelReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073380; } } // Folded Steel Reading Glasses

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FoldedSteelReadingGlasses()
		{
			Hue = 1150;
			Attributes.DefendChance = 15;
			Attributes.NightSight = 1;
			Attributes.BonusStr = 8;

			Resistances.Physical = 18;
			Resistances.Fire = 6;
			Resistances.Cold = 7;
			Resistances.Poison = 7;
			Resistances.Energy = 7;
		}

		public FoldedSteelReadingGlasses( Serial serial )
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
				Resistances.Physical = 18;
				Resistances.Fire = 6;
				Resistances.Cold = 7;
				Resistances.Poison = 7;
				Resistances.Energy = 7;
			}
		}
	}
}
