using System;
using Server;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Spellweaving;
using Server.Targeting;

namespace Server.Mobiles
{
	public class ArcanistAI : MageAI
	{
		public ArcanistAI( BaseCreature m )
			: base( m )
		{
		}

		public override Spell GetRandomHealingSpell()
		{
			if ( !GiftOfRenewalSpell.UnderEffect( m_Mobile ) && !GiftOfRenewalSpell.m_Table2.Contains( m_Mobile ) )
				return new GiftOfRenewalSpell( m_Mobile, null );

			return base.GetRandomHealingSpell();
		}

		public override Spell ChooseSpell( Mobile c )
		{
			Spell spell = CheckCastHealingSpell();

			if ( spell != null )
				return spell;

			switch ( Utility.Random( 16 ) )
			{
				case 0:
				case 1:
				case 2:	// Poison them
					{
						m_Mobile.DebugSay( "Attempting to poison" );

						if ( !c.Poisoned )
							spell = new PoisonSpell( m_Mobile, null );

						break;
					}
				case 3:	// Bless ourselves.
					{
						m_Mobile.DebugSay( "Blessing myself" );

						if ( Utility.RandomBool() && !ArcaneEmpowermentSpell.IsBuffed( m_Mobile ) )
							spell = new ArcaneEmpowermentSpell( m_Mobile, null );
						else
							spell = new BlessSpell( m_Mobile, null );

						break;
					}
				case 4: // Wildfire
					{
						m_Mobile.DebugSay( "Incendio!" );

						spell = new WildfireSpell( m_Mobile, null );

						break;
					}
				case 5: // Reduce their cast speed
					{
						if ( c.InRange( m_Mobile.Location, 6 ) )
						{
							if ( m_Mobile.Skills[SkillName.Spellweaving].Value >= 90.0 )
								spell = new EssenceOfWindSpell( m_Mobile, null );
							else if ( c.InRange( m_Mobile.Location, 2 ) )
								spell = new ThunderstormSpell( m_Mobile, null );
						}

						m_Mobile.DebugSay( "Attempting to reduce their cast speed" );

						break;
					}
				case 6: // Curse them.
					{
						m_Mobile.DebugSay( "Attempting to curse" );

						spell = GetRandomCurseSpell( c );
						break;
					}
				case 7:	// Paralyze them.
					{
						m_Mobile.DebugSay( "Attempting to paralyze" );

						if ( m_Mobile.Skills[SkillName.Magery].Value > 50.0 )
							spell = new ParalyzeSpell( m_Mobile, null );

						break;
					}
				case 8: // Drain mana
					{
						m_Mobile.DebugSay( "Attempting to drain mana" );

						spell = GetRandomManaDrainSpell( c );
						break;
					}

				default: // Damage them.
					{
						m_Mobile.DebugSay( "Just doing damage" );

						spell = GetRandomDamageSpell( c );
						break;
					}
			}

			return spell;
		}

		public override bool ProcessTarget()
		{
			if ( m_Mobile.Target is WildfireSpell.InternalTarget || m_Mobile.Target is GiftOfRenewalSpell.InternalTarget )
			{
				m_Mobile.Target.Invoke( m_Mobile, m_Mobile );
				return true;
			}

			return base.ProcessTarget();
		}
	}
}
