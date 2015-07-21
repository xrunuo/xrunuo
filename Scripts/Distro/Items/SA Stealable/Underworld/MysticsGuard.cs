using System;
using Server;

namespace Server.Items
{
	public class MysticsGuard : StealableSmallPlateShieldArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113536; } } // Mystic's Guard

		public override int ItemID { get { return 0x4200; } }

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 0; } }
		public override int BaseColdResistance { get { return 0; } }
		public override int BasePoisonResistance { get { return 0; } }
		public override int BaseEnergyResistance { get { return 1; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public MysticsGuard()
		{
			Hue = 0x671;
			Weight = 10.0;

			ArmorAttributes.SoulCharge = 30;
			Attributes.CastRecovery = 2;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.DefendChance = 10;
		}

		public MysticsGuard( Serial serial )
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
