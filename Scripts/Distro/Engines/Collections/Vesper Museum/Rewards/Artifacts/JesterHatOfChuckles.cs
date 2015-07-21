using System;
using Server;

namespace Server.Items
{
	public class JesterHatOfChuckles : JesterHat, ICollectionItem
	{
		public override int LabelNumber { get { return 1073256; } } // Jester Hat of Chuckles - Museum of Vesper Replica

		public override int InitMinHits { get { return 100; } }
		public override int InitMaxHits { get { return 100; } }

		public override int BasePhysicalResistance { get { return 12; } }
		public override int BaseFireResistance { get { return 12; } }
		public override int BaseColdResistance { get { return 12; } }
		public override int BasePoisonResistance { get { return 12; } }
		public override int BaseEnergyResistance { get { return 12; } }

		[Constructable]
		public JesterHatOfChuckles()
		{
			Hue = 641;
			Attributes.Luck = 150;
		}

		public JesterHatOfChuckles( Serial serial )
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