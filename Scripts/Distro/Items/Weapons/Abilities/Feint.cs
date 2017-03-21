using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;
using Server.Events;

namespace Server.Items
{
	/// <summary>
	/// Gain a defensive advantage over your primary opponent for a short time.
	/// Requires Bushido or Ninjitsu skill.
	/// </summary>
	public class Feint : SEWeaponAbility
	{
		private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds( 6.0 );

		public static new void Initialize()
		{
			EventSink.BeforeDamage += new BeforeDamageEventHandler( EventSink_BeforeDamage );
		}

		private static void EventSink_BeforeDamage( BeforeDamageEventArgs e )
		{
			int amount = e.Amount;

			AlterDamage( e.From, e.Mobile, ref amount );

			e.Amount = amount;
		}

		private static Dictionary<Mobile, FeintContext> m_Table = new Dictionary<Mobile, FeintContext>();

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !IsBladeweaveAttack && ( !Validate( attacker ) || !CheckMana( attacker, true ) ) )
				return;

			ClearCurrentAbility( attacker );

			if ( m_Table.ContainsKey( attacker ) )
			{
				Timer t = m_Table[attacker].Timer;

				if ( t != null )
					t.Stop();
			}

			Timer expireTimer = Timer.DelayCall( EffectDuration, new TimerCallback(
				delegate
				{
					m_Table.Remove( attacker );
				} ) );

			double skill = Math.Max( attacker.Skills.Bushido.Value, attacker.Skills.Ninjitsu.Value );

			int damageModifier = (int) ( skill * 50.0 / 120.0 );

			m_Table[attacker] = new FeintContext( expireTimer, defender, damageModifier );

			attacker.SendLocalizedMessage( 1063360 ); // You baffle your target with a feint!
			defender.SendLocalizedMessage( 1063361 ); // You were deceived by an attacker's feint!

			attacker.FixedParticles( 0x3728, 1, 13, 0x7F3, 0x962, 0, EffectLayer.Waist );

			BuffInfo.AddBuff( attacker, new BuffInfo( BuffIcon.Feint, 1151308, 1151307, EffectDuration, attacker, String.Format( "{0}\t{1}", defender.Name, damageModifier ) ) );
		}

		private static void AlterDamage( Mobile attacker, Mobile defender, ref int damage )
		{
			if ( m_Table.ContainsKey( defender ) )
			{
				FeintContext context = m_Table[defender];

				if ( context.Target == attacker )
					damage -= (int) ( damage * context.DamageModifier / 100.0 );
			}
		}
	}

	public class FeintContext
	{
		private Timer m_Timer;
		private Mobile m_Target;
		private int m_DamageModifier;

		public Timer Timer { get { return m_Timer; } }
		public Mobile Target { get { return m_Target; } }
		public int DamageModifier { get { return m_DamageModifier; } }

		public FeintContext( Timer timer, Mobile target, int damageModifier )
		{
			m_Timer = timer;
			m_Target = target;
			m_DamageModifier = damageModifier;
		}
	}
}