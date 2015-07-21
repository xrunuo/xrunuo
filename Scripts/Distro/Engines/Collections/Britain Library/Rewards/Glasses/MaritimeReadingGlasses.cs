using System;
using Server;

namespace Server.Items
{
	public class MaritimeReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073364; } } // Maritime Reading Glasses

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public MaritimeReadingGlasses()
		{
			Hue = 1409;

			Attributes.Luck = 150;
			Attributes.NightSight = 1;
			Attributes.ReflectPhysical = 20;

			Resistances.Physical = 1;
			Resistances.Cold = 27;
			Resistances.Poison = 2;
		}

		public MaritimeReadingGlasses( Serial serial )
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
				Resistances.Physical = 1;
				Resistances.Cold = 27;
				Resistances.Poison = 2;
			}
		}
	}
}
