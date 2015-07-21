using System;
using Server;

namespace Server.Items
{
	public class WizardsCrystalReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073374; } } // Wizard's Crystal Reading Glasses

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public WizardsCrystalReadingGlasses()
		{
			Hue = 557;
			Attributes.BonusMana = 10;
			Attributes.RegenMana = 3;
			Attributes.SpellDamage = 15;

			Resistances.Physical = 3;
			Resistances.Fire = 1;
			Resistances.Cold = 2;
			Resistances.Poison = 2;
			Resistances.Energy = 2;
		}

		public WizardsCrystalReadingGlasses( Serial serial )
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
				Resistances.Cold = 2;
				Resistances.Poison = 2;
				Resistances.Energy = 2;
			}
		}
	}
}
