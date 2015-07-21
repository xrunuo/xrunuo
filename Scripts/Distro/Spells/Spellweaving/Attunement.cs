using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.BuffIcons;

namespace Server.Spells.Spellweaving
{
	public class AttunementSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Attunement", "Haeldril",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 24; } }

		public AttunementSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( m_Table.ContainsKey( Caster ) )
			{
				Caster.SendLocalizedMessage( 501775 ); // This spell is already in effect.
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( AttunementSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1075124 ); // You must wait before casting that spell again.
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.PlaySound( 0x5C3 );

				Caster.FixedParticles( 0x3728, 1, 13, 0x26B8, 0x455, 7, EffectLayer.Waist );
				Caster.FixedParticles( 0x3779, 1, 15, 0x251E, 0x3F, 7, EffectLayer.Waist );

				int focuslevel = GetFocusLevel( Caster );
				int skill = Caster.Skills[SkillName.Spellweaving].Fixed;

				int damageAbsorb = (int) ( 18 + ( ( skill - 100 ) / 100 ) * 3 + ( focuslevel * 6 ) );
				Caster.MeleeDamageAbsorb = damageAbsorb;

				TimeSpan duration = TimeSpan.FromSeconds( 60 + ( focuslevel * 12 ) );

				ExpireTimer t = new ExpireTimer( Caster, duration );
				t.Start();

				m_Table[Caster] = t;

				Caster.BeginAction( typeof( AttunementSpell ) );

				BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.AttuneWeapon, 1075798, duration, Caster, damageAbsorb.ToString() ) );
			}

			FinishSequence();
		}

		private static Hashtable m_Table = new Hashtable();

		public static void TryAbsorb( Mobile defender, ref int damage )
		{
			if ( damage == 0 || !IsAbsorbing( defender ) || defender.MeleeDamageAbsorb <= 0 )
				return;

			int absorbed = Math.Min( damage, defender.MeleeDamageAbsorb );

			damage -= absorbed;
			defender.MeleeDamageAbsorb -= absorbed;

			defender.SendLocalizedMessage( 1075127, String.Format( "{0}\t{1}", absorbed, defender.MeleeDamageAbsorb ) ); // ~1_damage~ point(s) of damage have been absorbed. A total of ~2_remaining~ point(s) of shielding remain.

			if ( defender.MeleeDamageAbsorb <= 0 )
				StopAbsorbing( defender, true );
		}

		public static bool IsAbsorbing( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public static void StopAbsorbing( Mobile m )
		{
			StopAbsorbing( m, false );
		}

		public static void StopAbsorbing( Mobile m, bool message )
		{
			if ( m_Table.ContainsKey( m ) )
				( (ExpireTimer) m_Table[m] ).DoExpire( message );
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;

			public ExpireTimer( Mobile m, TimeSpan delay )
				: base( delay )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				DoExpire( true );
			}

			public void DoExpire( bool message )
			{
				Stop();

				m_Mobile.MeleeDamageAbsorb = 0;

				if ( message )
				{
					m_Mobile.SendLocalizedMessage( 1075126 ); // Your attunement fades.
					m_Mobile.PlaySound( 0x1F8 );
				}

				m_Table.Remove( m_Mobile );

				Timer.DelayCall( TimeSpan.FromSeconds( 120 ), new TimerCallback( Expire_Callback ) );

				BuffInfo.RemoveBuff( m_Mobile, BuffIcon.AttuneWeapon );
			}

			public void Expire_Callback()
			{
				m_Mobile.EndAction( typeof( AttunementSpell ) );
			}
		}
	}
}