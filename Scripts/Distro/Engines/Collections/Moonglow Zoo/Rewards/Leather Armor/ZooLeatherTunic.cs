using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ZooLeatherTunic : LeatherChest, ICollectionItem
	{
		public override int LabelNumber { get { return 1073222; } } // Leather Armor of the Britannia Royal Zoo

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 10; } }

		[Constructable]
		public ZooLeatherTunic()
		{
			Hue = 265;
			Attributes.BonusMana = 3;
			Attributes.RegenStam = 3;
			Attributes.ReflectPhysical = 10;
			Attributes.LowerRegCost = 15;
		}

		public ZooLeatherTunic( Serial serial )
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