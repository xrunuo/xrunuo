using System;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

namespace Server.Spells.First
{
	public class MagicArrowSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Magic Arrow", "In Por Ylem",
				212,
				9041,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.First; } }
		public override bool DelayedDamage { get { return false; } }

		public MagicArrowSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			PlayerMobile pm = Caster as PlayerMobile;

			if ( pm != null && DateTime.UtcNow < ( pm.LastArrow + DamageDelay ) )
			{
				DoFizzle();
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int) this.Circle, Caster, ref m );

				Caster.MovingParticles( m, 0x36E4, 5, 0, false, true, 3006, 4006, 0 );
				Caster.PlaySound( 0x1E5 );

				if ( pm != null )
					pm.LastArrow = DateTime.UtcNow;

				Timer.DelayCall( DamageDelay, new TimerCallback(
					delegate
					{
						double damage = GetNewAosDamage( 10, 1, 4, m );

						SpellHelper.Damage( this, m, damage, 0, 100, 0, 0, 0 );
					} ) );
			}

			FinishSequence();
		}

		private static readonly TimeSpan DamageDelay = TimeSpan.FromSeconds( 1.0 );

		private class InternalTarget : Target
		{
			private MagicArrowSpell m_Owner;

			public InternalTarget( MagicArrowSpell owner )
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