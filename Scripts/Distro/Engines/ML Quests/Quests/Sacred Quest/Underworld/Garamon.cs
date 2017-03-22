using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Events;

namespace Server.Mobiles
{
	public class Garamon : Mobile
	{
		private DateTime m_NextTalk;

		public Garamon()
		{
			Name = "Garamon";
			Race = Race.Human;

			HairItemID = 0x2044; // Mohawk
			HairHue = 0x8FD;
			FacialHairItemID = 0x204B; // Medium Short Beard
			FacialHairHue = 0x8FD;

			Hue = 0x83EA;

			Hits = HitsMax;

			Blessed = true;
			Frozen = true;

			AddItem( new Shoes( 0x756 ) );
			AddItem( new LongPants( 0x1BB ) );
			AddItem( new Robe( 0x3B3 ) );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m.Alive && m.Player && !m.Hidden && this.InRange( m, 3 ) && !this.InRange( oldLocation, 3 ) )
				PublicOverheadMessage( MessageType.Regular, 0x24E, true, "Greetings Adventurer!  If you are seeking to enter the Abyss, I may be of assistance to you." );
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			return DateTime.UtcNow > m_NextTalk && from.InRange( Location, 2 );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			string speech = e.Speech.ToLower();

			for ( int i = 0; i < m_Keywords.Length; i++ )
			{
				if ( speech.Contains( m_Keywords[i].Keyword ) )
				{
					m_NextTalk = DateTime.UtcNow + TimeSpan.FromSeconds( 10.0 );

					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback(
						delegate
						{
							Direction = this.GetDirectionTo( e.Mobile );
							Say( m_Keywords[i].Answer );
						}
					) );

					return;
				}
			}
		}

		private static KeywordDefinition[] m_Keywords = new KeywordDefinition[]
			{
				new KeywordDefinition( "abyss", "Its entrance is protected by stone guardians who will only grant passage to the carrier of a Tripartite Key." ),
				new KeywordDefinition( "burn", "I can tell you right away it's not fire based. Surely something within the dungeon will yield what you need." ),
				new KeywordDefinition( "guardian", "They will not let you enter the Abyss unless you can present a Tripartite Key." ),
				new KeywordDefinition( "key", "Its three parts you must find and reunite as one." ),
				new KeywordDefinition( "secret", "He who pays close attention to the walls may notice something unusual." ),
				new KeywordDefinition( "shadow", "A most foul traitor. Once you have the first two parts, challenge him for the third! He dwells beyond the void in the Shrine." ),
				new KeywordDefinition( "tripartite", "Its three parts you must find and reunite as one." ),
				new KeywordDefinition( "vines", "Aaah yes! Tricky things they are. Try to find something that could burn through them." )
			};

		public Garamon( Serial serial )
			: base( serial )
		{
		}

		public override bool CanBeDamaged()
		{
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}

		private class KeywordDefinition
		{
			private string m_Keyword, m_Answer;

			public string Keyword { get { return m_Keyword; } }
			public string Answer { get { return m_Answer; } }

			public KeywordDefinition( string keyword, string answer )
			{
				m_Keyword = keyword;
				m_Answer = answer;
			}
		}
	}
}
