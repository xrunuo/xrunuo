using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Questions
	{
		private static QuestionDefinition[] m_Definitions = new QuestionDefinition[]
			{
				new QuestionDefinition
					(
						1075678, // What is the symbol of Humility?
						new AnswerDefinition( 1075679, true ), // A Shepherd’s Crook
						new AnswerDefinition( 1075680, false ), // Five hands embraced 
						new AnswerDefinition( 1075681, false ), // A Bowed Head 
						new AnswerDefinition( 1075682, false ) // A Bare Hand
					),
				new QuestionDefinition
					(
						1075683, // What opposes Humility?
						new AnswerDefinition( 1075685, true ), // Pride 
						new AnswerDefinition( 1075684, false ), // Glory 
						new AnswerDefinition( 1075686, false ), // Hatred
						new AnswerDefinition( 1075687, false ) // Ego
					),
				new QuestionDefinition
					(
						1075688, // What is the color of Humility?
						new AnswerDefinition( 1075691, true ), // Black 
						new AnswerDefinition( 1075689, false ), // Grey  
						new AnswerDefinition( 1075690, false ), // Red
						new AnswerDefinition( 1075692, false ) // Brown
					),
				new QuestionDefinition
					(
						1075693, // How doth one find Humility?
						new AnswerDefinition( 1075697, true ), // Through the absence of virtue 
						new AnswerDefinition( 1075694, false ), // Through Love and Courage  
						new AnswerDefinition( 1075695, false ), // With Love and Truth
						new AnswerDefinition( 1075696, false ) // Through a bounty of Truth, Love, and Courage
					),
				new QuestionDefinition
					(
						1075698, // Which city embodies the need for Humility?
						new AnswerDefinition( 1075700, true ), // Magincia
						new AnswerDefinition( 1075701, false ), // Britain
						new AnswerDefinition( 1075702, false ), // Buccaneer’s Den
						new AnswerDefinition( 1075699, false ) // Nujel’m
					),
				new QuestionDefinition
					(
						1075703, // By name, which den of evil challenges one’s humility?
						new AnswerDefinition( 1075705, true ), // Hythloth
						new AnswerDefinition( 1075704, false ), // Despise 
						new AnswerDefinition( 1075706, false ), // Covetous
						new AnswerDefinition( 1075707, false ) // Shame
					),
				new QuestionDefinition
					(
						1075708, // Finish this truism: Humility shows us...
						new AnswerDefinition( 1075709, true ), // ...the value of all individuals.
						new AnswerDefinition( 1075710, false ), // ...that pride is evil. 
						new AnswerDefinition( 1075711, false ), // ...the path to true glory.
						new AnswerDefinition( 1075712, false ) // ...that “I” am not “great”.
					),
			};

		public static QuestionDefinition[] BuildQuestions()
		{
			QuestionDefinition[] questions = new QuestionDefinition[4];
			Array.Copy( Utility.Shuffle( m_Definitions ), questions, 4 );

			return questions;
		}

		public static void OnQuestionsFailed( Mobile from )
		{
			from.PlaySound( 0x5B3 );
			from.PlaySound( 0x41F );

			/* Ah... no, that is not quite right. Truly, Humility is something that
			 * takes time and experience to understand. I wish to challenge thee to
			 * seek out more knowledge concerning this virtue, and tomorrow let us
			 * speak again about what thou hast learned. */
			from.SendGump( new GenericQuestGump( 1075713 ) );

			if ( from is PlayerMobile )
				( (PlayerMobile) from ).HumilityQuestNextChance = DateTime.UtcNow + TimeSpan.FromDays( 1.0 );
		}

		public static void OnQuestionsPassed( Mobile from )
		{
			from.PlaySound( 0x5B5 );
			from.PlaySound( 0x41A );

			/* Very good! I can see that ye hath more than just a passing interest in
			 * our work. There are many trials before thee, but I have every hope that
			 * ye shall have the diligence and fortitude to carry on to the very end.
			 * Before we begin, please prepare thyself by thinking about the virtue of
			 * Humility. Ponder not only its symbols, but also its meanings. Once ye
			 * believe that thou art ready, speak with me again. */
			from.SendGump( new GenericQuestGump( 1075714 ) );

			if ( from is PlayerMobile )
				( (PlayerMobile) from ).HumilityQuestStatus = HumilityQuestStatus.QuestChain;
		}
	}
}