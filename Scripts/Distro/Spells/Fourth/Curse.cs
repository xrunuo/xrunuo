using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Engines.BuffIcons;
using Server.Spells.Bard;

namespace Server.Spells.Fourth
{
	public class CurseSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Curse", "Des Sanct",
				227,
				9031,
				Reagent.Nightshade,
				Reagent.Garlic,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public CurseSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		private static Hashtable m_UnderEffect = new Hashtable();

		#region Tainted Tree
		public static void AddTimer( Mobile m, Timer t )
		{
			m_UnderEffect[m] = t;
		}

		public static Timer GetTimer( Mobile m )
		{
			return (Timer) m_UnderEffect[m];
		}
		#endregion

		public static void RemoveEffect( object state )
		{
			Mobile m = (Mobile) state;

			m_UnderEffect.Remove( m );

			m.UpdateResistances();
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_UnderEffect.Contains( m );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
				DoCurse( m );
			}

			FinishSequence();
		}

		public void DoCurse( Mobile m )
		{
			SpellHelper.Turn( Caster, m );

			SpellHelper.CheckReflect( (int) this.Circle, Caster, ref m );

			SpellHelper.AddStatCurse( Caster, m, StatType.Str ); SpellHelper.DisableSkillCheck = true;
			SpellHelper.AddStatCurse( Caster, m, StatType.Dex );
			SpellHelper.AddStatCurse( Caster, m, StatType.Int ); SpellHelper.DisableSkillCheck = false;

			Timer t = (Timer) m_UnderEffect[m];

			if ( t == null )
			{
				TimeSpan duration = ComputeDuration( Caster, m );

				int percentage = (int) ( SpellHelper.GetOffsetScalar( Caster, m, true ) * 100 );
				string buffFormat = String.Format( "{0}\t{0}\t{0}\t{1}\t{1}\t{1}\t{1}", percentage, 10 );

				BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Curse, 1075835, duration, m, buffFormat ) );

				m_UnderEffect[m] = t = Timer.DelayCall( duration, new TimerStateCallback( RemoveEffect ), m );
				m.UpdateResistances();
			}

			if ( m.Spell != null )
				m.Spell.OnCasterHurt();

			m.Paralyzed = false;

			m.FixedParticles( 0x374A, 10, 15, 5028, EffectLayer.Waist );
			m.PlaySound( 0x1E1 );
		}

		private static TimeSpan ComputeDuration( Mobile caster, Mobile defender )
		{
			double seconds = SpellHelper.GetDuration( caster, defender ).TotalSeconds;

			Resilience song = Spellsong.GetEffectSpellsong<Resilience>( defender );

			if ( song != null )
				seconds = seconds - ( song.CurseReduction * seconds / 100.0 );

			return TimeSpan.FromSeconds( seconds );
		}

		private class InternalTarget : Target
		{
			private CurseSpell m_Owner;

			public InternalTarget( CurseSpell owner )
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