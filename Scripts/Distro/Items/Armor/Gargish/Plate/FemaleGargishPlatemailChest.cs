using System;
using Server.Items;

namespace Server.Items
{
	public class FemaleGargishPlatemailChest : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 5; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 65; } }

		public override int StrengthReq { get { return 95; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public FemaleGargishPlatemailChest()
			: base( 0x4051 )
		{
			Layer = Layer.InnerTorso;
			Weight = 10.0;
		}

		public FemaleGargishPlatemailChest( Serial serial )
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