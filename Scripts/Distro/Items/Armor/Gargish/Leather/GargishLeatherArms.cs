using System;
using Server.Items;

namespace Server.Items
{
	public class GargishLeatherArms : BaseArmor
	{
		public override int LabelNumber { get { return 1095327; } } // gargish leather arms

		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 8; } }
		public override int BaseColdResistance { get { return 6; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 7; } }

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 50; } }

		public override int StrengthReq { get { return 25; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishLeatherArms()
			: this( 0 )
		{
		}

		[Constructable]
		public GargishLeatherArms( int hue )
			: base( 0x4048 )
		{
			Layer = Layer.Arms;
			Weight = 4.0;
			Hue = hue;
		}

		public GargishLeatherArms( Serial serial )
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