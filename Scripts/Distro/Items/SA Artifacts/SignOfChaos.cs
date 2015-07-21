using System;
using Server;

namespace Server.Items
{
	public class SignOfChaos : ChaosShield
	{
		public override int LabelNumber { get { return 1113535; } } // Sign of Chaos

		public override int BasePhysicalResistance { get { return 3; } }
		public override int BaseFireResistance { get { return 2; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 2; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public SignOfChaos()
		{
			Hue = 0x81B;

			ArmorAttributes.SoulCharge = 20;
			Attributes.AttackChance = 5;
			Attributes.DefendChance = 10;
			Attributes.CastSpeed = 1;
		}

		public SignOfChaos( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}