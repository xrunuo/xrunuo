using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Mysticism
{
	public class BombardSpell : MysticSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Bombard", "Corp Por Ylem",
				-1,
				9002,
				Reagent.Bloodmoss,
				Reagent.DragonsBlood,
				Reagent.Garlic,
				Reagent.SulfurousAsh
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }

		public override double RequiredSkill { get { return 58.0; } }
		public override int RequiredMana { get { return 20; } }

		public BombardSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( CheckHSequence( m ) )
			{
				/* Hurls a magical boulder at the Target, dealing physical damage.
				 * This spell also has a chance to knockback and stun a player
				 * Target. The stun chance is determined by a comparison between
				 * the Caster's Evaluating Intelligence and Mysticism skills and
				 * the Target's Resisting Spells skill.
				 */

				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( 5, Caster, ref m );

				Caster.MovingParticles( m, 0x1363, 7, 0, false, true, 0, 0, 0xBBE, 0xFA6, 0xFFFF, 0 );
				Caster.PlaySound( 0x64B );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback<Mobile>( Damage ), m );
			}

			FinishSequence();
		}

		private void Damage( Mobile to )
		{
			if ( to != null )
			{
				// send the effects
				Effects.SendPacket( to, to.Map, new GraphicalEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x36BD, to.Location, to.Location, 20, 10, true, false ) );
				to.PlaySound( 0x307 );

				// deal the damage
				SpellHelper.Damage( this, to, GetNewAosDamage( 40, 1, 5, to ), 100, 0, 0, 0, 0 );

				// stun the target
				var stunChance = ( GetBaseSkill( Caster ) + GetBoostSkill( Caster ) ) / 2000.0;
				stunChance -= GetResistSkill( to ) / 1000.0;

				if ( Utility.RandomDouble() < stunChance && !ParalyzingBlow.IsInmune( to ) )
				{
					to.Freeze( TimeSpan.FromSeconds( 2.0 ) );
					ParalyzingBlow.BeginInmunity( to, TimeSpan.FromSeconds( 10.0 ) );
				}
			}
		}

		private class InternalTarget : Target
		{
			private BombardSpell m_Owner;

			public InternalTarget( BombardSpell owner )
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
