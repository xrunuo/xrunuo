using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public enum HumilityQuestStatus
	{
		NonStarted,
		QuestChain,
		RewardPending,
		RewardRefused,
		RewardAccepted,
		Finished
	}

	public abstract class BaseCommunityServiceQuest : BaseQuest
	{
		public override QuestChain ChainID { get { return QuestChain.HumilityCloak; } }
		public override bool DoneOnce { get { return true; } }

		public BaseCommunityServiceQuest()
		{
			m_RewardAvailable = true;
		}

		private bool m_RewardAvailable;

		public bool RewardAvailable
		{
			get { return m_RewardAvailable; }
			set { m_RewardAvailable = value; }
		}

		public static bool RewardAvailableFor<T>( Mobile from ) where T : BaseCommunityServiceQuest
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return false;

			T quest = QuestHelper.GetQuest<T>( pm );

			return quest != null && quest.RewardAvailable;
		}

		public static void GetReward<T>( Mobile from ) where T : BaseCommunityServiceQuest
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			T quest = QuestHelper.GetQuest<T>( pm );

			if ( quest != null )
				quest.RewardAvailable = false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) m_RewardAvailable );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_RewardAvailable = reader.ReadBool();
		}
	}

	public class CommunityServiceMuseumQuest : BaseCommunityServiceQuest
	{
		public override Type NextQuest { get { return typeof( CommunityServiceZooQuest ); } }

		/* Community Service - Museum */
		public override object Title { get { return 1075716; } }

		/* 'Tis time to help out the community of Britannia. Visit the Vesper Museum
		 * and donate to their collection, and eventually thou wilt be able to receive
		 * a replica of the Shepherd's Crook of Humility. Once ye have it, return to
		 * me. Art thou willing to do this? */
		public override object Description { get { return 1075717; } }

		/* I wish that thou wouldest reconsider. */
		public override object Refuse { get { return 1075731; } }

		/* Hello my friend. The museum sitteth in southern Vesper. If ye go downstairs,
		 * ye will discover a small donation chest. That is the place where ye should
		 * leave thy donation. */
		public override object Uncomplete { get { return 1075720; } }

		/* Terrific! The Museum is a worthy cause. Many will benefit from the
		 * inspiration and learning that thine donation hath supported. */
		public override object Complete { get { return 1075721; } }

		// Well done! Thou hast completed this step of the quest. Please return and speak with Gareth.
		public override int CompleteMessage { get { return 1075790; } }

		public CommunityServiceMuseumQuest()
		{
			AddObjective( new ObtainObjective( typeof( ShepherdsCrookOfHumility ), "Shepherd's Crook of Humility", 1 ) );

			AddReward( new BaseReward( 1075852 ) ); // A better understanding of Britannia's people
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

	public class CommunityServiceZooQuest : BaseCommunityServiceQuest
	{
		public override Type NextQuest { get { return typeof( CommunityServiceLibraryQuest ); } }

		/* Community Service – Zoo */
		public override object Title { get { return 1075722; } }

		/* Now, go on and donate to the Moonglow Zoo. Givest thou enough to receive
		 * a 'For the Life of Britannia' sash. Once ye have it, return it to me.
		 * Wilt thou continue? */
		public override object Description { get { return 1075723; } }

		/* I wish that thou wouldest reconsider. */
		public override object Refuse { get { return 1075731; } }

		/* Hello again. The zoo lies a short ways south of Moonglow. Close to the
		 * entrance thou wilt discover a small donation chest. That is where thou
		 * shouldest leave thy donation. */
		public override object Uncomplete { get { return 1075726; } }

		/* Wonderful! The Zoo is a very special place from which people young and
		 * old canst benefit. Thanks to thee, it can continue to thrive. */
		public override object Complete { get { return 1075727; } }

		// Well done! Thou hast completed this step of the quest. Please return and speak with Gareth.
		public override int CompleteMessage { get { return 1075790; } }

		public CommunityServiceZooQuest()
		{
			AddObjective( new ObtainObjective( typeof( ForTheLifeOfBritanniaSash ), "For the Life of Britannia Sash", 1 ) );

			AddReward( new BaseReward( 1075853 ) ); // A better understanding of Britannia's wildlife
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

	public class CommunityServiceLibraryQuest : BaseCommunityServiceQuest
	{
		public override Type NextQuest { get { return typeof( WhosMostHumbleQuest ); } }

		/* Community Service – Library */
		public override object Title { get { return 1075728; } }

		/* I have one more charity for thee, my diligent friend. Go forth and donate
		 * to the Britain Library and do that which is necessary to receive a special
		 * printing of ‘Virtue’, by Lord British. Once in hand, bring the book back
		 * with ye. Art thou ready? */
		public override object Description { get { return 1075729; } }

		/* I wish that thou wouldest reconsider. */
		public override object Refuse { get { return 1075731; } }

		/* Art thou having trouble? The Library lieth north of Castle British's gates.
		 * I believe the representatives in charge of the donations are easy enough
		 * to find. They await thy visit, amongst the many tomes of knowledge. */
		public override object Uncomplete { get { return 1075732; } }

		/* Very good! The library is of great import to the people of Britannia. Thou
		 * hath done a worthy deed and this is thy last required donation. I encourage
		 * thee to continue contributing to thine community, beyond the obligations
		 * of this endeavor. */
		public override object Complete { get { return 1075733; } }

		// Well done! Thou hast completed this step of the quest. Please return and speak with Gareth.
		public override int CompleteMessage { get { return 1075790; } }

		public CommunityServiceLibraryQuest()
		{
			AddObjective( new ObtainObjective( typeof( SpecialPrintingOfVirtue ), "Special Printing of 'Virtue' by Lord British", 1 ) );

			AddReward( new BaseReward( 1075854 ) ); // A better understanding of Britannia's history
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

	public class WhosMostHumbleQuest : BaseQuest
	{
		public override QuestChain ChainID { get { return QuestChain.HumilityCloak; } }
		public override bool DoneOnce { get { return true; } }

		/* Who's Most Humble */
		public override object Title { get { return 1075734; } }

		/* Thou art challenged to find seven citizens spread out among the towns of
		 * Britannia: Skara Brae, Minoc, Britain, and one of the towns upon an isle
		 * at sea. Each citizen wilt reveal some thought concerning Humility. But
		 * who doth best exemplify the virtue? Here, thou needeth wear this plain
		 * grey cloak, for they wilt know ye by it. Wilt thou continue? */
		public override object Description { get { return 1075735; } }

		/* 'Tis a difficult quest, but well worth it. Wilt thou reconsider? */
		public override object Refuse { get { return 1075737; } }

		/* There art no less than seven 'humble citizens' spread across the Britannia
		 * proper. I know that they can be found in the towns of Minoc, Skara Brae
		 * and Britain. Another may be upon an island at sea, the name of which
		 * escapes me at the moment. Thou needeth visit all seven to solve the puzzle.
		 * Be diligent, for they have a tendency to wander about.
		 * 
		 * Dost thou wear the plain grey cloak? */
		public override object Uncomplete { get { return 1075738; } }

		/* Aha! Yes, this is exactly what I was looking for. What think ye of Sean?
		 * Of all those I have met, he is the least concerned with others' opinions
		 * of him. He excels at his craft, yet always believes he has something left
		 * to learn. *looks at the iron chain necklace* And it shows, does it not? */
		public override object Complete { get { return 1075773; } }

		// Well done! Thou hast completed this step of the quest. Please return and speak with Gareth.
		public override int CompleteMessage { get { return 1075790; } }

		public WhosMostHumbleQuest()
		{
			AddObjective( new ObtainObjective( typeof( IronChain ), "Iron Chain", 1 ) );

			AddReward( new BaseReward( 1075855 ) ); // A chance to better know thyself
		}

		public override void OnAccept()
		{
			base.OnAccept();

			Owner.PlaceInBackpack( new BrassRing() );
			Owner.PlaceInBackpack( new PlainGreyCloak() );

			m_Desires = DesireCollection.Build();
		}

		public override void OnResign( bool resignChain )
		{
			base.OnResign( resignChain );

			Owner.DropHolding();

			foreach ( DesireDefinition def in BaseErrand.Desires )
				Owner.DeleteItemsByType( def.Type );

			Owner.DeleteItemsByType<BrassRing>();
			Owner.DeleteItemsByType<PlainGreyCloak>();
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Owner.HumilityQuestStatus = HumilityQuestStatus.RewardPending;
			Owner.SendGump( new HumilityProofGump() );
		}

		private DesireCollection m_Desires;

		public static DesireCollection GetDesires( PlayerMobile pm )
		{
			WhosMostHumbleQuest quest = QuestHelper.GetQuest<WhosMostHumbleQuest>( pm );

			if ( quest != null )
				return quest.m_Desires;

			return null;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			m_Desires.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Desires = new DesireCollection( reader );
		}
	}

	public class Gareth : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( CommunityServiceMuseumQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		[Constructable]
		public Gareth()
			: base( "Gareth", "the Emissary of the RBC" )
		{
			SpeechHue = 0x47E;
		}

		public Gareth( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x203D;
			HairHue = 0x2E0;
			FacialHairItemID = 0x204B;
			FacialHairHue = 0x2E0;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Boots() );
			AddItem( new LongPants( 0x222 ) );
			AddItem( new FancyShirt( 0x2B8 ) );
			AddItem( new BodySash( 0x386 ) );
		}

		public override void OnTalk( PlayerMobile player )
		{
			switch ( player.HumilityQuestStatus )
			{
				case HumilityQuestStatus.NonStarted:
					{
						if ( player.HasGump( typeof( GenericQuestGump ) ) || player.HasGump( typeof( QuestQuestionGump ) ) )
							break;

						if ( player.HumilityQuestNextChance < DateTime.UtcNow )
							player.SendGump( new KnowThyHumilityGump() );
						else
							Say( 1075787 ); // I feel that thou hast yet more to learn about Humility... Please ponder these things further, and visit me again on the 'morrow.

						break;
					}
				case HumilityQuestStatus.QuestChain:
					{
						base.OnTalk( player );
						break;
					}
				case HumilityQuestStatus.RewardPending:
					{
						break;
					}
				case HumilityQuestStatus.RewardRefused:
					{
						player.CloseGump( typeof( GenericQuestGump ) );

						/* Ah yes, there is an island far to the south where ye canst find the
						 * Shrine of Humility. Seek solace there, my friend, that thine questions
						 * might be answered. */
						player.SendGump( new GenericQuestGump( 1075900 ) );

						break;
					}
				case HumilityQuestStatus.RewardAccepted:
					{
						Say( 1075898 ); // Worry not, noble one! We shall never forget thy deeds!
						break;
					}
				case HumilityQuestStatus.Finished:
					{
						Say( "Hail, friend!" );
						break;
					}
			}
		}

		public override void Advertise()
		{
			Say( 1075674 ); // Hail! Care to join our efforts for the Rise of Britannia?
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