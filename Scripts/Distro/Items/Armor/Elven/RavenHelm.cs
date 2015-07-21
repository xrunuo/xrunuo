using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2B71, 0X3168 )]
	public class RavenHelm : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 1; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 5; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 35; } }

		public override int StrengthReq { get { return 25; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Wood; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public RavenHelm()
			: base( 0x2B71 )
		{
			Weight = 5.0;
			Layer = Layer.Helm;
		}

		public RavenHelm( Serial serial )
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

			if ( Weight == 1.0 )
				Weight = 5.0;
		}
	}
}