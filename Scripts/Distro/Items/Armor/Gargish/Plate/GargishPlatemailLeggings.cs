using System;
using Server.Items;

namespace Server.Items
{
	public class GargishPlatemailLeggings : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 5; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 65; } }

		public override int StrengthReq { get { return 90; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishPlatemailLeggings()
			: base( 0x4056 )
		{
			Layer = Layer.Pants;
			Weight = 7.0;
		}

		public GargishPlatemailLeggings( Serial serial )
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