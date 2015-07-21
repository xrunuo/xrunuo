using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class SpeckledPoisonSac : TransientItem
	{
		public override int LabelNumber { get { return 1073133; } } // Speckled Poison Sac

		[Constructable]
		public SpeckledPoisonSac()
			: base( 0x023A, TimeSpan.FromSeconds( 3600.0 ) )
		{
			LootType = LootType.Blessed;
			Weight = 2.0;
		}

		public SpeckledPoisonSac( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}