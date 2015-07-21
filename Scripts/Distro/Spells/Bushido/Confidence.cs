using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Bushido
{
	public class Confidence : SamuraiSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Confidence", null,
				-1,
				9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 0.25 ); } }

		public override double RequiredSkill { get { return 25.0; } }
		public override int RequiredMana { get { return 10; } }

		public static Hashtable m_Table = new Hashtable();

		private static Hashtable m_RegenTable = new Hashtable();

		private static Timer GetTimer( Mobile m )
		{
			return (Timer) m_RegenTable[m];
		}

		public static bool IsConfident( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static bool IsRegenerating( Mobile m )
		{
			return m_RegenTable.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m.Send( new ToggleSpecialAbility( 0x92, 0 ) );

			m.SendLocalizedMessage( 1063116 ); // Your confidence wanes.

			m_Table.Remove( m );
		}

		public Confidence( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.SendLocalizedMessage( 1063115 ); // You exude confidence.

				Caster.FixedParticles( 0x375A, 1, 17, 0x7DA, 0x960, 0x3, EffectLayer.Waist );
				Caster.PlaySound( 0x51A );

				OnCastSuccessful( Caster );

				// BeginConfidence
				Timer t = (Timer) m_Table[Caster];

				if ( t != null )
					t.Stop();

				m_Table[Caster] = t = Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerStateCallback( Expire_Callback ), Caster );

				// BeginRegenerating
				Timer regen = (Timer) m_RegenTable[Caster];

				if ( regen != null )
					regen.Stop();

				m_RegenTable[Caster] = regen = new RegenTimer( Caster );

				regen.Start();
			}

			FinishSequence();
		}

		public static void EndConfidence( Mobile m )
		{
			Timer t = (Timer) m_Table[m];

			if ( t != null )
				t.Stop();

			m_Table.Remove( m );

			OnEffectEnd( m, typeof( Confidence ) );
		}

		public static void StopRegenerating( Mobile m )
		{
			Timer t = GetTimer( m );

			if ( t != null )
				t.Stop();

			Confidence.m_RegenTable.Remove( m );
		}

		private class RegenTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_Count;

			public RegenTimer( Mobile m )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.33 ), 15 )
			{
				m_Mobile = m;

			}

			protected override void OnTick()
			{
				if ( !m_Mobile.Alive )
				{
					m_RegenTable.Remove( m_Mobile );
					Stop();
				}

				if ( m_Count == 15 )
					m_RegenTable.Remove( m_Mobile );

				if ( IsRegenerating( m_Mobile ) )
				{
					Skill skill = m_Mobile.Skills[SkillName.Bushido];

					double v = skill.Value;

					double points = ( 15 + ( v * v ) / 576 ) / 15.0;

					if ( ( points - (int) points ) > Utility.RandomDouble() )
						points++;

					m_Mobile.Hits += (int) points;

					m_Count++;
				}
			}
		}
	}
}