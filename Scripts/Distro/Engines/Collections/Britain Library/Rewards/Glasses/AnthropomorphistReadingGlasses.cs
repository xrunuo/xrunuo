using System;
using Server;

namespace Server.Items
{
	public class AnthropomorphistReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073379; } } // Anthropomorphist Reading Glasses

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public AnthropomorphistReadingGlasses()
		{
			Hue = 128;
			Attributes.RegenMana = 3;
			Attributes.BonusHits = 5;

			Resistances.Physical = 3;
			Resistances.Fire = 1;
			Resistances.Cold = 7;
			Resistances.Poison = 17;
			Resistances.Energy = 17;
		}

		public AnthropomorphistReadingGlasses( Serial serial )
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
				Resistances.Physical = 3;
				Resistances.Fire = 1;
				Resistances.Cold = 7;
				Resistances.Poison = 17;
				Resistances.Energy = 17;
			}
		}
	}
}
