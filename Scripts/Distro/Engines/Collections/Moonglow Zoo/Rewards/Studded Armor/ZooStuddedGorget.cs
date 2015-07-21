using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ZooStuddedGorget : StuddedGorget, ICollectionItem
	{
		public override int LabelNumber { get { return 1073223; } } // Studded Armor of the Britannia Royal Zoo

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 10; } }

		[Constructable]
		public ZooStuddedGorget()
		{
			Hue = 265;
			Attributes.BonusMana = 3;
			Attributes.BonusHits = 2;
			Attributes.LowerManaCost = 10;
			ArmorAttributes.MageArmor = 1;
		}

		public ZooStuddedGorget( Serial serial )
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