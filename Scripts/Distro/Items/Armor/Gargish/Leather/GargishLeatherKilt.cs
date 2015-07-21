using System;
using Server.Items;

namespace Server.Items
{
	public class GargishLeatherKilt : BaseArmor
	{
		public override int LabelNumber { get { return 1095331; } } // gargish leather kilt

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
		public GargishLeatherKilt()
			: this( 0 )
		{
		}

		[Constructable]
		public GargishLeatherKilt( int hue )
			: base( 0x404C )
		{
			Layer = Layer.Gloves;
			Weight = 5.0;
			Hue = hue;
		}

		public GargishLeatherKilt( Serial serial )
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

			if ( Layer != Layer.Gloves )
				Layer = Layer.Gloves;
		}
	}
}