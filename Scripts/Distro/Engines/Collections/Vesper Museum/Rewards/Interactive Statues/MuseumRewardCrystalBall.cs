using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardCrystalBall : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1073244; } } // Nystul's Crystal Ball - Museum of Vesper Replica

		[Constructable]
		public MuseumRewardCrystalBall()
			: base( 0x0E30 )
		{
			Weight = 10.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardCrystalBall( Serial serial )
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