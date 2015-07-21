using System;
using System.Collections;
using Server.Mobiles;

namespace Server.Items
{
	/// <summary>
	/// A successful Paralyzing Blow will leave the target stunned, unable to move, attack, or cast spells, for a few seconds.
	/// </summary>
	public class ParalyzingBlow : WeaponAbility
	{
		public ParalyzingBlow()
		{
		}

		public override int BaseMana { get { return 30; } }

		public static readonly TimeSpan PlayerFreezeDuration = TimeSpan.FromSeconds( 3.0 );
		public static readonly TimeSpan NPCFreezeDuration = TimeSpan.FromSeconds( 6.0 );
		public static readonly TimeSpan InmunityDuration = TimeSpan.FromSeconds( 8.0 );

		public static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m_Table.Remove( m );
		}

		public static bool IsInmune( Mobile m )
		{
			DateTime date = DateTime.MinValue;

			if ( m is PlayerMobile )
				date = ( (PlayerMobile) m ).NextPBlow;
			if ( m is BaseCreature )
				date = ( (BaseCreature) m ).NextPBlow;

			if ( date < DateTime.Now )
				return false;

			return true;
		}

		public static void BeginInmunity( Mobile m )
		{
			BeginInmunity( m, InmunityDuration + ( m.IsPlayer ? PlayerFreezeDuration : NPCFreezeDuration ) );
		}

		public static void BeginInmunity( Mobile m, TimeSpan duration )
		{
			Timer t = (Timer) m_Table[m];

			if ( t != null )
				t.Stop();

			m_Table[m] = t = Timer.DelayCall( duration, new TimerStateCallback( Expire_Callback ), m );

			if ( m is PlayerMobile )
				( (PlayerMobile) m ).NextPBlow = DateTime.Now + duration;
			else if ( m is BaseCreature )
				( (BaseCreature) m ).NextPBlow = DateTime.Now + duration;
		}

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( defender.Frozen || UnderEffect( defender ) )
			{
				attacker.SendLocalizedMessage( 1061923 ); // The target is already frozen.
				return;
			}

			if ( !IsBladeweaveAttack )
			{
				if ( !Validate( attacker ) )
					return;
				if ( !CheckMana( attacker, true ) )
					return;
			}

			ClearCurrentAbility( attacker );

			if ( !IsInmune( defender ) )
			{
				attacker.SendLocalizedMessage( 1060163 ); // You deliver a paralyzing blow!
				defender.SendLocalizedMessage( 1072221 ); // You have been hit by a paralyzing blow!

				defender.Freeze( defender.IsPlayer ? PlayerFreezeDuration : NPCFreezeDuration );

				BeginInmunity( defender );
			}
			else
			{
				attacker.SendLocalizedMessage( 1063346 ); // Your attack was blocked!
				defender.SendLocalizedMessage( 1070813 ); // You resist paralysis.
			}

			defender.FixedEffect( 0x376A, 9, 32 );
			defender.PlaySound( 0x204 );
		}
	}
}