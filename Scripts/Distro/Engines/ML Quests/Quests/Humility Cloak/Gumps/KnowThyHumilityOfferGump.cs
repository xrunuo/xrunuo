using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests.HumilityCloak
{
	public class KnowThyHumilityGump : GenericQuestGump
	{
		public KnowThyHumilityGump()
			: base( 1075675, GenericQuestGumpButton.Accept | GenericQuestGumpButton.Refuse, OnAccepted, OnCanceled )
		{
			/* Greetings my friend! My name is Gareth, and I represent a group of
			 * citizens who wish to rejuvenate interest in our kingdom's noble
			 * heritage. 'Tis our belief that one of Britannia's greatest triumphs
			 * was the institution of the Virtues, neglected though they be now.
			 * To that end I have a set of tasks prepared for one who would follow
			 * a truly Humble path. Art thou interested in joining our effort? */

			AddHtmlLocalized( 98, 140, 312, 16, 1072202, 0x2710, false, false ); // Description
			AddHtmlLocalized( 160, 108, 250, 16, 1075850, 0x2710, false, false ); // Know Thy Humility
		}

		public static void OnAccepted( Mobile from )
		{
			// Wonderful! First, let us see if thou art reading from the same roll of parchment as we are. *smiles*
			from.SendGump( new GenericQuestGump( 1075676, GenericQuestGumpButton.Continue, StartQuestions ) );
		}

		public static void OnCanceled( Mobile from )
		{
			// I wish that thou wouldest reconsider.
			from.SendGump( new GenericQuestGump( 1075677 ) );
		}

		public static void StartQuestions( Mobile from )
		{
			QuestionDefinition[] questions = Questions.BuildQuestions();

			from.SendGump( new QuestQuestionGump( questions, 0, Questions.OnQuestionsPassed, Questions.OnQuestionsFailed ) );
		}
	}
}