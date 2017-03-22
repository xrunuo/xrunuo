using System;
using System.Collections;
using Server.Mobiles;
using Server.Spells.Necromancy;
using Server.Network;
using Server.Engines.BuffIcons;
using Server.Spells.Bard;

namespace Server.Items
{
	/// <summary>
	/// Make your opponent bleed profusely with this wicked use of your weapon.
	/// When successful, the target will bleed for several seconds, taking damage as time passes for up to ten seconds.
	/// The rate of damage slows down as time passes, and the blood loss can be completely staunched with the use of bandages. 
	/// </summary>
	public class BleedAttack : WeaponAbility
	{
		private static readonly int DurationSeconds = 10;

		public BleedAttack()
		{
		}

		public override int BaseMana { get { return 30; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !IsBladeweaveAttack )
			{
				if ( !Validate( attacker ) )
					return;
				if ( !CheckMana( attacker, true ) )
					return;
			}

			ClearCurrentAbility( attacker );

			if ( defender is BaseCreature )
			{
				if ( defender != null && ( (BaseCreature) defender ).BleedImmune )
				{
					attacker.SendLocalizedMessage( 1062052 ); // Your target is not affected by the bleed attack!

					return;
				}
			}

			TransformContext context = TransformationSpell.GetContext( defender );

			// Necromancers under Lich Form or Wraith Form are immune to Bleed Attacks.
			if ( context == null || ( context.Type != typeof( LichFormSpell ) && context.Type != typeof( WraithFormSpell ) ) )
			{
				attacker.SendLocalizedMessage( 1060159 ); // Your target is bleeding!
				defender.SendLocalizedMessage( 1060160 ); // You are bleeding!

				if ( defender is PlayerMobile )
				{
					defender.LocalOverheadMessage( MessageType.Regular, 0x21, 1060757 ); // You are bleeding profusely
					defender.NonlocalOverheadMessage( MessageType.Regular, 0x21, 1060758, defender.Name ); // ~1_NAME~ is bleeding profusely
				}

				defender.PlaySound( 0x133 );
				defender.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist );

				TimeSpan duration = ComputeDuration( defender );

				BuffInfo.AddBuff( defender, new BuffInfo( BuffIcon.Bleed, 1075829, duration, defender, String.Format( "{0}\t{1}\t{2}", 5, 10, 2 ) ) );

				BeginBleed( defender, attacker, duration );
			}
			else
			{
				attacker.SendLocalizedMessage( 1062052 ); // Your target is not affected by the bleed attack!
			}
		}

		private static TimeSpan ComputeDuration( Mobile defender )
		{
			double seconds = DurationSeconds;

			Resilience song = Spellsong.GetEffectSpellsong<Resilience>( defender );

			if ( song != null )
				seconds = seconds - ( song.CurseReduction * seconds / 100.0 );

			return TimeSpan.FromSeconds( seconds );
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool IsBleeding( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void BeginBleed( Mobile m, Mobile from, TimeSpan duration )
		{
			Timer t = (Timer) m_Table[m];

			if ( t != null )
				t.Stop();

			t = new InternalTimer( from, m, duration );
			m_Table[m] = t;

			t.Start();
		}

		public static void DoBleed( Mobile m, Mobile from, int level )
		{
			if ( m.Alive )
			{
				int damage = Utility.RandomMinMax( level, level * 2 );

				if ( !m.Player )
					damage *= 2;

				m.PlaySound( 0x133 );
				m.Damage( damage, from );

				Blood blood = new Blood();
				blood.ItemID = Utility.Random( 0x122A, 5 );
				blood.MoveToWorld( m.Location, m.Map );

				BaseWeapon weapon = from.Weapon as BaseWeapon;

				if ( weapon != null && weapon.WeaponAttributes[AosWeaponAttribute.BloodDrinker] != 0 && from.Hits < from.HitsMax )
				{
					from.FixedParticles( 0x375A, 1, 10, 0x1AE9, 0x21, 0x2, EffectLayer.Head );
					from.SendLocalizedMessage( 1113606 ); // The blood drinker effect heals you.

					from.Hits += damage;
				}
			}
			else
			{
				EndBleed( m, false );
			}
		}

		public static void EndBleed( Mobile m, bool message )
		{
			Timer t = (Timer) m_Table[m];

			if ( t == null )
				return;

			t.Stop();
			m_Table.Remove( m );

			if ( message )
				m.SendLocalizedMessage( 1060167 ); // The bleeding wounds have healed, you are no longer bleeding!

			BuffInfo.RemoveBuff( m, BuffIcon.Bleed );
		}

		private class InternalTimer : Timer
		{
			private readonly Mobile m_From;
			private readonly Mobile m_Mobile;
			private readonly int m_MaxCount;

			private int m_Count;

			public InternalTimer( Mobile from, Mobile m, TimeSpan duration )
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_From = from;
				m_Mobile = m;

				m_MaxCount = (int) Math.Ceiling( duration.TotalSeconds / 2 );
			}

			protected override void OnTick()
			{
				DoBleed( m_Mobile, m_From, 5 - m_Count );

				if ( ++m_Count == m_MaxCount )
					EndBleed( m_Mobile, true );
			}
		}
	}
}