using System;
using Server.Items;

namespace Server.Items
{
	public class FemaleGargishStoneLeggings : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 6; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 4; } }
		public override int BasePoisonResistance { get { return 8; } }
		public override int BaseEnergyResistance { get { return 6; } }

		public override int InitMinHits { get { return 40; } }
		public override int InitMaxHits { get { return 50; } }

		public override int StrengthReq { get { return 40; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public FemaleGargishStoneLeggings()
			: base( 0x405D )
		{
			Layer = Layer.Pants;
			Weight = 15.0;
		}

		public FemaleGargishStoneLeggings( Serial serial )
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