using System;
using Server;

namespace Server.Items
{
	public class Circlet : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 1; } }
		public override int BaseFireResistance { get { return 5; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 5; } }

		public override int InitMinHits { get { return 45; } }
		public override int InitMaxHits { get { return 60; } }

		public override int StrengthReq { get { return 10; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public override bool Meditable { get { return true; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public Circlet()
			: base( 0x2B6E )
		{
			Weight = 5.0;
			Layer = Layer.Helm;
		}

		public Circlet( Serial serial )
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