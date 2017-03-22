using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Mysticism
{
	public class NetherBoltSpell : MysticSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Nether Bolt", "In Corp Ylem",
				-1,
				9002,
				Reagent.BlackPearl,
				Reagent.SulfurousAsh
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 0.75 ); } }

		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 4; } }

		public NetherBoltSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			var pm = Caster as PlayerMobile;

			if ( pm != null && DateTime.Now < ( pm.LastArrow + DamageDelay ) )
			{
				DoFizzle();
			}
			else if ( CheckHSequence( m ) )
			{
				/* Fires a bolt of nether energy at the Target,
				 * dealing chaos damage.
				 */

				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( 0, Caster, ref m );

				Caster.MovingParticles( m, 0x36D4, 7, 0, false, true, 0x49A, 0, 0x251E, 0x4A7A, 0x160, 0 );
				Caster.PlaySound( 0x211 );

				if ( pm != null )
					pm.LastArrow = DateTime.Now;

				Timer.DelayCall( DamageDelay, delegate
				{
					double damage = GetNewAosDamage( 10, 1, 4, m );

					var types = new int[4];
					types[Utility.Random( types.Length )] = 100;

					SpellHelper.Damage( this, m, damage, 0, types[0], types[1], types[2], types[3] );
				} );
			}

			FinishSequence();
		}

		private static readonly TimeSpan DamageDelay = TimeSpan.FromSeconds( 1.0 );

		private class InternalTarget : Target
		{
			private NetherBoltSpell m_Owner;

			public InternalTarget( NetherBoltSpell owner )
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
