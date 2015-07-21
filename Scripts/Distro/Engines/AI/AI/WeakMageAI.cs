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
	public class WeakMageAI : MageAI
	{
		private const double ChanceToUseNecroSpells = 0.5;

		public WeakMageAI( BaseCreature m )
			: base( m )
		{
		}

		public override Spell ChooseSpell( Mobile c )
		{
			Spell spell = null;

			spell = CheckCastHealingSpell();

			if ( spell != null )
				return spell;

			switch ( Utility.Random( 3 ) )
			{
				default:
				case 0:
				case 1: // Curse them.
					{
						spell = GetRandomCurseSpell( c );

						break;
					}
				case 2: // Drain some mana
					{
						spell = GetRandomManaDrainSpell( c );

						break;
					}
			}

			return spell;
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
				switch ( Utility.Random( 4 ) )
				{
					default:
					case 0: return new CorpseSkinSpell( m_Mobile, null );
					case 1: return new MindRotSpell( m_Mobile, null );
					case 2: return new EvilOmenSpell( m_Mobile, null );
					case 3: return new BloodOathSpell( m_Mobile, null );
				}
			}

			return base.GetRandomCurseSpell( c ); // Use magery spells
		}
	}
}
