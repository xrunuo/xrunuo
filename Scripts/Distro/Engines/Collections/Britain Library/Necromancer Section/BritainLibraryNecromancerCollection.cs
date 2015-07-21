using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests.HumilityCloak;

namespace Server.Engines.Collections
{
	public class BritainLibraryNecromancerCollection : BritainLibraryCollection
	{
		public override int Section { get { return 1073445; } } // Oil and Oubliette Section Donation Representative.

		public override IRewardEntry[] Rewards { get { return RewardList; } }
		public override DonationEntry[] Donations { get { return DonationList; } }

		private static readonly IRewardEntry[] RewardList = new IRewardEntry[]
			{
				new ConditionalRewardEntry( typeof( SpecialPrintingOfVirtue ), 5000, 1075793, 0xFF0, 0x47E, BaseCommunityServiceQuest.GetReward<CommunityServiceLibraryQuest>, BaseCommunityServiceQuest.RewardAvailableFor<CommunityServiceLibraryQuest> ),

				new RewardEntry( typeof( LibraryRewardBodySash ), 100000, 1073346, ClothHues, 5441, 800 ),
				new RewardEntry( typeof( LibraryRewardFeatheredHat ), 100000, 1073347, ClothHues, 5914, 800 ),
				new RewardEntry( typeof( LibraryRewardSurcoat ), 100000, 1073348, ClothHues, 8189, 800 ),
				new RewardEntry( typeof( LibraryRewardPants ), 100000, 1073349, ClothHues, 5433, 800 ),
				new RewardEntry( typeof( LibraryRewardCloak ), 100000, 1073350, ClothHues, 5397, 800 ),
				new RewardEntry( typeof( LibraryRewardDoublet ), 100000, 1073351, ClothHues, 8059, 800 ),
				new RewardEntry( typeof( LibraryRewardKilt ), 100000, 1073352, ClothHues, 5431, 800 ),
				new RewardEntry( typeof( RewardTitle ), 100000, 1073859, 1073341, 4079, 0 ),
				new RewardEntry( typeof( LibraryRewardLantern ), 200000, 1073339, BookHues, 2584, 450 ),
				new RewardEntry( typeof( LibraryRewardChair ), 200000, 1073340, BookHues, 11755, 450 ),
				new RewardEntry( typeof( RewardTitle ), 200000, 1073860, 1073342, 4079, 0 ),
				new RewardEntry( typeof( QuotesSherryTheMouse ), 350000, 1073300, BookHues, 4029, 450 ),
				new RewardEntry( typeof( QuotesWyrdBeastmaster ), 350000, 1073310, BookHues, 4029, 450 ),
				new RewardEntry( typeof( QuotesMercenaryJustin ), 350000, 1073317, BookHues, 4029, 450 ),
				new RewardEntry( typeof( QuotesMoonglow ), 350000, 1073327, BookHues, 4029, 450 ),
				new RewardEntry( typeof( QuotesHoraceTrader ), 350000, 1073338, BookHues, 4029, 450 ),
				new RewardEntry( typeof( RewardTitle ), 350000, 1073861, 1073343, 4079, 0 ),
				new RewardEntry( typeof( TalismanTreatiseonAlchemy ), 550000, 1073353, 12120, 0 ),
				new RewardEntry( typeof( TalismanAprimeronArms ), 550000, 1073354, 12121, 0 ),
				new RewardEntry( typeof( TalismanMyBook ), 550000, 1073355, 12122, 0 ),
				new RewardEntry( typeof( TalismanTalkingtoWisps ), 550000, 1073356, 12123, 0 ),
				new RewardEntry( typeof( TalismanGrammarofOrchish ), 550000, 1073358, 12121, 0 ),
				new RewardEntry( typeof( TalismanBirdsofBritannia ), 550000, 1073359, 12122, 0 ),
				new RewardEntry( typeof( TalismanTravelingMinstrel ), 550000, 1073360, 12123, 0 ),
				new RewardEntry( typeof( RewardTitle ), 550000, 1073862, 1073344, 4079, 0 ),
				new RewardEntry( typeof( RewardTitle ), 800000, 1073863, 1073345, 4079, 0 ),

				// page 6
				new RewardEntry( typeof( NecromanticReadingGlasses ), 800000, 1073377, 12216, 557 ),
			};

		private static readonly DonationEntry[] DonationList = new DonationEntry[]
			{
				// page 1
				new DonationEntry( typeof( Gold ), 1.0 / 15.0, 1073116, 3823, 0 ),
				new DonationEntry( typeof( BankCheck ), 1.0 / 15.0, 1075013, 5360, 52 ),
				new DonationEntry( typeof( NecromancerSpellbook ), 12, 1074909, 8787, 0 ),
				new DonationEntry( typeof( PigIron ), 1, 1023978, 3978, 0 ),
				new DonationEntry( typeof( GraveDust ), 1, 1023983, 3983, 0 ),
				
				// page 2
				new DonationEntry( typeof( NoxCrystal ), 1, 1023982, 3982, 0 ),
				new DonationEntry( typeof( DaemonBlood ), 1, 1023965, 3965, 0 ),
				new DonationEntry( typeof( BatWing ), 1, 1023960, 3960, 0 ),
				new DonationEntry( typeof( BrownBook ), 3, 1074906, 4079, 0 ),
                new DonationEntry( typeof( TanBook ), 3, 1074906, 4080, 0 ),
			};

		public override void Spawn_Representative()
		{
			NecromancerSectionRepresentative npc = new NecromancerSectionRepresentative();
			npc.Controller = this;
			this.Representative = npc;
			npc.MoveToWorld( this.Location, this.Map );
		}

		[Constructable]
		public BritainLibraryNecromancerCollection()
		{
		}

		public BritainLibraryNecromancerCollection( Serial serial )
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