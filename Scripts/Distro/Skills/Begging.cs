using System;
using Server;
using Server.Misc;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.SkillHandlers
{
	public class Begging
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int) SkillName.Begging].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.RevealingAction();

			m.Target = new InternalTarget();
			m.RevealingAction();

			m.SendLocalizedMessage( 500397 ); // To whom do you wish to grovel?

			return TimeSpan.FromHours( 6.0 );
		}

		private class InternalTarget : Target
		{
			private bool m_SetSkillTime = true;

			public InternalTarget()
				: base( 12, false, TargetFlags.None )
			{
			}

			protected override void OnTargetFinish( Mobile from )
			{
				if ( m_SetSkillTime )
					from.NextSkillTime = DateTime.UtcNow;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				from.RevealingAction();

				int number = -1;

				if ( targeted is Mobile )
				{
					Mobile targ = (Mobile) targeted;

					if ( targ.Player ) // We can't beg from players
					{
						number = 500398; // Perhaps just asking would work better.
					}
					else if ( !targ.Body.IsHuman ) // Make sure the NPC is human or elf
					{
						number = 500399; // There is little chance of getting money from that!
					}
					else if ( !from.InRange( targ, 2 ) )
					{
						if ( !targ.Female )
							number = 500401; // You are too far away to beg from him.
						else
							number = 500402; // You are too far away to beg from her.
					}
					else if ( from.Mounted ) // If we're on a mount, who would give us money?
					{
						number = 500404; // They seem unwilling to give you any money.
					}
					else
					{
						// Face eachother
						from.Direction = from.GetDirectionTo( targ );
						targ.Direction = targ.GetDirectionTo( from );

						from.Animate( 32, 5, 1, true, false, 0 ); // Bow

						new InternalTimer( from, targ ).Start();

						m_SetSkillTime = false;
					}
				}
				else // Not a Mobile
				{
					number = 500399; // There is little chance of getting money from that!
				}

				if ( number != -1 )
					from.SendLocalizedMessage( number );
			}

			private class InternalTimer : Timer
			{
				private Mobile m_From, m_Target;

				public InternalTimer( Mobile from, Mobile target )
					: base( TimeSpan.FromSeconds( 2.0 ) )
				{
					m_From = from;
					m_Target = target;
				}

				protected override void OnTick()
				{
					Container theirPack = m_Target.Backpack;

					double badKarmaChance = 0.5 - ( (double) m_From.Karma / 8570 );

					if ( theirPack == null )
					{
						m_From.SendLocalizedMessage( 500404 ); // They seem unwilling to give you any money.
					}
					else if ( m_From.Karma < 0 && badKarmaChance > Utility.RandomDouble() )
					{
						m_Target.PublicOverheadMessage( MessageType.Regular, m_Target.SpeechHue, 500406 ); // Thou dost not look trustworthy... no gold for thee today!
					}
					else if ( m_From.CheckTargetSkill( SkillName.Begging, m_Target, 0.0, 100.0 ) )
					{
						if ( m_Target.Race == Race.Elf )
						{
							Item item = null;
							string name = string.Empty;
							double random = Utility.RandomDouble();

							if ( 0.01 > random )
							{
								switch ( Utility.RandomMinMax( 1, 8 ) )
								{
									case 1:
										item = new Begging_Bedroll();
										name = "a bedroll.";
										break;
									case 2:
										item = new Begging_CookieMix();
										name = "a plate of cookies.";
										break;
									case 3:
										item = new Begging_FishingPole();
										name = "a fishing pole.";
										break;
									case 4:
										item = new Begging_FishSteak();
										name = "a fish steak.";
										break;
									case 5:
										item = new Begging_FlowerGarland();
										name = "a flower garland.";
										break;
									case 6:
										item = new Begging_PitcherOfWine1();
										name = "a pitcher of wine.";
										break;
									case 7:
										item = new Begging_PitcherOfWine2();
										name = "a pitcher of wine.";
										break;
									case 8:
										item = new Begging_Sake();
										name = "a bottle of sake.";
										break;
									case 9:
										item = new Begging_Turnip();
										name = "a turnip.";
										break;
								}
							}
							else if ( 0.24 > random )
							{
								switch ( Utility.RandomMinMax( 1, 8 ) )
								{
									case 1:
										item = new Begging_BowlOfStew();
										name = "a bowl of stew.";
										break;
									case 2:
										item = new Begging_CheeseWedge();
										name = "a wedge of cheese.";
										break;
									case 3:
										item = new Begging_Dates();
										name = "a bunch of dates.";
										break;
									case 4:
										item = new Begging_Lantern();
										name = "a lantern.";
										break;
									case 5:
										item = new Begging_PitcherOfLiquor1();
										name = "a pitcher of liquor.";
										break;
									case 6:
										item = new Begging_PitcherOfLiquor2();
										name = "a pitcher of liquor.";
										break;
									case 7:
										item = new Begging_Shirt();
										name = "a shirt.";
										break;
									case 8:
										item = new Begging_Pizza();
										name = "a pizza.";
										break;
								}
							}
							else
							{
								switch ( Utility.RandomMinMax( 1, 3 ) )
								{
									case 1:
										item = new Begging_FrenchBread();
										name = "a french bread.";
										break;
									case 2:
										item = new Begging_PitcherOfWater1();
										name = "a pitcher of water.";
										break;
									case 3:
										item = new Begging_PitcherOfWater2();
										name = "a pitcher of water.";
										break;
								}
							}

							m_Target.Say( 1074854 ); // Here, take this...

							m_From.AddToBackpack( item );
							m_From.SendLocalizedMessage( 1074853, name ); // You have been given ~1_name~

							if ( m_From.Karma > -3000 )
							{
								int toLose = m_From.Karma + 3000;

								Utility.FixMax( ref toLose, 40 );

								Titles.AwardKarma( m_From, -toLose, true );
							}
						}
						else
						{
							int toConsume = theirPack.GetAmount( typeof( Gold ) ) / 10;
							int max = 10 + ( m_From.Fame / 2500 );

							Utility.FixMinMax( ref max, 10, 14 );
							Utility.FixMax( ref toConsume, max );

							if ( toConsume > 0 )
							{
								int consumed = theirPack.ConsumeUpTo( typeof( Gold ), toConsume );

								if ( consumed > 0 )
								{
									m_Target.PublicOverheadMessage( MessageType.Regular, m_Target.SpeechHue, 500405 ); // I feel sorry for thee...

									Gold gold = new Gold( consumed );

									m_From.AddToBackpack( gold );
									m_From.PlaySound( gold.GetDropSound() );

									if ( m_From.Karma > -3000 )
									{
										int toLose = m_From.Karma + 3000;

										Utility.FixMax( ref toLose, 40 );

										Titles.AwardKarma( m_From, -toLose, true );
									}
								}
								else
								{
									m_Target.PublicOverheadMessage( MessageType.Regular, m_Target.SpeechHue, 500407 ); // I have not enough money to give thee any!
								}
							}
							else
							{
								m_Target.PublicOverheadMessage( MessageType.Regular, m_Target.SpeechHue, 500407 ); // I have not enough money to give thee any!
							}
						}
					}
					else
					{
						m_Target.SendLocalizedMessage( 500404 ); // They seem unwilling to give you any money.
					}

					m_From.NextSkillTime = DateTime.UtcNow + TimeSpan.FromSeconds( 10.0 );
				}
			}
		}
	}
}