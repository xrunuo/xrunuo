using System;
using Server.Items;
using Server.Mobiles;
using Server.Factions;

namespace Server.Engines.Collections
{
	public class FactionCommunityCollection : CollectionController
	{
		// TODO (ML): verify this
		public override int PointsPerTier { get { return 100000 * ( Tier + 1 ); } }

		public override int MaxTiers { get { return 5; } }

		public override IRewardEntry[] Rewards { get { return RewardList; } }
		public override DonationEntry[] Donations { get { return DonationList; } }

		public override int LabelNumber { get { return 1094770; } } // The Vault

		private static readonly IRewardEntry[] RewardList = new IRewardEntry[]
			{
				// page 1
				new RewardEntry( typeof( GreaterStaminaPotion ), 50, 1094764, 3849, 33 ),
				new RewardEntry( typeof( ShrineGem ), 100, 1094711, 4040, 1151 ),
				new RewardEntry( typeof( SupernovaPotion ), 100, 1094718, 3849, 13 ),
				new RewardEntry( typeof( EnchantedBandage ), 100, 1094712, 3617, 1174 ),
				new RewardEntry( typeof( FactionStrongoldRune ), 150, 1094700, 7956, 1154 ),
				new RewardEntry( typeof( PowderOfPerseverance ), 300, 1094756, 4102, 1352 ),
				new RewardEntry( typeof( MorphEarrings ), 1000, 1094746, 4231, 250 ),

				// page 2
				new FactionRewardEntry( typeof( FactionKasaOfRajin ), 1, 1070969, 10136, 0 ),
				new FactionRewardEntry( typeof( FactionRuneBeetleCarapace ), 1, 1070968, 10109, 0 ),
				new FactionRewardEntry( typeof( FactionStormgrip ), 1, 1070970, 10130, 0 ),
				new FactionRewardEntry( typeof( FactionFeyLeggings ), 1, 1075041, 5054, 0 ),
				new FactionRewardEntry( typeof( FactionRingOfTheVile ), 2, 1061102, 4234, 1271 ),
				new FactionRewardEntry( typeof( FactionCrimsonCincture ), 2, 1075043, 5435, 232 ),
				
				// page 3
				new FactionRewardEntry( typeof( FactionHeartOfTheLion ), 2, 1070817, 5141, 1281 ),
				new FactionRewardEntry( typeof( FactionHuntersHeaddress ), 2, 1061595, 5447, 1428 ),
				new FactionRewardEntry( typeof( FactionTomeOfLostKnowledge ), 3, 1070971, 3834, 1328 ),
				new FactionRewardEntry( typeof( FactionTalismanAprimeronArms ), 3, 1094704, 12121, 0 ),
				new FactionRewardEntry( typeof( FactionWizardsCrystalReadingGlasses ), 3, 1073374, 12216, 688 ),

				// page 4
				new FactionRewardEntry( typeof( FactionSpiritOfTheTotem ), 3, 1061599, 5445, 1109 ),
				new FactionRewardEntry( typeof( FactionCrystallineRing ), 4, 1075096, 4234, 1152 ),
				new FactionRewardEntry( typeof( FactionFoldedSteelReadingGlasses ), 4, 1073380, 12216, 1150 ),
				new FactionRewardEntry( typeof( FactionClaininsSpellbook ), 4, 1094705, 3834, 2125 ),
				new FactionRewardEntry( typeof( FactionMuseumOrderShield ), 4, 1073258, 7108, 0 ),

				// page 5
				new FactionRewardEntry( typeof( FactionOrnamentOfTheMagician ), 5, 1061105, 4230, 1364 ),
				new FactionRewardEntry( typeof( FactionInquisitorsResolution ), 5, 1060206, 5140, 1266 ),
				new FactionRewardEntry( typeof( FactionMaceAndShieldReadingGlasses ), 5, 1073381, 12216, 477 ),
			};

		private static readonly DonationEntry[] DonationList = new DonationEntry[]
			{
				new DonationEntry( typeof( Silver ), 1, 1044572, 3826, 0 )
			};

		public override void OnRewardCreated( Mobile from, Item reward )
		{
			if ( reward is IFactionArtifact )
				( (IFactionArtifact) reward ).Owner = from;
		}

		[Constructable]
		public FactionCommunityCollection()
			: base( 0x9AB, 1157 )
		{
		}

		public FactionCommunityCollection( Serial serial )
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

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class FactionRewardEntry : RewardEntry
	{
		private readonly int m_Tier;

		public FactionRewardEntry( Type type, int tier, int cliloc, int tile, int hue )
			: base( type, 0, cliloc, null, tile, hue )
		{
			m_Tier = tier;
		}

		public override int GetPrice( Mobile m )
		{
			int rank = GetFactionRank( m );

			int min = ( m_Tier * 1000 );
			int max = ( m_Tier * m_Tier * 2000 );

			return Math.Max( max - ( min * ( rank - 1 ) ), min );
		}

		private int GetFactionRank( Mobile m )
		{
			var playerState = PlayerState.Find( m );
			return playerState != null ? playerState.Rank.Rank : 1;
		}
	}
}