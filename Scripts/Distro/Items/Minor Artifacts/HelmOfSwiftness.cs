using System;
using Server;

namespace Server.Items
{
	public class HelmOfSwiftness : BaseArmor
	{
		public override int LabelNumber { get { return 1075037; } } // Helm of Swiftness

		public override int BasePhysicalResistance { get { return 6; } }
		public override int BaseFireResistance { get { return 5; } }
		public override int BaseColdResistance { get { return 6; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 8; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int StrengthReq { get { return 45; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public HelmOfSwiftness()
			: base( 0x268A )
		{
			Layer = Layer.Helm;
			Weight = 5.0;
			Hue = 1426;

			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 2;
			Attributes.BonusInt = 5;
			ArmorAttributes.MageArmor = 1;
		}

		public HelmOfSwiftness( Serial serial )
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