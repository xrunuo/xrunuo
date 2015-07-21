using System;
using Server;

namespace Server.Items
{
	public class KeeoneanChainMail : ChainChest, ICollectionItem
	{
		public override int LabelNumber { get { return 1073264; } } // Keeonean's Chain Mail - Museum of Vesper Replica

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 20; } }
		public override int BaseColdResistance { get { return 15; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 15; } }

		[Constructable]
		public KeeoneanChainMail()
		{
			Hue = 2126;
			Attributes.NightSight = 1;
			Attributes.RegenHits = 3;
			ArmorAttributes.MageArmor = 1;
		}

		public KeeoneanChainMail( Serial serial )
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