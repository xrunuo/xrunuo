using System;
using Server.Items;

namespace Server.Items
{
	public class FemaleGargishClothChest : BaseClothing
	{
		public override int LabelNumber { get { return 1095353; } } // gargish cloth chest

		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 7; } }
		public override int BaseColdResistance { get { return 6; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 6; } }

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 40; } }

		public override int StrengthReq { get { return 25; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public FemaleGargishClothChest()
			: this( 0 )
		{
			Weight = 6.0;
		}

		[Constructable]
		public FemaleGargishClothChest( int hue )
			: base( 0x4061, Layer.InnerTorso )
		{
			Hue = hue;
			Weight = 6.0;
		}

		public FemaleGargishClothChest( Serial serial )
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