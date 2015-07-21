using System;
using Server;
using Server.Engines.Quests;
using Reward = Server.Engines.Quests.BaseReward;

namespace Server.Items
{
	public class RewardBox : WoodenBox
	{
		public int ItemAmount { get { return 6; } }

		public int MinIntensity { get { return 50; } }
		public int MaxIntensity { get { return 100; } }

		public int MinProperties { get { return 4; } }
		public int MaxProperties { get { return 6; } }

		[Constructable]
		public RewardBox()
		{
			Hue = Reward.StrongboxHue();

			for ( int i = 0; i < ItemAmount; i++ )
				DropItem( Reward.RandomItem( Utility.RandomMinMax( MinProperties, MaxProperties ), MinIntensity, MaxIntensity ) );

			if ( 0.25 > Utility.RandomDouble() ) // check
				DropItem( new RandomTalisman() );
		}

		public RewardBox( Serial serial )
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

			int version = reader.ReadInt();
		}
	}
}