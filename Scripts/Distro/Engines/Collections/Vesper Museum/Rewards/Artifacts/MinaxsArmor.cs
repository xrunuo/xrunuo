using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class MinaxsArmor : FemaleStuddedChest, ICollectionItem
	{
		public override int LabelNumber { get { return 1073257; } } // Minax's Armor - Museum of Vesper Replica

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override int BasePhysicalResistance { get { return 15; } }
		public override int BaseFireResistance { get { return 5; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 15; } }
		public override int BaseEnergyResistance { get { return 25; } }

		[Constructable]
		public MinaxsArmor()
		{
			Hue = 1107;
			Attributes.RegenMana = 2;
			ArmorAttributes.MageArmor = 1;
		}

		public MinaxsArmor( Serial serial )
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