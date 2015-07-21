using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class KillDeathwatchBeetlesObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Kill 10 Deathwatch Beetle Hatchlings and return
				 * to Ansella Gryen.
				*/
				return 1063316;
			}
		}

		public override int MaxProgress { get { return 10; } }

		public override bool Completed
		{
			get
			{
				if ( CurProgress > 0 )
				{
					return true;
				}
				else
				{
					return base.Completed;
				}
			}
		}

		public KillDeathwatchBeetlesObjective()
		{
		}

		public override void RenderProgress( BaseQuestGump gump )
		{
			if ( !Completed )
			{
				gump.Intern( "/" );
				gump.Intern( CurProgress.ToString() );
				gump.Intern( MaxProgress.ToString() );

				gump.AddHtmlObject( 70, 260, 270, 100, 1063318, BaseQuestGump.Blue, false, false ); // Deathwatch Beetle Hatchlings killed:
				gump.AddLabelIntern( 70, 280, 0x64, 1 );
				gump.AddLabelIntern( 100, 280, 0x64, 0 );
				gump.AddLabelIntern( 130, 280, 0x64, 2 );
			}
			else
			{
				base.RenderProgress( gump );
			}
		}

		public override void OnRead()
		{
			CheckCompletionStatus();
		}

		public override void OnKill( BaseCreature creature, Container corpse )
		{
			if ( creature is DeathWatchBeetleHatchling && corpse.Map == Map.Tokuno )
			{
				if ( CurProgress == 0 )
				{
					CurProgress++;

					System.AddObjective( new GreatJobObjective() );
				}
			}
		}
	}

	public class GreatJobObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Great job! One less terrible hatchling in the
			         * Waste!<BR><BR>
				 * Once you've killed 10 of the Deathwatch Beetle
				 * Hatchlings, return to Ansella for your reward!
				 */
				return 1063320;
			}
		}

		public override int MaxProgress { get { return 10; } }

		public override bool Completed
		{
			get
			{
				if ( CurProgress > 0 )
				{
					return true;
				}
				else
				{
					return base.Completed;
				}
			}
		}

		public override void OnRead()
		{
			if ( !Completed )
			{
				CurProgress = 1;
			}

			CheckCompletionStatus();

			System.AddObjective( new ContinueKillDeathwatchBeetlesObjective() );
		}

		public GreatJobObjective()
		{
		}

		public override void RenderProgress( BaseQuestGump gump )
		{
			if ( !Completed )
			{
				gump.Intern( "/" );
				gump.Intern( "1" );
				gump.Intern( MaxProgress.ToString() );

				gump.AddHtmlObject( 70, 260, 270, 100, 1063318, BaseQuestGump.Blue, false, false ); // Deathwatch Beetle Hatchlings killed:
				gump.AddLabelIntern( 70, 280, 0x64, 1 );
				gump.AddLabelIntern( 100, 280, 0x64, 0 );
				gump.AddLabelIntern( 130, 280, 0x64, 2 );
			}
			else
			{
				base.RenderProgress( gump );
			}
		}
	}

	public class ContinueKillDeathwatchBeetlesObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				// Continue killing Deathwatch Beetle Hatchlings.
				return 1063319;
			}
		}

		public override int MaxProgress { get { return 10; } }

		public ContinueKillDeathwatchBeetlesObjective()
		{
			CurProgress = 1;
		}

		public override void OnRead()
		{
			CheckCompletionStatus();
		}

		public override void RenderProgress( BaseQuestGump gump )
		{
			if ( !Completed )
			{
				gump.Intern( "/" );
				gump.Intern( CurProgress.ToString() );
				gump.Intern( MaxProgress.ToString() );

				gump.AddHtmlObject( 70, 260, 270, 100, 1063318, BaseQuestGump.Blue, false, false ); // Deathwatch Beetle Hatchlings killed:
				gump.AddLabelIntern( 70, 280, 0x64, 1 );
				gump.AddLabelIntern( 100, 280, 0x64, 0 );
				gump.AddLabelIntern( 130, 280, 0x64, 2 );
			}
			else
			{
				base.RenderProgress( gump );
			}
		}

		public override void OnKill( BaseCreature creature, Container corpse )
		{
			if ( creature is DeathWatchBeetleHatchling && corpse.Map == Map.Tokuno )
			{
				CurProgress++;
			}
		}

		public override void OnComplete()
		{
			System.AddObjective( new ReturnToAnsellaGryenObjective() );
		}
	}

	public class ReturnToAnsellaGryenObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				// Return to Ansella Gryen for your reward.
				return 1063313;
			}
		}

		public ReturnToAnsellaGryenObjective()
		{
		}
	}
}