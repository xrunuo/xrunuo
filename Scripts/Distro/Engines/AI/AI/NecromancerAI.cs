using System;
using System.Linq;
using Server;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Necromancy;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
	public class NecromancerAI : MageAI
	{
		private const double ChanceToUseNecroSpells = 0.5;

		public NecromancerAI( BaseCreature m )
			: base( m )
		{
		}

		public override Spell ChooseSpell( Mobile c )
		{
			Spell spell = null;

			spell = GetRandomBlessSpell();

			if ( spell != null )
				return spell;

			spell = CheckCastHealingSpell();

			if ( spell != null )
				return spell;

			switch ( Utility.Random( 12 ) )
			{
				default:
				case 0:
				case 1: // Curse them.
					{
						spell = GetRandomCurseSpell( c );

						break;
					}
				case 2: // Poison them
					{
						if ( !c.Poisoned )
							spell = new PoisonSpell( m_Mobile, null );

						break;
					}
				case 3: // Drain some mana
					{
						spell = GetRandomManaDrainSpell( c );

						break;
					}
				case 4: // Animate dead
					{
						spell = new AnimateDeadSpell( m_Mobile, null );

						break;
					}
				case 5: // Vengeful spirit
					{
						spell = new VengefulSpiritSpell( m_Mobile, null );

						break;
					}
				case 6:
				case 7:
				case 8: // Deal some damage
					{
						spell = GetRandomDamageSpell( c );

						break;
					}
				case 9:
				case 10:
				case 11: // Set up a combo
					{
						if ( m_Mobile.Mana < 40 && m_Mobile.Mana > 15 )
						{
							if ( c.Paralyzed && !c.Poisoned )
							{
								m_Mobile.DebugSay( "I am going to meditate" );

								m_Mobile.UseSkill( SkillName.Meditation );
							}
							else if ( !c.Poisoned )
							{
								spell = new ParalyzeSpell( m_Mobile, null );
							}
						}
						else if ( m_Mobile.Mana > 60 )
						{
							if ( Utility.Random( 2 ) == 0 && !c.Paralyzed && !c.Frozen && !c.Poisoned )
							{
								m_Combo = 0;
								spell = new ParalyzeSpell( m_Mobile, null );
							}
							else
							{
								m_Combo = 1;
								spell = new ExplosionSpell( m_Mobile, null );
							}
						}

						break;
					}
			}

			return spell;
		}

		public override bool ProcessTarget()
		{
			Target targ = m_Mobile.Target;

			if ( targ == null )
				return false;

			if ( m_Mobile.Target is AnimateDeadSpell.InternalTarget )
			{
				var toAnimate = m_Mobile.GetItemsInRange( 10 ).OfType<Corpse>().FirstOrDefault();

				if ( toAnimate != null )
					targ.Invoke( m_Mobile, toAnimate );
				else
					targ.Cancel( m_Mobile, TargetCancelType.Canceled );

				return true;
			}
			else
			{
				return base.ProcessTarget();
			}
		}

		public override Spell GetRandomHealingSpell()
		{
			if ( m_Mobile.Hits < ( m_Mobile.HitsMax - 50 ) )
			{
				if ( m_Mobile.GetItemsInRange( 3 ).OfType<Corpse>().Where( c => !c.Channeled ).Any() )
					return new SkillHandlers.SpiritSpeak.SpiritSpeakSpell( m_Mobile );
			}

			return base.GetRandomHealingSpell(); // Use magery spells
		}

		public override Spell GetRandomCurseSpell( Mobile c )
		{
			if ( ChanceToUseNecroSpells > Utility.RandomDouble() ) // Use necro spells
			{
				switch ( Utility.Random( 5 ) )
				{
					default:
					case 0: return new CorpseSkinSpell( m_Mobile, null );
					case 1: return new StrangleSpell( m_Mobile, null );
					case 2: return new MindRotSpell( m_Mobile, null );
					case 3: return new EvilOmenSpell( m_Mobile, null );
					case 4: return new BloodOathSpell( m_Mobile, null );
				}
			}

			return base.GetRandomCurseSpell( c ); // Use magery spells
		}

		public override Spell GetRandomDamageSpell( Mobile c )
		{
			if ( c.Hits < c.HitsMax / 10 && !PainSpikeSpell.UnderEffect( c ) )
				return new PainSpikeSpell( m_Mobile, null );

			if ( ChanceToUseNecroSpells > Utility.RandomDouble() )
			{
				int maxCircle = (int) ( ( m_Mobile.Skills[SkillName.Necromancy].Value + 20.0 ) / ( 100.0 / 7.0 ) );
				int foes = 0;

				foreach ( Mobile foe in m_Mobile.GetMobilesInRange( 5 ) )
				{
					foes++;
				}

				if ( maxCircle >= 6 && Utility.Random( foes ) != 0 )
					return new WitherSpell( m_Mobile, null );
				else if ( maxCircle >= 5 )
					return new PoisonStrikeSpell( m_Mobile, null );
			}

			return base.GetRandomDamageSpell( c );
		}
	}
}
