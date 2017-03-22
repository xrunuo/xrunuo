using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class Bandage : Item, IDyable
	{
		public static readonly int Range = 2;

		[Constructable]
		public Bandage()
			: this( 1 )
		{
		}

		[Constructable]
		public Bandage( int amount )
			: base( 0xE21 )
		{
			Stackable = true;
			Weight = 0.1;
			Amount = amount;
		}

		public Bandage( Serial serial )
			: base( serial )
		{
		}

		public virtual int HealingBonus { get { return 0; } }

		public virtual bool Dye( Mobile from, IDyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), Range ) )
			{
				from.RevealingAction();

				from.SendLocalizedMessage( 500948 ); // Who will you use the bandages on?
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
			}
		}

		private class InternalTarget : Target
		{
			private Bandage m_Bandage;

			public InternalTarget( Bandage bandage )
				: base( Bandage.Range, false, TargetFlags.Beneficial )
			{
				m_Bandage = bandage;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Bandage.Deleted )
					return;

				if ( targeted is Mobile )
				{
					if ( from.InRange( m_Bandage.GetWorldLocation(), Range ) )
					{
						BandageContext context;

						if ( ( context = BandageContext.BeginHeal( from, (Mobile) targeted, m_Bandage ) ) != null )
						{
							m_Bandage.OnHealStarted( context );
							m_Bandage.Consume();
						}
					}
					else
					{
						from.SendLocalizedMessage( 500295 ); // You are too far away to do that.
					}
				}
				else
				{
					from.SendLocalizedMessage( 500970 ); // Bandages can not be used on that.
				}
			}
		}

		public virtual void OnHealStarted( BandageContext context )
		{
		}

		public virtual void OnHealFinished( BandageContext context )
		{
		}

		public virtual bool AttemptsMidlifeCure { get { return true; } }
	}

	public class BandageContext
	{
		private Mobile m_Healer;
		private Mobile m_Patient;
		private Bandage m_Bandage;
		private int m_Slips;
		private TimeSpan m_Delay;
		private Timer m_Timer;
		private int m_AmountDivisor;

		public Mobile Healer { get { return m_Healer; } }
		public Mobile Patient { get { return m_Patient; } }
		public Bandage Bandage { get { return m_Bandage; } }
		public int Slips { get { return m_Slips; } set { m_Slips = value; } }
		public TimeSpan Delay { get { return m_Delay; } }
		public Timer Timer { get { return m_Timer; } }

		public void Slip()
		{
			m_Healer.SendLocalizedMessage( 500961 ); // Your fingers slip!
			++m_Slips;
		}

		public BandageContext( Mobile healer, Mobile patient, TimeSpan delay, Bandage bandage )
		{
			m_Healer = healer;
			m_Patient = patient;
			m_Bandage = bandage;
			m_Delay = delay;

			m_Timer = new InternalTimer( this, delay );
			m_Timer.Start();
		}

		public void RefreshTimer( TimeSpan newDelay )
		{
			m_Delay = newDelay;

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = new InternalTimer( this, newDelay );
			m_Timer.Start();
		}

		private static Hashtable m_Table = new Hashtable();

		public static BandageContext GetContext( Mobile healer )
		{
			return (BandageContext) m_Table[healer];
		}

		public static SkillName GetPrimarySkill( Mobile m, Mobile n )
		{
			if ( m is CuSidhe && n is CuSidhe )
				return SkillName.Healing;
			else if ( !m.Player && ( m.Body.IsMonster || m.Body.IsAnimal ) )
				return SkillName.Veterinary;
			else
				return SkillName.Healing;
		}

		public static SkillName GetSecondarySkill( Mobile m, Mobile n )
		{
			if ( m is CuSidhe && n is CuSidhe )
				return SkillName.Healing;
			else if ( !m.Player && ( m.Body.IsMonster || m.Body.IsAnimal ) )
				return SkillName.AnimalLore;
			else
				return SkillName.Anatomy;
		}

		public void AttemptMidlifeCure()
		{
			if ( m_Bandage == null || !m_Bandage.AttemptsMidlifeCure )
				return;

			if ( m_Healer != m_Patient || !m_Healer.Alive || !m_Healer.Player )
				return;

			SkillName primarySkill = GetPrimarySkill( m_Patient, m_Healer );
			SkillName secondarySkill = GetSecondarySkill( m_Patient, m_Healer );

			double healing = m_Healer.Skills[primarySkill].Value;
			double anatomy = m_Healer.Skills[secondarySkill].Value;

			if ( healing >= 80.0 && anatomy >= 80.0 )
			{
				int poisonStrength = GetPoisonStrength( m_Patient );
				if ( poisonStrength > 0 )
				{
					double chance = ( healing + anatomy - 120 ) * 25 / ( poisonStrength * 2000 );
					if ( chance > Utility.RandomDouble() )
					{
						if ( BleedAttack.IsBleeding( m_Patient ) )
						{
							m_Patient.SendLocalizedMessage( 1060167 ); // The bleeding wounds have healed, you are no longer bleeding!

							BleedAttack.EndBleed( m_Patient, message: false );
						}
						else if ( m_Patient.Poisoned && m_Patient.CurePoison( m_Healer ) )
						{
							m_Patient.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
						}

						m_AmountDivisor = poisonStrength;
					}
				}
			}
		}

		private static int GetPoisonStrength( Mobile patient )
		{
			if ( BleedAttack.IsBleeding( patient ) )
				return 3;

			if ( patient.Poisoned )
				return patient.Poison.Level;

			return 0;
		}

		public void EndHeal()
		{
			m_Table.Remove( m_Healer );

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;

			if ( m_Bandage != null )
				m_Bandage.OnHealFinished( this );

			int healerNumber = -1, patientNumber = -1;
			bool playSound = true;
			bool checkSkills = false;

			SkillName primarySkill = GetPrimarySkill( m_Patient, m_Healer );
			SkillName secondarySkill = GetSecondarySkill( m_Patient, m_Healer );

			BaseCreature petPatient = m_Patient as BaseCreature;

			if ( !m_Healer.Alive )
			{
				healerNumber = 500962; // You were unable to finish your work before you died.
				patientNumber = -1;
				playSound = false;
			}
			else if ( !m_Healer.InRange( m_Patient, Bandage.Range ) )
			{
				healerNumber = 500963; // You did not stay close enough to heal your target.
				patientNumber = -1;
				playSound = false;
			}
			else if ( !m_Patient.Alive || ( petPatient != null && petPatient.IsDeadPet ) )
			{
				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ( ( healing - 68.0 ) / 50.0 ) - ( m_Slips * 0.02 );

				if ( m_Patient is BaseMount && m_Healer == (Mobile) petPatient.ControlMaster && ( ( (BaseMount) m_Patient ).ItemID == 16047 || ( (BaseMount) m_Patient ).ItemID == 16048 || ( (BaseMount) m_Patient ).ItemID == 16049 || ( (BaseMount) m_Patient ).ItemID == 16050 ) )
				{
					healerNumber = 503255; // You are able to resurrect the creature.
					patientNumber = -1;

					m_Patient.PlaySound( 0x214 );
					m_Patient.FixedEffect( 0x376A, 10, 16 );

					petPatient.ControlMaster.CloseGump( typeof( PetResurrectGump ) );

					if ( petPatient.ControlMaster == m_Healer )
					{
						petPatient.ResurrectPet();

						for ( int i = 0; i < petPatient.Skills.Length; ++i ) // Decrease all skills on pet.
							petPatient.Skills[i].Base -= 0.1;
					}
					else
					{
						petPatient.ControlMaster.SendGump( new PetResurrectGump( m_Healer, petPatient ) );
					}
				}
				else if ( ( checkSkills = ( healing >= 80.0 && anatomy >= 80.0 ) ) && chance > Utility.RandomDouble() )
				{
					if ( m_Patient.Map == null || !m_Patient.Map.CanFit( m_Patient.Location, 16, false, false ) )
					{
						healerNumber = 501042; // Target can not be resurrected at that location.
						patientNumber = 502391; // Thou can not be resurrected there!
					}
					else if ( m_Patient.Region != null && m_Patient.Region.Name == "Khaldun" )
					{
						healerNumber = 1010395; // The veil of death in this area is too strong and resists thy efforts to restore life.
						patientNumber = -1;
					}
					else
					{
						healerNumber = 500965; // You are able to resurrect your patient.
						patientNumber = -1;

						m_Patient.PlaySound( 0x214 );
						m_Patient.FixedEffect( 0x376A, 10, 16 );

						if ( petPatient != null && petPatient.IsDeadPet )
						{
							Mobile master = petPatient.ControlMaster;

							if ( master != null && master.InRange( petPatient, 3 ) )
							{
								healerNumber = 503255; // You are able to resurrect the creature.

								master.CloseGump( typeof( PetResurrectGump ) );

								if ( master == m_Healer )
								{
									petPatient.ResurrectPet();

									for ( int i = 0; i < petPatient.Skills.Length; ++i ) // Decrease all skills on pet.
										petPatient.Skills[i].Base -= 0.1;
								}
								else
								{
									master.SendGump( new PetResurrectGump( m_Healer, petPatient ) );
								}
							}
							else
							{
								bool found = false;

								List<Mobile> friends = petPatient.Friends;

								for ( int i = 0; friends != null && i < friends.Count; ++i )
								{
									Mobile friend = friends[i];

									if ( friend.InRange( petPatient, 3 ) )
									{
										healerNumber = 503255; // You are able to resurrect the creature.

										friend.CloseGump( typeof( PetResurrectGump ) );

										if ( friend == m_Healer )
											petPatient.ResurrectPet();
										else
											friend.SendGump( new PetResurrectGump( m_Healer, petPatient ) );

										found = true;
										break;
									}
								}

								if ( !found )
									healerNumber = 1049670; // The pet's owner must be nearby to attempt resurrection.
							}
						}
						else
						{
							m_Patient.CloseGump( typeof( ResurrectGump ) );
							m_Patient.SendGump( new ResurrectGump( m_Patient, m_Healer ) );
						}
					}
				}
				else
				{
					if ( petPatient != null && petPatient.IsDeadPet )
						healerNumber = 503256; // You fail to resurrect the creature.
					else
						healerNumber = 500966; // You are unable to resurrect your patient.

					patientNumber = -1;
				}
			}
			else if ( m_Patient.Poisoned )
			{
				m_Healer.SendLocalizedMessage( 500969 ); // You finish applying the bandages.

				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ( ( healing - 30.0 ) / 50.0 ) - ( m_Patient.Poison.Level * 0.1 ) - ( m_Slips * 0.02 );

				if ( ( checkSkills = ( healing >= 60.0 && anatomy >= 60.0 ) ) && chance > Utility.RandomDouble() )
				{
					if ( m_Patient.CurePoison( m_Healer ) )
					{
						healerNumber = ( m_Healer == m_Patient ) ? -1 : 1010058; // You have cured the target of all poisons.
						patientNumber = 1010059; // You have been cured of all poisons.
					}
					else
					{
						healerNumber = -1;
						patientNumber = -1;
					}
				}
				else
				{
					healerNumber = 1010060; // You have failed to cure your target!
					patientNumber = -1;
				}
			}
			else if ( BleedAttack.IsBleeding( m_Patient ) )
			{
				healerNumber = 1060088; // You bind the wound and stop the bleeding
				patientNumber = 1060167; // The bleeding wounds have healed, you are no longer bleeding!

				BleedAttack.EndBleed( m_Patient, false );
			}
			else if ( MortalStrike.IsWounded( m_Patient ) )
			{
				healerNumber = ( m_Healer == m_Patient ? 1005000 : 1010398 );
				patientNumber = -1;
				playSound = false;
			}
			else if ( m_Patient.Hits == m_Patient.HitsMax )
			{
				healerNumber = 500967; // You heal what little damage your patient had.
				patientNumber = -1;
			}
			else
			{
				checkSkills = true;
				patientNumber = -1;

				double healing = m_Healer.Skills[primarySkill].Value;
				double anatomy = m_Healer.Skills[secondarySkill].Value;
				double chance = ( ( healing + 10.0 ) / 100.0 ) - ( m_Slips * 0.02 );

				if ( m_Bandage != null )
					healing += m_Bandage.HealingBonus;

				if ( chance > Utility.RandomDouble() )
				{
					healerNumber = 500969; // You finish applying the bandages.

					double min, max;

					if ( m_Patient.Player )
					{
						min = ( anatomy / 6.0 ) + ( healing / 6.0 ) + 3.0;
						max = ( anatomy / 6.0 ) + ( healing / 3.0 ) + 10.0;
					}
					else
					{
						min = ( anatomy / 5.0 ) + ( healing / 5.0 ) + 3.0;
						max = ( anatomy / 5.0 ) + ( healing / 2.0 ) + 10.0;
					}

					double toHeal = min + ( Utility.RandomDouble() * ( max - min ) );

					if ( m_Patient.Body.IsMonster || m_Patient.Body.IsAnimal )
						toHeal += m_Patient.HitsMax / 100;

					toHeal -= toHeal * m_Slips * ( 0.35 + Math.Max( 0, 140 - m_Healer.Dex ) / 400.0 ); // TODO: Verify algorithm

					if ( m_AmountDivisor > 1 )
						toHeal /= m_AmountDivisor;

					if ( toHeal < 1 )
					{
						toHeal = 1;
						healerNumber = 500968; // You apply the bandages, but they barely help.
					}

					int healedPoints = (int) toHeal;

					Server.Spells.Spellweaving.ArcaneEmpowermentSpell.ApplyHealBonus( m_Healer, ref healedPoints );

					m_Patient.Heal( healedPoints, m_Healer, message: false );
				}
				else
				{
					healerNumber = 500968; // You apply the bandages, but they barely help.
					playSound = false;
				}
			}

			if ( healerNumber != -1 )
				m_Healer.SendLocalizedMessage( healerNumber );

			if ( patientNumber != -1 )
				m_Patient.SendLocalizedMessage( patientNumber );

			if ( playSound )
				m_Patient.PlaySound( 0x57 );

			if ( checkSkills )
			{
				m_Healer.CheckSkill( secondarySkill, 0.0, 120.0 );
				m_Healer.CheckSkill( primarySkill, 0.0, 120.0 );
			}
		}

		private class InternalTimer : Timer
		{
			private BandageContext m_Context;
			private bool m_AttemptedMidlifeCure;

			public InternalTimer( BandageContext context, TimeSpan delay )
				: base( TimeSpan.FromSeconds( delay.TotalSeconds / 2 ), TimeSpan.FromSeconds( delay.TotalSeconds / 2 ) )
			{
				m_Context = context;
				m_AttemptedMidlifeCure = false;
			}

			protected override void OnTick()
			{
				if ( !m_AttemptedMidlifeCure )
				{
					m_Context.AttemptMidlifeCure();
					m_AttemptedMidlifeCure = true;
				}
				else
				{
					m_Context.EndHeal();
					Stop();
				}
			}
		}

		public static BandageContext BeginHeal( Mobile healer, Mobile patient, Bandage bandage )
		{
			bool isDeadPet = ( patient is BaseCreature && ( (BaseCreature) patient ).IsDeadPet );

			if ( patient is BaseCreature && ( (BaseCreature) patient ).IsGolem )
			{
				// Bandages cannot be used on that.
				healer.SendLocalizedMessage( 500970 );
			}
			else if ( patient is BaseCreature && ( (BaseCreature) patient ).IsAnimatedDead )
			{
				// You cannot heal that.
				healer.SendLocalizedMessage( 500951 );
			}
			else if ( !patient.Poisoned && patient.Hits == patient.HitsMax && !BleedAttack.IsBleeding( patient ) && !isDeadPet )
			{
				// That being is not damaged!
				healer.SendLocalizedMessage( 500955 );
			}
			else if ( !patient.Alive && ( patient.Map == null || !patient.Map.CanFit( patient.Location, 16, false, false ) ) )
			{
				// Target cannot be resurrected at that location.
				healer.SendLocalizedMessage( 501042 );
			}
			else if ( healer.CanBeBeneficial( patient, true, true ) )
			{
				healer.DoBeneficial( patient );

				bool onSelf = ( healer == patient );
				int dex = healer.Dex;

				double seconds;
				double resDelay = ( patient.Alive ? 0.0 : 5.0 );

				if ( onSelf )
				{
					seconds = 5 + ( 0.5 * ( (double) ( 120 - dex ) / 10 ) ); // TODO: Verify algorithm
				}
				else
				{
					if ( GetPrimarySkill( patient, healer ) == SkillName.Veterinary )
						seconds = 2;
					else
						seconds = 4 - ( (double) ( dex / 60 ) ) + resDelay;
				}

				if ( seconds < 1 )
					seconds = 1;

				if ( seconds > 8 )
					seconds = 8;

				BandageContext context = GetContext( healer );

				if ( context != null )
					context.RefreshTimer( TimeSpan.FromSeconds( seconds ) );
				else
					m_Table[healer] = context = new BandageContext( healer, patient, TimeSpan.FromSeconds( seconds ), bandage );

				if ( !onSelf )
					patient.SendLocalizedMessage( 1008078, false, healer.Name ); //  : Attempting to heal you.

				if ( healer.NetState != null )
					healer.NetState.Send( new CooldownInfo( bandage, (int) seconds ) );

				BuffInfo.AddBuff( healer, new BuffInfo( BuffIcon.Healing, 1151311, 1151400, TimeSpan.FromSeconds( seconds ), healer, patient.Name ) );

				healer.SendLocalizedMessage( 500956 ); // You begin applying the bandages.
				return context;
			}

			return null;
		}
	}
}