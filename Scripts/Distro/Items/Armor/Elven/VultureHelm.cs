using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x2B72, 0X3169 )]
	public class VultureHelm : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 1; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 5; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 35; } }

		public override int StrengthReq { get { return 25; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public VultureHelm()
			: base( 0x2B72 )
		{
			Weight = 5.0;
			Resource = CraftResource.Wood;
			Layer = Layer.Helm;
		}

		public VultureHelm( Serial serial )
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