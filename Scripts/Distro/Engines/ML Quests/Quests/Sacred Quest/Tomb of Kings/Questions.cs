using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests.SacredQuest
{
	public class Questions
	{
		private static QuestionDefinition[] m_Definitions = new QuestionDefinition[]
			{
				new QuestionDefinition
					(
						1112601, // Which of these is a Principle?
						new AnswerDefinition( 1112656, true ), // Control
						new AnswerDefinition( 1112661, false ), // Persistence
						new AnswerDefinition( 1112664, false ), // Precision
						new AnswerDefinition( 1112660, false ) // Feeling
					),
				new QuestionDefinition
					(
						1112602, // From what Principle does Direction spring?
						new AnswerDefinition( 1112656, true ), // Control
						new AnswerDefinition( 1112662, false ), // Balance
						new AnswerDefinition( 1112658, false ), // Diligence
						new AnswerDefinition( 1112657, false ) // Passion
					),
				new QuestionDefinition
					(
						1112603, // From Passion springs which Virtue?
						new AnswerDefinition( 1112660, true ), // Feeling
						new AnswerDefinition( 1112661, false ), // Persistence
						new AnswerDefinition( 1112663, false ), // Achievement
						new AnswerDefinition( 1112656, false ) // Control
					),
				new QuestionDefinition
					(
						1112604, // From Diligence springs which Virtue?
						new AnswerDefinition( 1112661, true ), // Persistence
						new AnswerDefinition( 1112667, false ), // Singularity
						new AnswerDefinition( 1112656, false ), // Control
						new AnswerDefinition( 1112666, false ) // Order
					),
				new QuestionDefinition
					(
						1112605, // Is any Virtue more important than another?
						new AnswerDefinition( 1112669, true ), // No
						new AnswerDefinition( 1112652, false ), // Spirituality is the most important
						new AnswerDefinition( 1112645, false ), // All but chaos are important
						new AnswerDefinition( 1112644, false ) // Order is more important
					),
				new QuestionDefinition
					(
						1112606, // Are each of the Virtues considered to be equal?
						new AnswerDefinition( 1112649, true ), // All are equal
						new AnswerDefinition( 1112646, false ), // Singularity is more imporant than all others
						new AnswerDefinition( 1112669, false ), // No
						new AnswerDefinition( 1112644, false ) // Order is more important
					),
				new QuestionDefinition
					(
						1112607, // Amongst all else, of how many Virtues does the Circle consist?
						new AnswerDefinition( 1112668, true ), // Eight
						new AnswerDefinition( 1112655, false ), // Twelve
						new AnswerDefinition( 1112653, false ), // Seven
						new AnswerDefinition( 1112654, false ) // Ten
					)
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

			/* You realize that is not the correct answer.
			 * 
			 * You vow to study the Book of Circles again so that you might
			 * understand all that is required of you. Perhaps meditating
			 * again soon will bring the wisdom that you seek. */
			from.SendGump( new GenericQuestGump( 1112680 ) );

			if ( from is PlayerMobile )
				( (PlayerMobile) from ).SacredQuestNextChance = DateTime.Now + TimeSpan.FromDays( 1.0 );
		}

		public static void OnQuestionsPassed( Mobile from )
		{
			from.PlaySound( 0x5B5 );
			from.PlaySound( 0x41A );

			/* Answering the last question correctly, you feel a strange energy
			 * wash over you.
			 * 
			 * You don't understand how you know, but you are absolutely certain
			 * that the guardians will no longer bar you from entering the
			 * Stygian Abyss.
			 * 
			 * It seems you have proven yourself worthy of La Insep Om. */
			from.SendGump( new GenericQuestGump( 1112700 ) );

			if ( from is PlayerMobile )
				( (PlayerMobile) from ).SacredQuest = true;
		}
	}
}