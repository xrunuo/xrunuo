using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Mysticism
{
	public class MassSleepSpell : MysticismSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Mass Sleep", "Vas Zu",
				-1,
				9002,
				Reagent.Ginseng,
				Reagent.Nightshade,
				Reagent.SpidersSilk
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.75 ); } }

		public override double RequiredSkill { get { return 45.0; } }
		public override int RequiredMana { get { return 14; } }

		public MassSleepSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			/*
			 * Puts one or more Targets within a radius around the Target's Location
			 * into a temporary Sleep state. In this state, Slept Targets will be
			 * unable to attack or cast spells, and will move at a much slower speed.
			 * They will awaken if harmed or after a set amount of time. The Sleep
			 * duration is determined by a comparison between the Caster's Mysticism
			 * and either Focus or Imbuing (whichever is greater) skills and the
			 * Target's Resisting Spells skill.
			 */

			if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, m );

				var map = Caster.Map;

				if ( map != null )
				{
					foreach ( var target in m.GetMobilesInRange( 2 ) )
					{
						if ( Caster != target && Caster.InLOS( target ) && SpellHelper.ValidIndirectTarget( Caster, target ) && Caster.CanBeHarmful( target, false ) && !target.Hidden )
						{
							if ( SleepSpell.CheckSleep( Caster, target ) )
								SleepSpell.DoSleep( Caster, target );
						}
					}
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private MassSleepSpell m_Owner;

			public InternalTarget( MassSleepSpell owner )
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
