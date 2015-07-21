using System;
using Server;

namespace Server.Items
{
	public class GypsyHeaddress : SkullCap, ICollectionItem
	{
		public override int LabelNumber { get { return 1073254; } } // Gypsy Headdress - Museum of Vesper Replica

		public override int InitMinHits { get { return 100; } }
		public override int InitMaxHits { get { return 100; } }

		public override int BasePhysicalResistance { get { return 15; } }
		public override int BaseFireResistance { get { return 20; } }
		public override int BaseColdResistance { get { return 20; } }
		public override int BasePoisonResistance { get { return 15; } }
		public override int BaseEnergyResistance { get { return 15; } }

		[Constructable]
		public GypsyHeaddress()
		{
			Hue = 641;
		}

		public GypsyHeaddress( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}