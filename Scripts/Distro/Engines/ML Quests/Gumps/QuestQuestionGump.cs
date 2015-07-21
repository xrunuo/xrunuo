using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
	public delegate void QuestQuestionEvent( Mobile m );

	public class QuestQuestionGump : Gump
	{
		private int m_CurrentQuestion;
		private QuestionDefinition[] m_Questions;
		private QuestQuestionEvent m_OnPassed, m_OnFailed;

		public QuestQuestionGump( QuestionDefinition[] questions, int currentQuestion, QuestQuestionEvent onPassed, QuestQuestionEvent onFailed )
			: base( 160, 100 )
		{
			m_Questions = questions;
			m_CurrentQuestion = currentQuestion;
			m_OnPassed = onPassed;
			m_OnFailed = onFailed;

			QuestionDefinition question = m_Questions[m_CurrentQuestion];
			AnswerDefinition[] answers = Utility.Shuffle( question.Answers );

			Disposable = false;
			Closable = false;

			AddImage( 0, 0, 0x4CC );
			AddImage( 40, 58, 0x5F );
			AddImageTiled( 49, 67, 301, 3, 0x60 );
			AddImage( 350, 58, 0x61 );
			AddImage( 50, 85, 0x8B0 );
			AddImage( 50, 125, 0x8B0 );
			AddImage( 50, 165, 0x8B0 );
			AddImage( 50, 205, 0x8B0 );

			AddHtmlLocalized( 30, 40, 340, 30, question.QuestionCliloc, 0x0, false, false );

			for ( int i = 0; i < answers.Length; i++ )
			{
				AnswerDefinition answer = answers[i];

				AddButton( 49, 84 + ( i * 40 ), 0x845, 0x846, answer.Correct ? 1 : 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 82 + ( i * 40 ), 275, 36, answer.Cliloc, 0x0, false, false );
			}
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if ( info.ButtonID == 1 )
			{
				m_CurrentQuestion++;

				if ( m_CurrentQuestion >= m_Questions.Length )
					m_OnPassed( from );
				else
					from.SendGump( new QuestQuestionGump( m_Questions, m_CurrentQuestion, m_OnPassed, m_OnFailed ) );
			}
			else
				m_OnFailed( from );
		}
	}

	public class QuestionDefinition
	{
		private int m_QuestionCliloc;
		private AnswerDefinition[] m_Answers;

		public int QuestionCliloc
		{
			get { return m_QuestionCliloc; }
			set { m_QuestionCliloc = value; }
		}

		public AnswerDefinition[] Answers
		{
			get { return m_Answers; }
			set { m_Answers = value; }
		}

		public QuestionDefinition( int question, params AnswerDefinition[] answers )
		{
			m_QuestionCliloc = question;
			m_Answers = answers;
		}
	}

	public class AnswerDefinition
	{
		private int m_Cliloc;
		private bool m_Correct;

		public int Cliloc
		{
			get { return m_Cliloc; }
			set { m_Cliloc = value; }
		}

		public bool Correct
		{
			get { return m_Correct; }
			set { m_Correct = value; }
		}

		public AnswerDefinition( int cliloc, bool correct )
		{
			m_Cliloc = cliloc;
			m_Correct = correct;
		}
	}
}