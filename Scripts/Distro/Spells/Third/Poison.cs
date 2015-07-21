using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.Third
{
	public class PoisonSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Poison", "In Nox",
				203,
				9051,
				Reagent.Nightshade
			);

		public override SpellCircle Circle { get { return SpellCircle.Third; } }

		public PoisonSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int) this.Circle, Caster, ref m );

				if ( m.Spell != null )
					m.Spell.OnCasterHurt();

				m.Paralyzed = false;

				if ( CheckResisted( m ) )
				{
					m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
				}
				else
				{
					int level = GetPoisonLevel( Caster, m, SkillName.Poisoning, SkillName.Magery );

					// Players with greater than GM Poisoning and GM Magery will have a 10%
					// chance to inflict lethal poison at distance of less than 3 tiles.

					double magery = Caster.Skills.Magery.Value;
					double poisoning = Caster.Skills.Poisoning.Value;

					bool isGrandMaster = poisoning >= 100.0 && magery >= 100.0;

					if ( level == 3 && isGrandMaster && 0.1 > Utility.RandomDouble() )
						level++;

					m.ApplyPoison( Caster, Poison.GetPoison( level ) );
				}

				m.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
				m.PlaySound( 0x474 );
			}

			FinishSequence();
		}

		public static int GetPoisonLevel( Mobile attacker, Mobile defender, SkillName primarySkill, SkillName secondarySkill )
		{
			int primarySkillFixed = attacker.Skills[primarySkill].Fixed;
			int secondarySkillFixed = attacker.Skills[secondarySkill].Fixed;

			int total = ( primarySkillFixed + secondarySkillFixed ) / 2;

			int level;

			if ( total >= 1000 )
				level = 3;
			else if ( total > 850 )
				level = 2;
			else if ( total > 650 )
				level = 1;
			else
				level = 0;

			int penalty;

			if ( attacker.InRange( defender, 2 ) )
				penalty = 0;
			else if ( attacker.InRange( defender, 4 ) )
				penalty = 1;
			else if ( attacker.InRange( defender, 5 ) )
				penalty = 2;
			else
				penalty = 3;

			level -= penalty;

			if ( level < 0 )
				level = 0;

			return level;
		}

		private class InternalTarget : Target
		{
			private PoisonSpell m_Owner;

			public InternalTarget( PoisonSpell owner )
				: base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile) o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}