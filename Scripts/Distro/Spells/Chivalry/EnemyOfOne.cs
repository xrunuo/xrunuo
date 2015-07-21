using System;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Engines.BuffIcons;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Chivalry
{
	public class EnemyOfOneSpell : PaladinSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Enemy of One", "Forul Solum",
				-1,
				9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 0.5 ); } }

		public override double RequiredSkill { get { return 45.0; } }
		public override int RequiredMana { get { return 20; } }
		public override int RequiredTithing { get { return 10; } }
		public override int MantraNumber { get { return 1060723; } } // Forul Solum
		public override bool BlocksMovement { get { return false; } }

		public EnemyOfOneSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override TimeSpan GetCastDelay()
		{
			if ( UnderEffect( Caster ) )
				return TimeSpan.Zero;

			return base.GetCastDelay();
		}

		public override void OnCast()
		{
			if ( UnderEffect( Caster ) )
			{
				PlayEffects();

				// Recasting while the spell is in effect will not cost any Mana or casting time.
				RemoveEffect( Caster, true );
			}
			else if ( CheckSequence() )
			{
				PlayEffects();

				double seconds = (double) ComputePowerValue( 1 );

				// TODO: Should caps be applied?
				Utility.FixMinMax( ref seconds, 90, 210 );

				TimeSpan delay = TimeSpan.FromSeconds( seconds );

				Timer timer = Timer.DelayCall( delay,
					delegate
					{
						RemoveEffect( Caster, true );
					} );

				DateTime expire = DateTime.Now + delay;

				m_Table[Caster] = new EnemyOfOneContext( Caster, timer, expire );
			}

			FinishSequence();
		}

		private void PlayEffects()
		{
			Caster.PlaySound( 0x0F5 );
			Caster.PlaySound( 0x1ED );

			Caster.FixedParticles( 0x375A, 1, 30, 9966, 33, 2, EffectLayer.Head );
			Caster.FixedParticles( 0x37B9, 1, 30, 9502, 43, 3, EffectLayer.Head );
		}

		private static Dictionary<Mobile, EnemyOfOneContext> m_Table = new Dictionary<Mobile, EnemyOfOneContext>();

		public static EnemyOfOneContext GetContext( Mobile m )
		{
			if ( !m_Table.ContainsKey( m ) )
				return null;

			return m_Table[m];
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public static void RemoveEffect( Mobile m )
		{
			RemoveEffect( m, false );
		}

		public static void RemoveEffect( Mobile m, bool sound )
		{
			if ( m_Table.ContainsKey( m ) )
			{
				var context = m_Table[m];

				m_Table.Remove( m );

				context.OnRemoved();

				if ( sound )
					m.PlaySound( 0x1F8 );
			}
		}
	}

	public class EnemyOfOneContext
	{
		private Mobile m_Owner;
		private Timer m_Timer;
		private DateTime m_Expire;
		private Type m_TargetType;
		private int m_DamageScalar;

		public Mobile Owner
		{
			get { return m_Owner; }
		}

		public Timer Timer
		{
			get { return m_Timer; }
		}

		public Type TargetType
		{
			get { return m_TargetType; }
		}

		public int DamageScalar
		{
			get { return m_DamageScalar; }
		}

		public EnemyOfOneContext( Mobile owner, Timer timer, DateTime expire )
		{
			m_Owner = owner;
			m_Timer = timer;
			m_Expire = expire;
			m_TargetType = null;
			m_DamageScalar = 50;

			UpdateBuffInfo();
		}

		public void OnHit( Mobile defender )
		{
			if ( m_TargetType == null )
			{
				m_TargetType = defender.GetType();

				// Odd but OSI recalculates when the target changes...
				int chivalry = (int) m_Owner.Skills.Chivalry.Value;
				m_DamageScalar = 10 + ( ( chivalry - 40 ) * 9 ) / 10;

				DeltaEnemies();
				UpdateBuffInfo();
			}
		}

		public void OnRemoved()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			DeltaEnemies();

			BuffInfo.RemoveBuff( m_Owner, BuffIcon.EnemyOfOne );
		}

		private void DeltaEnemies()
		{
			foreach ( Mobile m in m_Owner.GetMobilesInRange( 18 ) )
			{
				if ( m.GetType() == m_TargetType )
					m_Owner.Send( GenericPackets.MobileMoving( m, Notoriety.Compute( m_Owner, m ) ) );
			}
		}

		private void UpdateBuffInfo()
		{
			// TODO: display friendly name attribute when target is not null.
			BuffInfo.AddBuff( m_Owner, new BuffInfo( BuffIcon.EnemyOfOne, 1075653, 1075902, m_Expire - DateTime.Now, m_Owner, String.Format( "{0}\t{1}", m_DamageScalar.ToString(), 100.ToString() ) ) );
		}
	}
}