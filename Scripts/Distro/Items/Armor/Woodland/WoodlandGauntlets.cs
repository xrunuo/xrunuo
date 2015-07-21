using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x3161, 0x2B6A )]
	public class WoodlandGauntlets : BaseArmor
	{
		public override int LabelNumber { get { return 1031114; } }

		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 3; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 3; } }
		public override int BaseEnergyResistance { get { return 2; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 65; } }

		public override int StrengthReq { get { return 80; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public WoodlandGauntlets()
			: base( 0x3161 )
		{
			Weight = 5.0;
			Resource = CraftResource.Wood;
			Layer = Layer.Gloves;
		}

		public WoodlandGauntlets( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}