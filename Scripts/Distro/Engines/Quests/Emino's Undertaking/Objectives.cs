using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class FindDaimyoEminoObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Your value as a Ninja must be proven. Find
				 * Daimyo Emino and accept the test he offers.
				 */
				return 1063174;
			}
		}

		public FindDaimyoEminoObjective()
		{
		}
	}

	public class FindEliteNinjaZoelObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				// Find Elite Ninja Zoel immediately!
				return 1063176;
			}
		}

		public FindEliteNinjaZoelObjective()
		{
		}
	}

	public class EnterTheCaveObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Enter the cave and walk through it. You will be
				 * tested as you travel along the path.
				 */
				return 1063179;
			}
		}

		public EnterTheCaveObjective()
		{
		}
	}

	public class UseNinjaTrainingsObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Use your Ninja training to move invisibly past
				 * the magical guardians.
				 */
				return 1063261;
			}
		}

		public UseNinjaTrainingsObjective()
		{
		}
	}

	public class TakeGreenTeleporterObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* The special tile is known as a teleporter. Step
				 * on the teleporter tile and you will be
				 * transported to a new location.
				 */
				return 1063183;
			}
		}

		public TakeGreenTeleporterObjective()
		{
		}
	}

	public class BringNoteToZoelObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Bring the note to Elite Ninja Zoel and speak
				 * with him again. He is near the cave entrance.
				 * You can hand the note to Zoel by dragging it
				 * and dropping it on his body.
				 */
				return 1063185;
			}
		}

		public BringNoteToZoelObjective()
		{
		}
	}

	public class TakeBlueTeleporterObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Take the Blue Teleporter Tile from Daimyo
				 * Emino's house to the Abandoned Inn. Quietly look
				 * around to gain information.
				 */
				return 1063190;
			}
		}

		public TakeBlueTeleporterObjective()
		{
		}
	}

	public class GoBackBlueTeleporterObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Go back through the blue teleporter and tell
				 * Daimyo Emino what you've overheard.
				 */
				return 1063197;
			}
		}

		public GoBackBlueTeleporterObjective()
		{
		}
	}

	public class TakeWhiteTeleporterObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Take the white teleporter and check the chests
				 * for the sword. Leave everything else behind.
				 * Avoid damage from traps you may encounter. To
				 * use a potion, make sure at least one hand is
				 * free and double click on the bottle.
				 */
				return 1063200;
			}
		}

		public TakeWhiteTeleporterObjective()
		{
		}
	}

	public class WalkThroughHallwayObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Walk through the hallway being careful to avoid
				 * the traps. You may be able to time the traps
				 * to avoid injury.
				 */
				return 1063202;
			}
		}

		public WalkThroughHallwayObjective()
		{
		}
	}

	public class TakeSwordObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* Take the sword and bring it back to Daimyo
				 * Emino.
				 */
				return 1063204;
			}
		}

		public TakeSwordObjective()
		{
		}
	}

	public class KillHenchmensObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				// Kill three henchmen.
				return 1063206;
			}
		}

		public override int MaxProgress { get { return 3; } }

		public override void RenderProgress( BaseQuestGump gump )
		{
			if ( !Completed )
			{
				gump.Intern( "/" );
				gump.Intern( CurProgress.ToString() );
				gump.Intern( MaxProgress.ToString() );

				gump.AddHtmlObject( 70, 260, 270, 100, 1063207, BaseQuestGump.Blue, false, false ); // Henchmen killed:
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
			if ( creature is Henchman )
			{
				CurProgress++;
			}
		}

		public KillHenchmensObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddConversation( new GoToEminoConversation() );

			System.AddObjective( new ReturnToDaimyoObjective() );
		}
	}

	public class ReturnToDaimyoObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				/* You have proven your fighting skills. Bring the
				 * Sword to Daimyo Emino immediately. Be sure to
				 * follow the path back to the teleporter.
				 */
				return 1063210;
			}
		}

		public ReturnToDaimyoObjective()
		{
		}
	}
}