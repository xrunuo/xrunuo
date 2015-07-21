using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests.HumilityCloak;

namespace Server.Engines.Collections
{
	public class MoonglowZooCollection : CollectionController
	{
		public override int PointsPerTier { get { return 1000000 * ( Tier + 1 ); } }
		public override int MaxTiers { get { return 11; } }

		public override IRewardEntry[] Rewards { get { return RewardList; } }
		public override DonationEntry[] Donations { get { return DonationList; } }

		private static readonly int[] ClothHues = new int[]
			{
				1365, 174, 148, 632,
				50, 40, 807, 1050
			};

		private static readonly int[] StatueHues = new int[]
			{
				52, 450, 675
			};

		private static readonly Rectangle2D[] m_Locations = new Rectangle2D[]
			{
				// Tier 1 - Crane
				new Rectangle2D( new Point2D( 4498, 1381 ), new Point2D( 4503, 1384 ) ),
				
				// Tier 2 - Wolves
				new Rectangle2D( new Point2D( 4488, 1354 ), new Point2D( 4499, 1374 ) ),

				// Tier 3 - Polar Bears
				new Rectangle2D( new Point2D( 4506, 1390 ), new Point2D( 4517, 1398 ) ),

				// Tier 4 - Quagmire
				new Rectangle2D( new Point2D( 4480, 1376 ), new Point2D( 4491, 1398 ) ),

				// Tier 5 - Wyvern
				new Rectangle2D( new Point2D( 4506, 1377 ), new Point2D( 4518, 1384 ) ),

				// Tier 6 - Dragons
				new Rectangle2D( new Point2D( 4506, 1369 ), new Point2D( 4518, 1375 ) ),

				// Tier 7 - Changeling
				new Rectangle2D( new Point2D( 4514, 1354 ), new Point2D( 4525, 1362 ) ),

				// Tier 8 - Reptalon
				new Rectangle2D( new Point2D( 4525, 1383 ), new Point2D( 4535, 1391 ) ),

				// Tier 9 - Yamandon
				new Rectangle2D( new Point2D( 4493, 1390 ), new Point2D( 4504, 1398 ) ),

				// Tier 10 - Sphynx
				new Rectangle2D( new Point2D( 4527, 1354 ), new Point2D( 4535, 1373 ) ),

				// Tier 11 - Silver Steed
				new Rectangle2D( new Point2D( 4501, 1354 ), new Point2D( 4512, 1362 ) )
			};

		public static readonly RewardEntry[] RewardList = new RewardEntry[]
			{
				new ConditionalRewardEntry( typeof( ForTheLifeOfBritanniaSash ), 5000, 1075792, 0x1541, 0xB7, BaseCommunityServiceQuest.GetReward<CommunityServiceZooQuest>, BaseCommunityServiceQuest.RewardAvailableFor<CommunityServiceZooQuest> ),

				// page 1
				new RewardEntry( typeof( ZooRewardCloak ), 100000, 1073221, ClothHues, 5397, 1365 ),
				new RewardEntry( typeof( ZooRewardRobe ), 100000, 1073221, ClothHues, 7940, 1365 ),
				new RewardEntry( typeof( ZooRewardPlainDress ), 100000, 1073221, ClothHues, 7937, 1365 ),
				new RewardEntry( typeof( ZooRewardBodySash ), 100000, 1073221, ClothHues, 5441, 1365 ),

				// page 2
				new RewardEntry( typeof( ZooRewardThighBoots ), 100000, 1073221, ClothHues, 5906, 1365 ),
				new RewardEntry( typeof( ZooRewardFloppyHat ), 100000, 1073221, ClothHues, 5907, 1365 ),
				new RewardEntry( typeof( ZooRewardBonnet ), 100000, 1073221, ClothHues, 5913, 1365 ),
				new RewardEntry( typeof( RewardTitle ), 100000, 1073624, 1073201, 4079, 0 ),
				new RewardEntry( typeof( ZooRewardQuagmireStatue ), 200000, 1073195, 9748, 0 ),
				new RewardEntry( typeof( ZooRewardBakeKitsuneStatue ), 200000, 1073189, 10083, 0 ),

				// page 3
				new RewardEntry( typeof( ZooRewardWolfStatue ), 200000, 1073190, 8482, 0 ),
				new RewardEntry( typeof( ZooRewardChangelingStatue ), 200000, 1073191, 11658, 0 ),
				new RewardEntry( typeof( ZooRewardReptalonStatue ), 200000, 1073192, 11669, 0 ),
				new RewardEntry( typeof( ZooRewardPolarBearStatue ), 200000, 1073193, 8417, 0 ),

				// page 4
				new RewardEntry( typeof( ZooRewardSnakeStatue ), 200000, 1073194, 8444, 450 ),
				new RewardEntry( typeof( RewardTitle ), 200000, 1073627, 1073202, 4079, 0 ),
				new RewardEntry( typeof( ZooRewardSilverSteedIntStatue ), 350000, 1073219, 9629, 0 ),
				new RewardEntry( typeof( ZooRewardQuagmireIntStatue ), 350000, 1074848, StatueHues, 9748, 52 ),
				new RewardEntry( typeof( ZooRewardBakeKitsuneIntStatue ), 350000, 1074849, StatueHues, 10083, 52 ),

				// page 5
				new RewardEntry( typeof( ZooRewardDireWolfIntStatue ), 350000, 1073196, StatueHues, 9680, 52 ),
				new RewardEntry( typeof( ZooRewardCraneIntStatue ), 350000, 1073197, new int[] { 0, 52, 450, 675 }, 10084, 0 ),
				new RewardEntry( typeof( ZooRewardPolarBearIntStatue ), 350000, 1074851, StatueHues, 8417, 52 ),
				new RewardEntry( typeof( ZooRewardChangelingIntStatue ), 350000, 1074850, StatueHues, 11658, 52 ),

				// page 6
				new RewardEntry( typeof( ZooRewardReptalonIntStatue ), 350000, 1074852, StatueHues, 11669, 52 ),
				new RewardEntry( typeof( RewardTitle ), 350000, 1073628, 1073203, 4079, 0 ),
				new RewardEntry( typeof( ZooLeatherLeggings ), 550000, 1073222, 5067, 265 ),
				new RewardEntry( typeof( ZooLeatherGloves ), 550000, 1073222, 5062, 265 ),
				new RewardEntry( typeof( ZooLeatherGorget ), 550000, 1073222, 5063, 265 ),

				// page 7
				new RewardEntry( typeof( ZooLeatherSleeves ), 550000, 1073222, 5061, 265 ),
				new RewardEntry( typeof( ZooLeatherTunic ), 550000, 1073222, 5068, 265 ),
				new RewardEntry( typeof( ZooLeatherTunicFemale ), 550000, 1073222, 7174, 265 ),
				new RewardEntry( typeof( ZooStuddedTunic ), 550000, 1073223, 5083, 265 ),
				new RewardEntry( typeof( ZooStuddedSleeves ), 550000, 1073223, 5076, 265 ),

				// page 8
				new RewardEntry( typeof( ZooStuddedGorget ), 550000, 1073223, 5078, 265 ),
				new RewardEntry( typeof( ZooStuddedGloves ), 550000, 1073223, 5077, 265 ),
				new RewardEntry( typeof( ZooStuddedLeggings ), 550000, 1073223, 5089, 265 ),
				new RewardEntry( typeof( ZooStuddedTunicFemale ), 550000, 1073223, 7170, 265 ),
				new RewardEntry( typeof( ZooPlateTunic ), 550000, 1073224, 5141, 265 ),

				// page 9
				new RewardEntry( typeof( ZooPlateSleeves ), 550000, 1073224, 5136, 265 ),
				new RewardEntry( typeof( ZooPlateGorget ), 550000, 1073224, 5139, 265 ),
				new RewardEntry( typeof( ZooPlateGloves ), 550000, 1073224, 5140, 265 ),
				new RewardEntry( typeof( ZooPlateLeggings ), 550000, 1073224, 5137, 265 ),
				new RewardEntry( typeof( ZooPlateHelm ), 550000, 1073224, 5145, 265 ),

				// page 10
				new RewardEntry( typeof( ZooPlateTunicFemale ), 550000, 1073224, 7174, 265 ),
				new RewardEntry( typeof( RewardTitle ), 550000, 1073629, 1073204, 4079, 0 ),
				new RewardEntry( typeof( ZooSpecialAchievementAward ), 800000, 1073226, 12278, 0 ),
				new RewardEntry( typeof( RewardTitle ), 800000, 1073630, 1073205, 4079, 0 ),
				new RewardEntry( typeof( RewardTitle ), 800000, 1073631, 1073206, 4079, 0 ),
			};

		private static readonly DonationEntry[] DonationList = new DonationEntry[]
			{
				/*
				 * TODO:
				 * 
				 * 30: Grey Wolf, Timber Wolf, White Wolf
				 * 250: Ki-Rin, Nightmare, Unicorn
				 * 300: Fire Beetle, Giant Beetle, Rune Beetle
				 * 400: Dragon, Frake, White Wyrm
				 */

				// page 1
				new DonationEntry( typeof( Gold ), 1.0 / 15.0, 1073116, 3823, 0 ),
				new DonationEntry( typeof( BankCheck ), 1.0 / 15.0, 1075013, 5360, 52 ),
				new DonationEntry( typeof( TimberWolf ), 30, 1073118, 8482, 0 ),
				new DonationEntry( typeof( Slime ), 45, 1073117, 4650, 2204 ),
				
				// page 2
				new DonationEntry( typeof( PolarBear ), 45, 1073120, 8417, 0 ),
				new DonationEntry( typeof( Unicorn ), 250, 10748212, 9678, 0 ),
				new DonationEntry( typeof( RuneBeetle ), 300, 1074820, 10095, 0 ),
				new DonationEntry( typeof( Dragon ), 400, 1073119,8406, 0 ),

				// page 3
				new DonationEntry( typeof( Reptalon ), 550, 1073121, 11669, 0 )
			};

		[Constructable]
		public MoonglowZooCollection()
		{
		}

		public override void OnTierAdvanced()
		{
			foreach ( Item item in Map.GetItemsInBounds( m_Locations[Tier - 1] ) )
			{
				if ( item is CreatureSpawner && !( (CreatureSpawner) item ).Active )
					( (CreatureSpawner) item ).Active = true;
			}
		}

		public override void OnTierDecreased()
		{
			foreach ( Item item in Map.GetItemsInBounds( m_Locations[Tier] ) )
			{
				if ( item is CreatureSpawner && ( (CreatureSpawner) item ).Active )
					( (CreatureSpawner) item ).Active = false;
			}
		}

		public MoonglowZooCollection( Serial serial )
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

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				for ( int i = 0; i < 16; i++ )
					reader.ReadItem();
			}
		}
	}
}