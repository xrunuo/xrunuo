using System;
using System.Collections;
using Server.Mobiles;
using Server.Engines.BuffIcons;

namespace Server.Items
{
	/// <summary>
	/// This attack allows you to disarm your foe.
	/// Now in Age of Shadows, a successful Disarm leaves the victim unable to re-arm another weapon for several seconds.
	/// </summary>
	public class Disarm : WeaponAbility
	{
		public Disarm()
		{
		}

		public override int BaseMana { get { return 20; } }

		public static readonly TimeSpan ImmunityDuration = TimeSpan.FromSeconds( 10.0 );

		public static Hashtable m_Table = new Hashtable();

		public static void BeginInmunity( Mobile m, TimeSpan duration )
		{
			Timer t = (Timer) m_Table[m];

			if ( t != null )
				t.Stop();

			m_Table[m] = t = Timer.DelayCall( duration, new TimerStateCallback( Expire_Callback ), m );

			if ( m is PlayerMobile )
				( (PlayerMobile) m ).NextDisarm = DateTime.Now + duration;
			else if ( m is BaseCreature )
				( (BaseCreature) m ).NextDisarm = DateTime.Now + duration;
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m_Table.Remove( m );
		}

		public static bool IsImmune( Mobile m )
		{
			DateTime date = DateTime.MinValue;

			if ( m is PlayerMobile )
				date = ( (PlayerMobile) m ).NextDisarm;
			if ( m is BaseCreature )
				date = ( (BaseCreature) m ).NextDisarm;

			if ( date < DateTime.Now )
				return false;

			return true;
		}

		public static readonly TimeSpan BlockEquipDuration = TimeSpan.FromSeconds( 5.0 );

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) )
				return;

			ClearCurrentAbility( attacker );

			Item toDisarm = defender.FindItemOnLayer( Layer.OneHanded );

			if ( toDisarm == null || !toDisarm.Movable )
				toDisarm = defender.FindItemOnLayer( Layer.TwoHanded );

			Container pack = defender.Backpack;

			if ( pack == null || ( toDisarm != null && !toDisarm.Movable ) )
			{
				attacker.SendLocalizedMessage( 1004001 ); // You cannot disarm your opponent.
			}
			else if ( toDisarm == null )
			{
				attacker.SendLocalizedMessage( 1060849 ); // Your target is already unarmed!
			}
			else if ( toDisarm is BaseShield )
			{
				attacker.SendLocalizedMessage( 1111822 ); // Your attempt to disarm your target has failed!
				defender.SendLocalizedMessage( 1111823 ); // Your attacker's attempt to disarm you has failed!
			}
			else if ( CheckMana( attacker, true ) )
			{
				if ( !IsImmune( defender ) )
				{
					attacker.SendLocalizedMessage( 1060092 ); // You disarm their weapon!
					defender.SendLocalizedMessage( 1060093 ); // Your weapon has been disarmed!

					defender.PlaySound( 0x3B9 );
					defender.FixedParticles( 0x37BE, 232, 25, 9948, EffectLayer.LeftHand );

					pack.DropItem( toDisarm );

					BuffInfo.AddBuff( defender, new BuffInfo( BuffIcon.NoRearm, 1075637, BlockEquipDuration, defender ) );

					BaseWeapon.BlockEquip( defender, BlockEquipDuration );
					if ( !( attacker.Weapon is Fists ) )
					{
						BeginInmunity( defender, ImmunityDuration );
					}
				}
				else
				{
					attacker.SendLocalizedMessage( 1154081 ); // your opponent cannot be disarmed at this time // 1060168: Your confusion has passed, you may now arm a weapon!
				}
			}
		}
	}
}