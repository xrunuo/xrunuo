using System;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Engines.BuffIcons;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.SkillHandlers;
using Server.Targeting;

namespace Server.Spells.Ninjitsu
{
	public class DeathStrike : NinjaMove
	{
		public static readonly TimeSpan DamageDelay = TimeSpan.FromSeconds( 3.0 );

		public DeathStrike()
		{
		}

		public override int BaseMana { get { return 30; } }
		public override double RequiredSkill { get { return 85.0; } }

		public override TextDefinition AbilityMessage { get { return new TextDefinition( 1063091 ); } } // You prepare to hit your opponent with a Death Strike.

		public override double GetDamageScalar( Mobile attacker, Mobile defender )
		{
			return 0.5;
		}

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentMove( attacker );

			if ( !CheckGain( attacker ) )
			{
				attacker.SendLocalizedMessage( 1070779 ); // You missed your opponent with a Death Strike.
				return;
			}

			DeathStrikeInfo info;

			if ( m_Table.ContainsKey( defender ) )
			{
				defender.SendLocalizedMessage( 1063092 ); // Your opponent lands another Death Strike!

				info = m_Table[defender];

				if ( info.m_Timer != null )
					info.m_Timer.Stop();

				m_Table.Remove( defender );
			}
			else
			{
				defender.SendLocalizedMessage( 1063093 ); // You have been hit by a Death Strike!  Move with caution!
			}

			attacker.SendLocalizedMessage( 1063094 ); // You inflict a Death Strike upon your opponent!

			defender.FixedParticles( 0x374A, 1, 17, 0x26BC, EffectLayer.Waist );
			attacker.PlaySound( attacker.Female ? 0x50D : 0x50E );

			info = new DeathStrikeInfo( defender, attacker );
			info.m_Timer = Timer.DelayCall( DamageDelay, () => ProcessDeathStrike( info ) );

			m_Table[defender] = info;

			int buffdamage = (int) ( attacker.Skills[SkillName.Ninjitsu].Value / 3 );
			BuffInfo.AddBuff( defender, new BuffInfo( BuffIcon.DeathStrike, 1075645, DamageDelay, defender, String.Format( "{0}", buffdamage ) ) );
		}

		private static Dictionary<Mobile, DeathStrikeInfo> m_Table = new Dictionary<Mobile, DeathStrikeInfo>();

		private class DeathStrikeInfo
		{
			public Mobile m_Target;
			public Mobile m_Attacker;
			public int m_Steps;
			public bool m_IsRanged;
			public Timer m_Timer;

			public DeathStrikeInfo( Mobile target, Mobile attacker )
			{
				m_Target = target;
				m_Attacker = attacker;
				m_IsRanged = attacker.Weapon is BaseRanged || attacker.Weapon is BaseThrowing;
			}
		}

		public static void AddStep( Mobile m )
		{
			if ( !m_Table.ContainsKey( m ) )
				return;

			DeathStrikeInfo info = m_Table[m];

			info.m_Steps++;

			if ( info.m_Steps >= 5 )
				ProcessDeathStrike( info );
		}

		private static void ProcessDeathStrike( DeathStrikeInfo info )
		{
			double ninjitsu = info.m_Attacker.Skills[SkillName.Ninjitsu].Value;
			double hiding = info.m_Attacker.Skills[SkillName.Hiding].Value;
			double stealth = info.m_Attacker.Skills[SkillName.Stealth].Value;

			double scalar = 1.0 + ( hiding + stealth ) / 100;
			double damage = scalar * ( ninjitsu / 8 );

			if ( info.m_IsRanged )
				damage /= 2;

			if ( info.m_Steps < 5 )
				damage /= 3;

			info.m_Target.Damage( (int) damage, info.m_Attacker );

			if ( info.m_Timer != null )
			{
				info.m_Timer.Stop();
				info.m_Timer = null;
			}

			m_Table.Remove( info.m_Target );
		}
	}
}