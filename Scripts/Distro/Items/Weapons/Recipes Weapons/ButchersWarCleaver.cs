using System;
using Server;

namespace Server.Items
{
	public class ButchersWarCleaver : WarCleaver
	{
		public override int LabelNumber { get { return 1073526; } } // Butcher's War Cleaver

		[Constructable]
		public ButchersWarCleaver()
		{
			Slayer3 = TalisSlayerName.Bovine;
		}

		public ButchersWarCleaver( Serial serial )
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

			/*int version = */reader.ReadInt();

			if ( Slayer3 != TalisSlayerName.Bovine )
				Slayer3 = TalisSlayerName.Bovine;
		}
	}
}