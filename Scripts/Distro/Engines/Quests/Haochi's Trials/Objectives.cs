using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class SpeakToDaimyoHaochiObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				// Speak to Daimyo Haochi.
				return 1063026;
			}
		}

		public SpeakToDaimyoHaochiObjective()
		{
		}
	}

	public class FollowGreenPathObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Follow the green path. The guards will now let
				 * you through.
				 */
				return 1063030;
			}
		}

		public FollowGreenPathObjective()
		{
		}
	}

	public class KillRoninsOrSoulsObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Kill 3 Young Ronin or 3 Cursed Souls. Return to
				 * Daimyo Haochi when you have finished.
				 */
				return 1063032;
			}
		}

		public override void OnKill( BaseCreature creature, Container corpse )
		{
			HaochisTrialsQuest htq = System as HaochisTrialsQuest;

			if ( htq == null )
			{
				return;
			}

			if ( creature is YoungRonin )
			{
				htq.KilledRonins++;

				htq.From.SendLocalizedMessage( 1063039, htq.KilledRonins.ToString() ); // Young Ronin killed:  ~1_COUNT~				

				if ( !htq.SendRoninKarma )
				{
					htq.AddConversation( new GainKarmaForRoninConversation() );

					htq.SendRoninKarma = true;
				}

				if ( htq.KilledRonins == 3 )
				{
					htq.KilledSouls = 0;

					Complete();

					htq.AddObjective( new FirstTrialCompleteObjective() );
				}
			}
			else if ( creature is CursedSoul )
			{
				htq.KilledSouls++;

				htq.From.SendLocalizedMessage( 1063038, htq.KilledSouls.ToString() ); // Cursed Souls killed:  ~1_COUNT~

				if ( !htq.SendSoulsKarma )
				{
					htq.AddConversation( new GainKarmaForSoulsConversation() );

					htq.SendSoulsKarma = true;
				}

				if ( htq.KilledSouls == 3 )
				{
					htq.KilledRonins = 0;

					Complete();

					htq.AddObjective( new FirstTrialCompleteObjective() );
				}
			}
		}

		public KillRoninsOrSoulsObjective()
		{
		}
	}

	public class FirstTrialCompleteObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* The first trial is complete. Return to Daimyo
				 * Haochi.
				 */
				return 1063044;
			}
		}

		public FirstTrialCompleteObjective()
		{
		}
	}

	public class FollowYellowPathObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Follow the yellow path. The guards will now let
				 * you through.
				 */
				return 1063047;
			}
		}

		public FollowYellowPathObjective()
		{
		}
	}

	public class ChooseOpponentObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Choose your opponent and attack one with all
				 * your skill.
				 */
				return 1063058;
			}
		}

		public override void OnAttack( BaseCreature creature )
		{
			HaochisTrialsQuest htq = System as HaochisTrialsQuest;

			if ( htq == null )
			{
				return;
			}

			if ( creature is FierceDragon )
			{
				Complete();

				htq.Opponent = OpponentType.FierceDragon;

				System.AddObjective( new SecondTrialCompleteObjective() );
			}

			if ( creature is DeadlyImp )
			{
				Complete();

				htq.Opponent = OpponentType.DeadlyImp;

				System.AddObjective( new SecondTrialCompleteObjective() );
			}
		}

		public ChooseOpponentObjective()
		{
		}
	}

	public class SecondTrialCompleteObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* The second trial is complete.  Return to Daimyo
				 * Haochi.
				 */
				return 1063229;
			}
		}

		public SecondTrialCompleteObjective()
		{
		}
	}

	public class FollowBluePathObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* The next trial will test your benevolence. Follow
				 * the blue path. The guards will now let you
				 * through.
				 */
				return 1063061;
			}
		}

		public FollowBluePathObjective()
		{
		}
	}

	public class UseHonorableExecutionObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Use your Honorable Execution skill to finish off
				 * the wounded wolf. Double click the icon in your
				 * Book of Bushido to activate the skill. When you
				 * are done, return to Daimyo Haochi.
				 */
				return 1063063;
			}
		}

		public override void OnKill( BaseCreature creature, Container corpse )
		{
			if ( creature is InjuredWolf )
			{
				Complete();

				System.AddObjective( new ThirdTrialCompleteObjective() );
			}
		}

		public UseHonorableExecutionObjective()
		{
		}
	}

	public class ThirdTrialCompleteObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				// Return to Daimyo Haochi.
				return 1063064;
			}
		}

		public ThirdTrialCompleteObjective()
		{
		}
	}

	public class FollowRedPathObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Follow the red path and pass through the guards
				 * to the entrance of the fourth trial.
				 */
				return 1063066;
			}
		}

		public FollowRedPathObjective()
		{
		}
	}

	public class GiveGypsyGoldOrHuntCatsObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Give the gypsy gold or hunt one of the cats to
				 * eliminate the undue need it has placed on the
				 * gypsy.
				 */
				return 1063068;
			}
		}

		public override void OnKill( BaseCreature creature, Container corpse )
		{
			HaochisTrialsQuest htq = System as HaochisTrialsQuest;

			if ( htq == null )
			{
				return;
			}

			if ( creature is DiseasedCat )
			{
				Complete();

				htq.Choice = ChoiceType.Cats;

				System.AddObjective( new MadeChoiceObjective() );
			}
		}

		public GiveGypsyGoldOrHuntCatsObjective()
		{
		}
	}

	public class MadeChoiceObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* You have made your choice.  Return now to
				 * Daimyo Haochi.
				 */
				return 1063242;
			}
		}

		public MadeChoiceObjective()
		{
		}
	}

	public class RetrieveKatanaObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Retrieve Daimyo Haochi's katana from the
				 * treasure room.
				 */
				return 1063072;
			}
		}

		public RetrieveKatanaObjective()
		{
		}
	}

	public class GiveSwordDaimyoObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				// Give the sword to Daimyo Haochi. 
				return 1063073;
			}
		}

		public GiveSwordDaimyoObjective()
		{
		}
	}

	public class LightCandleObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Light one of the candles near the altar and
				 * return to Daimyo Haochi.
				 */
				return 1063078;
			}
		}

		public LightCandleObjective()
		{
		}
	}

	public class CandleCompleteObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* You have done as requested.  Return to Daimyo
				 * Haochi.
				 */
				return 1063252;
			}
		}

		public CandleCompleteObjective()
		{
		}
	}

	public class KillNinjaObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Three young Ninja must be dealt with. Your job
				 * is to kill them. When you have done so, return
				 * to Daimyo Haochi.
				 */
				return 1063080;
			}
		}

		public override int MaxProgress { get { return 3; } }

		public override void OnRead()
		{
			CheckCompletionStatus();
		}

		public override void OnKill( BaseCreature creature, Container corpse )
		{
			if ( creature is YoungNinja )
			{
				CurProgress++;
			}
		}

		public KillNinjaObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddObjective( new ExecutionsCompleteObjective() );
		}
	}

	public class ExecutionsCompleteObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* The executions are complete.  Return to the
				 * Daimyo.
				 */
				return 1063253;
			}
		}

		public ExecutionsCompleteObjective()
		{
		}
	}
}