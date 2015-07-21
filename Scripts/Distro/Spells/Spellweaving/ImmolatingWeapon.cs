using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Spells.Spellweaving
{
	public class ImmolatingWeaponSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Immolating Weapon", "Thalshara",
				-1, 9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

		public override double RequiredSkill { get { return 10.0; } }
		public override int RequiredMana { get { return 32; } }

		public ImmolatingWeaponSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			BaseWeapon weapon = Caster.Weapon as BaseWeapon;

			if ( weapon == null || weapon is Fists || weapon is BaseRanged )
			{
				Caster.SendLocalizedMessage( 1060179 ); // You must be wielding a weapon to use this ability!
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			BaseWeapon weapon = Caster.Weapon as BaseWeapon;

			if ( weapon == null || weapon is Fists || weapon is BaseRanged )
			{
				Caster.SendLocalizedMessage( 1060179 ); // You must be wielding a weapon to use this ability!
			}
			else if ( CheckSequence() )
			{
				Caster.PlaySound( 0x5CA );
				Caster.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );

				int focuslevel = GetFocusLevel( Caster );
				int skill = Caster.Skills[SkillName.Spellweaving].Fixed;

				int duration = 10 + (int) ( Math.Floor( skill / 240.0 ) ) + focuslevel;
				int damage = 5 + (int) ( Math.Floor( skill / 240.0 ) ) + focuslevel;

				ImmolatingWeaponEntry iEntry = m_Table[weapon] as ImmolatingWeaponEntry;

				if ( iEntry != null )
					iEntry.m_Timer.Stop();

				weapon.Immolating = true;

				m_Table[weapon] = new ImmolatingWeaponEntry( damage, weapon, Caster, duration );
			}

			FinishSequence();
		}

		private static void EndImmolating_Callback( object state )
		{
			BaseWeapon weapon = (BaseWeapon) state;

			EndImmolating( weapon, true );
		}

		public static bool IsImmolating( Mobile m )
		{
			return m.Weapon is BaseWeapon && ( (BaseWeapon) m.Weapon ).Immolating;
		}

		public static void EndImmolating( Mobile m )
		{
			if ( m.Weapon is BaseWeapon )
				EndImmolating( m.Weapon as BaseWeapon, false );
		}

		public static void EndImmolating( BaseWeapon weapon, bool playSound )
		{
			ImmolatingWeaponEntry iEntry = m_Table[weapon] as ImmolatingWeaponEntry;

			if ( iEntry != null )
			{
				iEntry.m_Timer.Stop();

				if ( playSound )
					iEntry.m_Owner.PlaySound( 0x27 );
			}

			weapon.Immolating = false;
			m_Table.Remove( weapon );
		}

		private static Hashtable m_Table = new Hashtable();

		public class ImmolatingWeaponEntry
		{
			public int m_Damage;
			public Timer m_Timer;
			public Mobile m_Owner;

			public ImmolatingWeaponEntry( int damage, BaseWeapon weapon, Mobile owner, int duration )
			{
				m_Damage = damage;
				m_Owner = owner;

				m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( duration ), new TimerStateCallback( EndImmolating_Callback ), weapon );
			}
		}

		public static int GetImmolatingDamage( BaseWeapon w )
		{
			ImmolatingWeaponEntry iEntry = m_Table[w] as ImmolatingWeaponEntry;

			if ( iEntry != null )
				return iEntry.m_Damage;

			return 0;
		}
	}
}