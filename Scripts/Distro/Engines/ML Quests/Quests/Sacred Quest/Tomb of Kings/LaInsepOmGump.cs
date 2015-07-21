using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests.SacredQuest
{
	public class LaInsepOmGump : GenericQuestGump
	{
		public LaInsepOmGump()
			: base( 1112682, GenericQuestGumpButton.Accept | GenericQuestGumpButton.Refuse, OnAccepted, OnCanceled )
		{
			/* Repeating the mantra, you gradually enter
			 * a state of enlightened meditation.
			 * 
			 * As you contemplate your worthiness, an image
			 * of the Book of Circles comes into focus.
			 * 
			 * Perhaps you are ready for La Insep Om? */

			AddHtmlLocalized( 98, 140, 312, 16, 1072202, 0x2710, false, false ); // Description
			AddHtmlLocalized( 160, 108, 250, 16, 1112681, 0x2710, false, false ); // La Insep Om
		}

		public static void OnAccepted( Mobile from )
		{
			// Focusing more upon the Book of Circles, you realize that you must now show your mastery of its contents.
			from.SendGump( new GenericQuestGump( 1112684, GenericQuestGumpButton.Continue, StartQuestions ) );
		}

		public static void OnCanceled( Mobile from )
		{
			// You feel as if you should return when you are worthy.
			from.SendGump( new GenericQuestGump( 1112683 ) );
		}

		public static void StartQuestions( Mobile from )
		{
			QuestionDefinition[] questions = Questions.BuildQuestions();

			from.SendGump( new QuestQuestionGump( questions, 0, Questions.OnQuestionsPassed, Questions.OnQuestionsFailed ) );
		}
	}
}