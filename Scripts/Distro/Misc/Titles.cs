using System;
using System.Text;
using Server;
using Server.Mobiles;

namespace Server.Misc
{
	public class Titles
	{
		public const int MinFame = 0;
		public const int MaxFame = 15000;

		public static void AwardFame( Mobile m, int offset, bool message )
		{
			if ( m is PlayerMobile && ( (PlayerMobile) m ).BlocksFameAward )
				return;

			if ( offset > 0 )
			{
				if ( m.Fame >= MaxFame )
					return;

				offset -= m.Fame / 100;

				if ( offset < 0 )
					offset = 0;
			}
			else if ( offset < 0 )
			{
				if ( m.Fame <= MinFame )
					return;

				offset -= m.Fame / 100;

				if ( offset > 0 )
					offset = 0;
			}

			if ( ( m.Fame + offset ) > MaxFame )
				offset = MaxFame - m.Fame;
			else if ( ( m.Fame + offset ) < MinFame )
				offset = MinFame - m.Fame;

			m.Fame += offset;

			if ( message )
			{
				if ( offset > 40 )
					m.SendLocalizedMessage( 1019054 ); // You have gained a lot of fame.
				else if ( offset > 20 )
					m.SendLocalizedMessage( 1019053 ); // You have gained a good amount of fame.
				else if ( offset > 10 )
					m.SendLocalizedMessage( 1019052 ); // You have gained some fame.
				else if ( offset > 0 )
					m.SendLocalizedMessage( 1019051 ); // You have gained a little fame.
				else if ( offset < -40 )
					m.SendLocalizedMessage( 1019058 ); // You have lost a lot of fame.
				else if ( offset < -20 )
					m.SendLocalizedMessage( 1019057 ); // You have lost a good amount of fame.
				else if ( offset < -10 )
					m.SendLocalizedMessage( 1019056 ); // You have lost some fame.
				else if ( offset < 0 )
					m.SendLocalizedMessage( 1019055 ); // You have lost a little fame.
			}
		}

		public const int MinKarma = -15000;
		public const int MaxKarma = 15000;

		public static void AwardKarma( Mobile m, int offset, bool message )
		{
			if ( offset > 0 )
			{
				if ( m is PlayerMobile && ( (PlayerMobile) m ).KarmaLocked )
					return;

				if ( m.Karma >= MaxKarma )
					return;

				offset -= m.Karma / 100;

				if ( offset < 0 )
					offset = 0;
			}
			else if ( offset < 0 )
			{
				if ( m.Karma <= MinKarma )
					return;

				offset -= m.Karma / 100;

				if ( offset > 0 )
					offset = 0;
			}

			if ( ( m.Karma + offset ) > MaxKarma )
				offset = MaxKarma - m.Karma;
			else if ( ( m.Karma + offset ) < MinKarma )
				offset = MinKarma - m.Karma;

			bool wasPositiveKarma = ( m.Karma >= 0 );

			m.Karma += offset;

			if ( message )
			{
				if ( offset > 40 )
					m.SendLocalizedMessage( 1019062 ); // You have gained a lot of karma.
				else if ( offset > 20 )
					m.SendLocalizedMessage( 1019061 ); // You have gained a good amount of karma.
				else if ( offset > 10 )
					m.SendLocalizedMessage( 1019060 ); // You have gained some karma.
				else if ( offset > 0 )
					m.SendLocalizedMessage( 1019059 ); // You have gained a little karma.
				else if ( offset < -40 )
					m.SendLocalizedMessage( 1019066 ); // You have lost a lot of karma.
				else if ( offset < -20 )
					m.SendLocalizedMessage( 1019065 ); // You have lost a good amount of karma.
				else if ( offset < -10 )
					m.SendLocalizedMessage( 1019064 ); // You have lost some karma.
				else if ( offset < 0 )
					m.SendLocalizedMessage( 1019063 ); // You have lost a little karma.
			}

			if ( wasPositiveKarma && m.Karma < 0 && m is PlayerMobile && !( (PlayerMobile) m ).KarmaLocked )
			{
				( (PlayerMobile) m ).KarmaLocked = true;
				m.SendLocalizedMessage( 1042511, "", 0x22 ); // Karma is locked.  A mantra spoken at a shrine will unlock it again.
			}
		}

		public static int FindMaxGroup( double[] ChampionTiers )
		{
			int result = 0;
			double score = ChampionTiers[0];

			for ( int i = 1; i < ChampionTiers.Length; i++ )
			{
				if ( ChampionTiers[i] > score )
				{
					score = ChampionTiers[i];

					result = i;
				}
			}

			return result;
		}

		public static string FindTitle( double scores, int type )
		{
			string title = null;

			switch ( type )
			{
				case 0:
					{
						// Cold Blood
						if ( scores >= 1 && scores <= 10 )
							title = "Blight of the Cold Blood";
						else if ( scores > 10 && scores <= 20 )
							title = "Slayer of the Cold Blood";
						else if ( scores > 20 )
							title = "Destroyer of the Cold Blood";

						break;
					}
				case 1:
					{
						// Forest Lord
						if ( scores >= 1 && scores <= 10 )
							title = "Enemy of the Forest Lord";
						else if ( scores > 10 && scores <= 20 )
							title = "Curse of the Forest Lord";
						else if ( scores > 20 )
							title = "Slaughterer of the Forest Lord";

						break;
					}
				case 2:
					{
						// Arachnid
						if ( scores >= 1 && scores <= 10 )
							title = "Bane of the Arachnid";
						else if ( scores > 10 && scores <= 20 )
							title = "Killer of the Arachnid";
						else if ( scores > 20 )
							title = "Vanquisher of the Arachnid";

						break;
					}
				case 3:
					{
						// Abyss
						if ( scores >= 1 && scores <= 10 )
							title = "Foe of the Abyss";
						else if ( scores > 10 && scores <= 20 )
							title = "Assassin of the Abyss";
						else if ( scores > 20 )
							title = "Conqueror of the Abyss";

						break;
					}
				case 4:
					{
						// Vermin Horde
						if ( scores >= 1 && scores <= 10 )
							title = "Adversary of the Vermin Horde";
						else if ( scores > 10 && scores <= 20 )
							title = "Subjugator of the Vermin Horde";
						else if ( scores > 20 )
							title = "Eradicator of the Vermin Horde";

						break;
					}
				case 5:
					{
						// Unholy Terror
						if ( scores >= 1 && scores <= 10 )
							title = "Scourge of the Unholy Terror";
						else if ( scores > 10 && scores <= 20 )
							title = "Punisher of the Unholy Terror";
						else if ( scores > 20 )
							title = "Nemesis of the Unholy Terror";

						break;
					}
				case 6:
					{
						// Sleeping Dragon
						if ( scores >= 1 && scores <= 10 )
							title = "Rival of the Sleeping Dragon";
						else if ( scores > 10 && scores <= 20 )
							title = "Challenger of the Sleeping Dragon";
						else if ( scores > 20 )
							title = "Antagonist of the Sleeping Dragon";

						break;
					}
				case 7:
					{
						// Corrupt
						if ( scores >= 1 && scores <= 10 )
							title = "Cleanser of the Corrupt";
						else if ( scores > 10 && scores <= 20 )
							title = "Expunger of the Corrupt";
						else if ( scores > 20 )
							title = "Depurator of the Corrupt";

						break;
					}
				case 8:
					{
						// Glade
						if ( scores >= 1 && scores <= 10 )
							title = "Banisher of the Glade";
						else if ( scores > 10 && scores <= 20 )
							title = "Enforcer of the Glade";
						else if ( scores > 20 )
							title = "Eradicator of the Glade";

						break;
					}
				case 9:
					{
						// Unliving
						if ( scores >= 1 && scores <= 10 )
							title = "Despair of the Unliving";
						else if ( scores > 10 && scores <= 20 )
							title = "Curse of the Unliving";
						else if ( scores > 20 )
							title = "Woe of the Unliving";

						break;
					}
				case 10:
					{
						// Pit
						if ( scores >= 1 && scores <= 10 )
							title = "Agony of the Pit";
						else if ( scores > 10 && scores <= 20 )
							title = "Torment of the Pit";
						else if ( scores > 20 )
							title = "Havoc of the Pit";

						break;
					}
			}

			return title;
		}

		public static string GetChampionTitle( Mobile beheld )
		{
			string title = null;

			PlayerMobile pm = beheld as PlayerMobile;

			if ( pm != null )
			{
				if ( pm.SuperChampionTiers[0] > 0 ) // player killed harrower at least one time
				{
					int count = 0;

					for ( int i = 1; i < pm.SuperChampionTiers.Length; i++ )
					{
						if ( pm.SuperChampionTiers[i] > 0 ) // player killed this champion at least one time
							count++;
					}

					switch ( count )
					{
						case 0:
							title = "Spite of Evil";
							break;
						case 1:
							title = "Opponent of Evil";
							break;
						case 2:
							title = "Hunter of Evil";
							break;
						case 3:
							title = "Venom of Evil";
							break;
						case 4:
							title = "Executioner of Evil";
							break;
						case 5:
							title = "Annihilator of Evil";
							break;
						case 6:
							title = "Champion of Evil";
							break;
						case 7:
							title = "Assailant of Evil";
							break;
						case 8:
							title = "Purifier of Evil";
							break;
						case 9:
							title = "Nullifier of Evil";
							break;
					}
				}
				else
				{
					int type = FindMaxGroup( pm.ChampionTiers ); // we find group of champions with maximum scores			   

					double scores = pm.ChampionTiers[type];

					title = FindTitle( scores, type ); // we find title in our group depend on scores
				}
			}

			return title;
		}

		public static string ComputeTitle( Mobile beholder, Mobile beheld )
		{
			StringBuilder title = new StringBuilder();

			int fame = beheld.Fame;
			int karma = beheld.Karma;

			bool showSkillTitle = beheld.ShowFameTitle && ( ( beholder == beheld ) || ( fame >= 5000 ) );

			if ( beheld.Murderer )
			{
				title.AppendFormat( beheld.Fame >= 10000 ? "The Murderer {1} {0}" : "The Murderer {0}", beheld.Name, beheld.Female ? "Lady" : "Lord" );
			}
			else if ( beheld.ShowFameTitle || ( beholder == beheld ) )
			{
				for ( int i = 0; i < m_FameEntries.Length; ++i )
				{
					FameEntry fe = m_FameEntries[i];

					if ( fame <= fe.m_Fame || i == ( m_FameEntries.Length - 1 ) )
					{
						KarmaEntry[] karmaEntries = fe.m_Karma;

						for ( int j = 0; j < karmaEntries.Length; ++j )
						{
							KarmaEntry ke = karmaEntries[j];

							if ( karma <= ke.m_Karma || j == ( karmaEntries.Length - 1 ) )
							{
								title.AppendFormat( ke.m_Title, beheld.Name, beheld.Female ? "Lady" : "Lord" );
								break;
							}
						}

						break;
					}
				}
			}
			else
			{
				title.Append( beheld.Name );
			}

			#region Champion Monster Titles
			PlayerMobile pm = beheld as PlayerMobile;

			string championTitle = GetChampionTitle( pm );

			if ( pm != null && pm.DisplayChampionTitle && championTitle != null )
			{
				title.AppendFormat( ": {0}", championTitle );
			}
			#endregion

			string customTitle = beheld.Title;

			if ( customTitle != null && ( customTitle = customTitle.Trim() ).Length > 0 )
			{
				title.AppendFormat( " {0}", customTitle );
			}
			else if ( showSkillTitle && beheld.Player )
			{
				Skill highest = GetHighestSkill( beheld ); // beheld.Skills.Highest;

				if ( highest != null && highest.BaseFixedPoint >= 300 )
				{
					string skillLevel = (string) Utility.GetArrayCap( m_Levels, ( highest.BaseFixedPoint - 300 ) / 100 );

					if ( highest.Info.SkillId == 0x34 )
					{
						skillLevel = (string) Utility.GetArrayCap( m_Levels_Samurai, ( highest.BaseFixedPoint - 300 ) / 100 );
					}

					if ( highest.Info.SkillId == 0x35 )
					{
						skillLevel = (string) Utility.GetArrayCap( m_Levels_Ninja, ( highest.BaseFixedPoint - 300 ) / 100 );
					}

					string skillTitle = highest.Info.Title;

					if ( beheld.Female )
					{
						if ( skillTitle.EndsWith( "man" ) )
						{
							skillTitle = skillTitle.Substring( 0, skillTitle.Length - 3 ) + "woman";
						}
					}

					title.AppendFormat( ", {0} {1}", skillLevel, skillTitle );
				}
			}

			return title.ToString();
		}

		private static Skill GetHighestSkill( Mobile m )
		{
			Skills skills = m.Skills;
			Skill highest = null;

			for ( int i = 0; i < m.Skills.Length; ++i )
			{
				Skill check = m.Skills[i];

				if ( highest == null || check.BaseFixedPoint > highest.BaseFixedPoint )
					highest = check;
				else if ( highest.Lock != SkillLock.Up && check.Lock == SkillLock.Up && check.BaseFixedPoint == highest.BaseFixedPoint )
					highest = check;
			}

			return highest;
		}

		private static string[] m_Levels = new string[]
			{
				"Neophyte",
				"Novice",
				"Apprentice",
				"Journeyman",
				"Expert",
				"Adept",
				"Master",
				"Grandmaster",
				"Elder",
				"Legendary"
			};

		private static string[] m_Levels_Ninja = new string[]
			{
				"Neophyte",
				"Novice",
				"Apprentice",
				"Journeyman",
				"Expert",
				"Adept",
				"Master",
				"Grandmaster",
				"Shinobi",
				"Ka-ge"
			};

		private static string[] m_Levels_Samurai = new string[]
			{
				"Neophyte",
				"Novice",
				"Apprentice",
				"Journeyman",
				"Expert",
				"Adept",
				"Master",
				"Grandmaster",
				"Tatsujin",
				"Kengo"
			};

		private static FameEntry[] m_FameEntries = new FameEntry[]
			{
				new FameEntry( 1249, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Outcast {0}" ),
					new KarmaEntry( -5000, "The Despicable {0}" ),
					new KarmaEntry( -2500, "The Scoundrel {0}" ),
					new KarmaEntry( -1250, "The Unsavory {0}" ),
					new KarmaEntry( -625, "The Rude {0}" ),
					new KarmaEntry( 624, "{0}" ),
					new KarmaEntry( 1249, "The Fair {0}" ),
					new KarmaEntry( 2499, "The Kind {0}" ),
					new KarmaEntry( 4999, "The Good {0}" ),
					new KarmaEntry( 9999, "The Honest {0}" ),
					new KarmaEntry( 10000, "The Trustworthy {0}" )
				} ),
				new FameEntry( 2499, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Wretched {0}" ),
					new KarmaEntry( -5000, "The Dastardly {0}" ),
					new KarmaEntry( -2500, "The Malicious {0}" ),
					new KarmaEntry( -1250, "The Dishonorable {0}" ),
					new KarmaEntry( -625, "The Disreputable {0}" ),
					new KarmaEntry( 624, "The Notable {0}" ),
					new KarmaEntry( 1249, "The Upstanding {0}" ),
					new KarmaEntry( 2499, "The Respectable {0}" ),
					new KarmaEntry( 4999, "The Honorable {0}" ),
					new KarmaEntry( 9999, "The Commendable {0}" ),
					new KarmaEntry( 10000, "The Estimable {0}" )
				} ),
				new FameEntry( 4999, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Nefarious {0}" ),
					new KarmaEntry( -5000, "The Wicked {0}" ),
					new KarmaEntry( -2500, "The Vile {0}" ),
					new KarmaEntry( -1250, "The Ignoble {0}" ),
					new KarmaEntry( -625, "The Notorious {0}" ),
					new KarmaEntry( 624, "The Prominent {0}" ),
					new KarmaEntry( 1249, "The Reputable {0}" ),
					new KarmaEntry( 2499, "The Proper {0}" ),
					new KarmaEntry( 4999, "The Admirable {0}" ),
					new KarmaEntry( 9999, "The Famed {0}" ),
					new KarmaEntry( 10000, "The Great {0}" )
				} ),
				new FameEntry( 9999, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Dread {0}" ),
					new KarmaEntry( -5000, "The Evil {0}" ),
					new KarmaEntry( -2500, "The Villainous {0}" ),
					new KarmaEntry( -1250, "The Sinister {0}" ),
					new KarmaEntry( -625, "The Infamous {0}" ),
					new KarmaEntry( 624, "The Renowned {0}" ),
					new KarmaEntry( 1249, "The Distinguished {0}" ),
					new KarmaEntry( 2499, "The Eminent {0}" ),
					new KarmaEntry( 4999, "The Noble {0}" ),
					new KarmaEntry( 9999, "The Illustrious {0}" ),
					new KarmaEntry( 10000, "The Glorious {0}" )
				} ),
				new FameEntry( 10000, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "The Dread {1} {0}" ),
					new KarmaEntry( -5000, "The Evil {1} {0}" ),
					new KarmaEntry( -2500, "The Dark {1} {0}" ),
					new KarmaEntry( -1250, "The Sinister {1} {0}" ),
					new KarmaEntry( -625, "The Dishonored {1} {0}" ),
					new KarmaEntry( 624, "{1} {0}" ),
					new KarmaEntry( 1249, "The Distinguished {1} {0}" ),
					new KarmaEntry( 2499, "The Eminent {1} {0}" ),
					new KarmaEntry( 4999, "The Noble {1} {0}" ),
					new KarmaEntry( 9999, "The Illustrious {1} {0}" ),
					new KarmaEntry( 10000, "The Glorious {1} {0}" )
				} )
			};
	}

	public class FameEntry
	{
		public int m_Fame;
		public KarmaEntry[] m_Karma;

		public FameEntry( int fame, KarmaEntry[] karma )
		{
			m_Fame = fame;
			m_Karma = karma;
		}
	}

	public class KarmaEntry
	{
		public int m_Karma;
		public string m_Title;

		public KarmaEntry( int karma, string title )
		{
			m_Karma = karma;
			m_Title = title;
		}
	}
}