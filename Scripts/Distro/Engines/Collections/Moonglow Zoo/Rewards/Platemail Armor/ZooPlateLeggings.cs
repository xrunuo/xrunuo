using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ZooPlateLeggings : PlateLegs, ICollectionItem
	{
		public override int LabelNumber { get { return 1073224; } } // Platemail Armor of the Britannia Royal Zoo

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 10; } }

		[Constructable]
		public ZooPlateLeggings()
		{
			Hue = 265;
			Attributes.RegenStam = 2;
			Attributes.Luck = 100;
			Attributes.DefendChance = 10;
			ArmorAttributes.MageArmor = 1;
		}

		public ZooPlateLeggings( Serial serial )
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