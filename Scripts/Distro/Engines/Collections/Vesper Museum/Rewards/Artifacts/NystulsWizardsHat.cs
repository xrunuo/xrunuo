using System;
using Server;

namespace Server.Items
{
	public class NystulsWizardsHat : WizardsHat, ICollectionItem
	{
		public override int LabelNumber { get { return 1073255; } } // Nystul's Wizard's Hat - Museum of Vesper Replica

		public override int InitMinHits { get { return 100; } }
		public override int InitMaxHits { get { return 100; } }

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 25; } }

		[Constructable]
		public NystulsWizardsHat()
		{
			Hue = 641;
			Attributes.LowerManaCost = 15;
		}

		public NystulsWizardsHat( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}