using System;
using Server;

namespace Server.Items
{
	public class CrownofArcaneTemperament : StealableCircletArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113762; } } // Crown of Arcane Temperament

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 14; } }
		public override int BaseColdResistance { get { return 4; } }
		public override int BasePoisonResistance { get { return 12; } }
		public override int BaseEnergyResistance { get { return 7; } }

		public override Race RequiredRace { get { return Race.Human; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public CrownofArcaneTemperament()
		{
			Hue = 0x7DC;
			Weight = 10.0;

			Attributes.BonusMana = 8;
			Attributes.CastingFocus = 2;
			Attributes.RegenMana = 3;
			Attributes.SpellDamage = 8;
			Attributes.LowerManaCost = 6;
		}

		public CrownofArcaneTemperament( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
