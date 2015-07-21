using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x3163, 0x2B6C )]
	public class WoodlandArms : BaseArmor
	{
		public override int LabelNumber { get { return 1031116; } }
		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 3; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 3; } }
		public override int BaseEnergyResistance { get { return 2; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 65; } }

		public override int StrengthReq { get { return 80; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Wood; } }
		public override CraftResource DefaultResource { get { return CraftResource.Wood; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public WoodlandArms()
			: base( 0x3163 )
		{
			Weight = 5.0;
			Layer = Layer.Arms;
		}

		public WoodlandArms( Serial serial )
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