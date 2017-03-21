using System;
using System.Collections.Generic;
using Server.Engines.BuffIcons;
using Server.Targeting;

namespace Server.Spells.Mysticism
{
	public class SleepSpell : MysticismSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Sleep", "In Zu",
				-1,
				9002,
				Reagent.Nightshade,
				Reagent.SpidersSilk,
				Reagent.BlackPearl
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.25 ); } }

		public override double RequiredSkill { get { return 20.0; } }
		public override int RequiredMana { get { return 9; } }

		public SleepSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( CheckSleep( Caster, m ) && CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				DoSleep( Caster, m );
			}

			FinishSequence();
		}

		public static bool CheckSleep( Mobile from, Mobile m )
		{
			if ( IsSlept( m ) )
			{
				from.SendLocalizedMessage( 1080134 ); // Your target is already immobilized and cannot be slept.
				return false;
			}
			if ( !m.Alive )
			{
				from.SendLocalizedMessage( 1080135 ); // Your target cannot be put to sleep
				return false;
			}

			return true;
		}

		public static void DoSleep( Mobile from, Mobile m )
		{
			/* Puts the Target into a temporary Sleep state. In this state,
			 * a Slept Target will be unable to attack or cast spells, and
			 * will move at a much slower speed. A Slept Target will awaken
			 * if harmed or after a set amount of time. The Sleep duration
			 * is determined by a comparison between the Caster's Evaluating
			 * Intelligence and Mysticism skills and the Target's Resisting
			 * Spells skill.
			 *
			 * Buff message:
			 * - Movement slowed.
			 * - Cannot attack or cast spells.
			 * - Defensive capabilities greatly reduced.
			 */

			from.PlaySound( 0x653 );

			var seconds = (int) ( GetBaseSkill( from ) + GetBoostSkill( from ) ) / 20;
			seconds -= (int) m.Skills[SkillName.MagicResist].Value / 10;

			if ( seconds > 0 && !UnderCooldown( m ) )
			{
				BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Sleep, 1080139, TimeSpan.FromSeconds( seconds ), m, seconds.ToString() ) );

				var t = new InternalTimer( m, DateTime.Now + TimeSpan.FromSeconds( seconds ) );
				t.Start();

				var mods = new List<AttributeMod>();
				mods.Add( new AttributeMod( MagicalAttribute.AttackChance, -45 ) );
				mods.Add( new AttributeMod( MagicalAttribute.DefendChance, -45 ) );
				mods.Add( new AttributeMod( MagicalAttribute.WeaponSpeed, -40 ) );
				mods.Add( new AttributeMod( MagicalAttribute.CastSpeed, -2 ) );
				mods.Add( new AttributeMod( MagicalAttribute.CastRecovery, -4 ) );
				foreach ( AttributeMod mod in mods )
				{
					m.AddAttributeMod( mod );
				}

				m_SleptTable[m] = new SleepContext( t, mods );

				m.ForcedWalk = true;

				var cooldownSeconds = seconds + (int) m.Skills.MagicResist.Value / 10;

				m_CooldownTable.Add( m );
				Timer.DelayCall( TimeSpan.FromSeconds( cooldownSeconds ), delegate
				{
					m_CooldownTable.Remove( m );
				} );
			}
			else
			{
				from.SendLocalizedMessage( 1080136 ); // Your target resists sleep.
				m.SendLocalizedMessage( 1080137 ); // You resist sleep.
			}
		}

		private static ISet<Mobile> m_CooldownTable = new HashSet<Mobile>();

		public static bool UnderCooldown( Mobile m )
		{
			return m_CooldownTable.Contains( m );
		}

		private static Dictionary<Mobile, SleepContext> m_SleptTable = new Dictionary<Mobile, SleepContext>();

		public static bool IsSlept( Mobile m )
		{
			return m_SleptTable.ContainsKey( m );
		}

		public static void RemoveEffect( Mobile m )
		{
			if ( !m_SleptTable.ContainsKey( m ) )
				return;

			var context = m_SleptTable[m];

			context.Timer.Stop();

			foreach ( var mod in context.Mods )
			{
				m.RemoveAttributeMod( mod );
			}

			m_SleptTable.Remove( m );

			BuffInfo.RemoveBuff( m, BuffIcon.Sleep );
			m.ForcedWalk = false;
		}

		private class SleepContext
		{
			public Timer Timer { get; }
			public List<AttributeMod> Mods { get; }

			public SleepContext( Timer timer, List<AttributeMod> mods )
			{
				Timer = timer;
				Mods = mods;
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Target;
			private DateTime m_End;

			public InternalTimer( Mobile target, DateTime end )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.5 ) )
			{

				m_Target = target;
				m_End = end;
			}

			protected override void OnTick()
			{
				if ( m_Target.Deleted || !m_Target.Alive || DateTime.Now > m_End )
				{
					RemoveEffect( m_Target );
				}
				else
				{
					Effects.SendTargetParticles( m_Target, 0x3779, 1, 32, 0x13BA, EffectLayer.Head );

					// OSI sends 0xDE packet here, maybe to update buff icon info. Is it really needed?
				}
			}
		}

		private class InternalTarget : Target
		{
			private SleepSpell m_Owner;

			public InternalTarget( SleepSpell owner )
				: base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile) o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
