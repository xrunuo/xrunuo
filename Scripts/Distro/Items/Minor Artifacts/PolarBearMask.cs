using System;
using Server;

namespace Server.Items
{
	public class PolarBearMask : BearMask
	{
		public override int LabelNumber { get { return 1070637; } } // Polar Bear Mask

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public PolarBearMask()
		{
			Hue = 0x481;

			Resistances.Physical = 10;
			Resistances.Cold = 13;

			ClothingAttributes.SelfRepair = 3;

			Attributes.RegenHits = 2;
			Attributes.NightSight = 1;
		}

		public PolarBearMask( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Physical = 10;
				Resistances.Cold = 13;
			}

			if ( Attributes.NightSight == 0 )
			{
				Attributes.NightSight = 1;
			}
		}
	}
}
