using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests.SE
{
	public class TerribleHatchlingsQuest : QuestSystem
	{
		private static Type[] m_TypeReferenceTable = new Type[] { typeof( SE.KillDeathwatchBeetlesObjective ), typeof( SE.GreatJobObjective ), typeof( SE.ContinueKillDeathwatchBeetlesObjective ), typeof( SE.ReturnToAnsellaGryenObjective ), typeof( SE.ImportantQuestInformationConversation ), typeof( SE.DeathwatchBeetlesLocationConversation ), typeof( SE.RewardsConversation ) };

		public override Type[] TypeReferenceTable { get { return m_TypeReferenceTable; } }

		public override object Name
		{
			get
			{
				// "Terrible Hatchlings"
				return 1063314;
			}
		}

		public override object OfferMessage
		{
			get
			{
				/* The Deathwatch Beetle Hatchlings have trampled 
				 * through my fields again, what a nuisance! Please 
				 * help me get rid of the terrible hatchlings. If
			 	 * you kill 10 of them, you will be rewarded. The
				 * Deathwatch Beetle Hatchlings live in The Waste
				 * - the desert close to this city.<BR><BR>
				 * Will you accept this challenge?
				 */
				return 1063315;
			}
		}

		public override TimeSpan RestartDelay { get { return TimeSpan.MaxValue; } }
		public override bool IsTutorial { get { return true; } }

		public override int Picture { get { return 0x15CF; } }

		public TerribleHatchlingsQuest( PlayerMobile from )
			: base( from )
		{
		}

		// Serialization
		public TerribleHatchlingsQuest()
		{
		}

		public override void Accept()
		{
			base.Accept();

			AddConversation( new ImportantQuestInformationConversation() );

			AddObjective( new KillDeathwatchBeetlesObjective() );
		}
	}
}