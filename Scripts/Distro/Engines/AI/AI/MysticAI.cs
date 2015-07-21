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
using Server.Spells.Mysticism;
using Server.Targeting;

namespace Server.Mobiles
{
	public class MysticAI : MageAI
	{
		public MysticAI( BaseCreature m )
			: base( m )
		{
		}

		public override Spell GetRandomHealingSpell()
		{
			Spell spell = null;

			if ( m_Mobile.Hits < ( m_Mobile.HitsMax - 50 ) )
			{
				if ( m_Mobile.Skills[SkillName.Mysticism].Value >= 70.0 )
					spell = new CleansingWindsSpell( m_Mobile, null );
				else
					spell = new GreaterHealSpell( m_Mobile, null );

				if ( spell == null )
					spell = new HealSpell( m_Mobile, null );
			}
			else if ( m_Mobile.Hits < ( m_Mobile.HitsMax - 10 ) )
				spell = new HealSpell( m_Mobile, null );

			return spell;
		}

		public override Spell GetRandomDamageSpell( Mobile c )
		{
			if ( 0.5 > Utility.RandomDouble() ) // Use mysticism spells
			{
				int maxCircle = (int) ( ( m_Mobile.Skills[SkillName.Mysticism].Value + 20.0 ) / ( 100.0 / 7.0 ) );

				if ( maxCircle < 1 )
					maxCircle = 1;

				switch ( Utility.Random( maxCircle * 2 ) )
				{
					case 0:
					case 1:
					case 2:
					case 3: return new NetherBoltSpell( m_Mobile, null );
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10: return new EagleStrikeSpell( m_Mobile, null );
					case 11:
					case 12:
					case 13: return new BombardSpell( m_Mobile, null );
					case 14: return new HailStormSpell( m_Mobile, null );
					case 15: return new SpellPlagueSpell( m_Mobile, null );
					case 16: return new NetherCycloneSpell( m_Mobile, null );
				}
			}

			return base.GetRandomDamageSpell( c ); // Use magery spells
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

						spell = new BlessSpell( m_Mobile, null );

						break;
					}
				case 4: // Sleep
					{
						m_Mobile.DebugSay( "¡A dormir!" );

						spell = new SleepSpell( m_Mobile, null );

						break;
					}
				case 5: // Curse them.
					{
						m_Mobile.DebugSay( "Attempting to curse" );

						spell = GetRandomCurseSpell( c );
						break;
					}
				case 6:
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
	}
}
