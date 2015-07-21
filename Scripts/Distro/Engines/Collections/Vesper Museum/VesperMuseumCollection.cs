using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests.HumilityCloak;

namespace Server.Engines.Collections
{
	public class VesperMuseumCollection : CollectionController
	{
		private static readonly int[] m_Points = new int[]
			{
				// p(t) = (10 si t=0 // p(t-1)+5(t+1) si t!=0)

				10000000, 20000000, 35000000, // 0, 1, 2
				55000000, 80000000, 110000000, // 3, 4, 5
				145000000, 185000000, 230000000, // 6, 7, 8
				280000000, 335000000, 395000000, 0 // 9, 10, 11, 12
			};

		public override int PointsPerTier { get { return m_Points[Tier]; } }
		public override int MaxTiers { get { return 12; } }

		public override IRewardEntry[] Rewards { get { return RewardList; } }
		public override DonationEntry[] Donations { get { return DonationList; } }

		public override int LabelNumber { get { return 1073407; } } // Please Contribute to the public Museum of Vesper.

		private static readonly int[] JewleryHues = new int[]
			{
				42, 637, 247, 566
			};

		private static readonly int[] ClothHues = new int[]
			{
				641, 371, 1409, 768
			};

		private static readonly Rectangle2D[] m_Locations = new Rectangle2D[]
			{
				// Tier 1 - The Necromantic Arts
				new Rectangle2D( new Point2D( 2919, 970 ), new Point2D( 2924, 973 ) ),

				// Tier 2 - The Paladin
				new Rectangle2D( new Point2D( 2923, 977 ), new Point2D( 2925, 981 ) ),

				// Tier 3 - Flora and Fauna of Faraway Lands
				new Rectangle2D( new Point2D( 2913, 981 ), new Point2D( 2917, 985 ) ),

				// Tier 4 - May the Cards Guide Thee
				new Rectangle2D( new Point2D( 2910, 983 ), new Point2D( 2912, 985 ) ),

				// Tier 5 - Swords and Silk
				new Rectangle2D( new Point2D( 2908, 962 ), new Point2D( 2911, 967 ) ),

				// Tier 6 - Whimsical Momentos
				new Rectangle2D( new Point2D( 2918, 983 ), new Point2D( 2920, 985 ) ),

				// Tier 7 - Nature's Children
				new Rectangle2D( new Point2D( 2912, 963 ), new Point2D( 2915, 967 ) ),

				// Tier 8 - Of Chaos
				new Rectangle2D( new Point2D( 2904, 978 ), new Point2D( 2908, 983 ) ),

				// Tier 9 - There Once ruled a Great King over These Lands
				new Rectangle2D( new Point2D( 2903, 967 ), new Point2D( 2909, 972 ) ),

				// Tier 10 - Notable Villains
				new Rectangle2D( new Point2D( 2912, 973 ), new Point2D( 2914, 977 ) ),

				// Tier 11 - Of Order
				new Rectangle2D( new Point2D( 2903, 973 ), new Point2D( 2907, 977 ) ),

				// Tier 12 - Notable Events
				new Rectangle2D( new Point2D( 2915, 973 ), new Point2D( 2917, 977 ) ),
			};

		public static readonly RewardEntry[] RewardList = new RewardEntry[]
			{
				new ConditionalRewardEntry( typeof( ShepherdsCrookOfHumility ), 5000, 1075856, 0xE81, 0x386, BaseCommunityServiceQuest.GetReward<CommunityServiceMuseumQuest>, BaseCommunityServiceQuest.RewardAvailableFor<CommunityServiceMuseumQuest> ),

				// page 1
				new RewardEntry( typeof( MuseumRewardRobe ), 100000, 1073250, new int[] { 1409, 632, 792, 767 }, 7939, 1409 ),
				new RewardEntry( typeof( MuseumRewardPlainDress ), 100000, 1073251, new int[] { 553, 398, 533, 245 }, 7937, 553 ),
				new RewardEntry( typeof( MuseumRewardCloak ), 100000, 1073252, ClothHues, 5424, 641 ),
				new RewardEntry( typeof( MuseumRewardBodySash ), 100000, 1073253, ClothHues, 5441, 641 ),
				new RewardEntry( typeof( RewardTitle ), 100000, 1073637, 1073235, 4079, 0 ),

				// page 2
				new RewardEntry( typeof( MuseumRewardRing ), 200000, 1073234, JewleryHues, 4234, 42 ),
				new RewardEntry( typeof( MuseumRewardNecklace ), 200000, 1073234, JewleryHues, 4232, 42 ),
				new RewardEntry( typeof( MuseumRewardBracelet ), 200000, 1073234, JewleryHues, 4230, 42 ),
				new RewardEntry( typeof( MuseumRewardEarrings ), 200000, 1073234, JewleryHues, 4231, 42 ),
				new RewardEntry( typeof( RewardTitle ), 200000, 1073638, 1073236, 4079, 0 ),
				new RewardEntry( typeof( MuseumRewardLordBritishsThroneDeed ), 350000, 1073243, 5359, 0 ),
				new RewardEntry( typeof( MuseumRewardTrollStatue ), 350000, 1073242, 8425, 0 ),
				
				// page 3
				new RewardEntry( typeof( MuseumRewardCrystalBall ), 350000, 1073244, 3632, 0 ),
				new RewardEntry( typeof( MuseumRewardDevourerStatue ), 350000, 1073245, 9763, 0 ),
				new RewardEntry( typeof( MuseumRewardLadyOfTheSnowStatue ), 350000, 1075016, 10092, 0 ),
				new RewardEntry( typeof( MuseumRewardGolemStatue ), 350000, 1075017, 9744, 0 ),

				// page 4
				new RewardEntry( typeof( MuseumRewardExodusStatue ), 350000, 1075018, 9740, 0 ),
				new RewardEntry( typeof( MuseumRewardJukaLordStatue ), 350000, 1075019, 9724, 0 ),
				new RewardEntry( typeof( MuseumRewardMeerCaptainStatue ), 350000, 1075020, 9722, 0 ),
				new RewardEntry( typeof( MuseumRewardMeerEternalStatue ), 350000, 1075021, 9720, 0 ),

				// page 5
				new RewardEntry( typeof( MuseumRewardSolenQueenStatue ), 350000, 1075022, 9730, 0 ),
				new RewardEntry( typeof( RewardTitle ), 350000, 1073639, 1073237, 4079, 0 ),
				new RewardEntry( typeof( MinaxsArmor ), 550000, 1073257, 7170, 1107 ),
				new RewardEntry( typeof( GypsyHeaddress ), 550000, 1073254, ClothHues, 5444, 641 ),
				new RewardEntry( typeof( NystulsWizardsHat ), 550000, 1073255, ClothHues, 5912, 641 ),
				new RewardEntry( typeof( JesterHatOfChuckles ), 550000, 1073256, ClothHues, 5916, 641 ),

				// page 6
				new RewardEntry( typeof( KeeoneanChainMail ), 550000, 1073264, 5055, 2126 ),
				new RewardEntry( typeof( ClaininsSpellbook ), 800000, 1073262, 3834, 2125 ),
				new RewardEntry( typeof( RewardTitle ), 550000, 1073640, 1073238, 4079, 0 ),
				new RewardEntry( typeof( MuseumOrderShield ), 800000, 1073258, 7108, 1000 ),
				new RewardEntry( typeof( MuseumChaosShield ), 800000, 1073259, 7107, 250 ),

				// page 7
				new RewardEntry( typeof( BlackthornsKryss ), 800000, 1073260, 5120, 1509 ),
				new RewardEntry( typeof( SwordOfJustice ), 800000, 1073261, 5050, 1150 ),
				new RewardEntry( typeof( GeoffreysAxe ), 800000, 1073263, 3910, 33 ),
				new RewardEntry( typeof( MuseumSpecialAchievementReplica ), 800000, 1073265, 11598, 0 ),
				new RewardEntry( typeof( RewardTitle ), 800000, 1073641, 1073239, 4079, 0 ),
			};

		private static readonly DonationEntry[] DonationList = new DonationEntry[]
			{
				new DonationEntry( typeof( Gold ), 1.0 / 15.0, 1073116, 3823, 0 ),
				new DonationEntry( typeof( BankCheck ), 1.0 / 15.0, 1075013, 5360, 52 ),
				new DonationEntry( typeof( Board ), 1, 1015101, 7127, 0 ),
				new DonationEntry( typeof( OakBoard ), 3, 1075052, 7127, 2010 ),
				new DonationEntry( typeof( AshBoard ), 6, 1075053, 7127, 1191 ),

				new DonationEntry( typeof( YewBoard ), 9, 1075054, 7127, 1192 ),	
				new DonationEntry( typeof( HeartwoodBoard ), 12, 1075062, 7127, 1193 ),
				new DonationEntry( typeof( BloodwoodBoard ), 24, 1075055, 7127, 1194 ),
				new DonationEntry( typeof( FrostwoodBoard ), 48, 1075056, 7127, 1151 ),
				new DonationEntry( typeof( Hinge ), 2, 1044172, 4181, 0 ),

				new DonationEntry( typeof( Hinge ), 2, 1044172, 4182, 0 ), // En OSI sale repetido
				new DonationEntry( typeof( Scorp ), 2, 1075057, 4327, 0 ),
				new DonationEntry( typeof( DrawKnife ), 2, 1075058, 4324, 0 ),
				new DonationEntry( typeof( JointingPlane ), 4, 1075059, 4144, 0 ),
				new DonationEntry( typeof( MouldingPlane ), 4, 1075060, 4140, 0 ),
				
				new DonationEntry( typeof( SmoothingPlane ), 4, 1075061, 4146, 0 ),
			};

		[Constructable]
		public VesperMuseumCollection()
		{
		}

		public override void OnTierAdvanced()
		{
			foreach ( Item item in Map.GetItemsInBounds( m_Locations[Tier - 1] ) )
			{
				if ( item is LocalizedSign || item.Movable )
					continue;

				if ( !item.Visible )
					item.Visible = true;
			}
		}

		public override void OnTierDecreased()
		{
			foreach ( Item item in Map.GetItemsInBounds( m_Locations[Tier] ) )
			{
				if ( item is LocalizedSign || item.Movable )
					continue;

				if ( item.Visible )
					item.Visible = false;
			}
		}

		public VesperMuseumCollection( Serial serial )
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

			reader.ReadInt();
		}
	}
}