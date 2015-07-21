using System;
using Server.Items;

namespace Server.Items
{
	public class FemaleGargishClothKilt : BaseClothing
	{
		public override int LabelNumber { get { return 1095355; } } // gargish cloth kilt

		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 7; } }
		public override int BaseColdResistance { get { return 6; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 6; } }

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 40; } }

		public override int StrengthReq { get { return 20; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public FemaleGargishClothKilt()
			: this( 0 )
		{
		}

		[Constructable]
		public FemaleGargishClothKilt( int hue )
			: base( 0x4063, Layer.Gloves )
		{
			Hue = hue;
			Weight = 2.0;
		}

		public FemaleGargishClothKilt( Serial serial )
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