using System;
using Server;

namespace Server.Items
{
	public class NecromanticReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073377; } } // Necromantic Reading Glasses

		public override int BasePhysicalResistance { get { return 0; } }
		public override int BaseFireResistance { get { return 0; } }
		public override int BaseColdResistance { get { return 0; } }
		public override int BasePoisonResistance { get { return 0; } }
		public override int BaseEnergyResistance { get { return 0; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public NecromanticReadingGlasses()
		{
			Hue = 557;

			Attributes.LowerManaCost = 15;
			Attributes.LowerRegCost = 30;
		}

		public NecromanticReadingGlasses( Serial serial )
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

			/*int version =*/
			reader.ReadInt();
		}
	}
}
