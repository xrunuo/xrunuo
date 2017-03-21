using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Events;

namespace Server.Engines.Macros
{
	public class EquipLastWeapon
	{
		public static void Initialize()
		{
			EventSink.EquipLastWeaponMacroUsed += new EquipLastWeaponMacroEventHandler( EventSink_EquipLastWeaponMacroUsed );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m.EndAction( typeof( EquipLastWeapon ) );
		}

		private static void EventSink_EquipLastWeaponMacroUsed( EquipLastWeaponMacroEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( !pm.CanBeginAction( typeof( EquipLastWeapon ) ) )
			{
				pm.SendLocalizedMessage( 500119 ); // You must wait to perform another action.
				return;
			}

			pm.BeginAction( typeof( EquipLastWeapon ) );

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Expire_Callback ), pm );

			if ( pm != null && pm.Backpack != null )
			{
				BaseWeapon weapon = pm.LastEquipedWeapon;
				Item hand1 = pm.FindItemOnLayer( Layer.OneHanded );
				Item hand2 = pm.FindItemOnLayer( Layer.TwoHanded );

				if ( weapon != null )
				{
					if ( !weapon.IsChildOf( pm.Backpack ) )
					{
						pm.SendLocalizedMessage( 1063109 ); // Your last weapon must be in your backpack to be able to switch it quickly.
						return;
					}

					if ( weapon.Layer == Layer.TwoHanded && hand2 is BaseShield )
					{
						pm.SendLocalizedMessage( 1063114 ); // You cannot pick up your last weapon!
						return;
					}

					Item olditem = hand2 is BaseWeapon ? hand2 : hand1;

					if ( olditem != null && !pm.AddToBackpack( olditem ) )
					{
						pm.SendLocalizedMessage( 1063110 ); // Your backpack cannot hold the weapon in your hand.
					}
					else if ( pm.EquipItem( weapon ) )
					{
						pm.SendLocalizedMessage( 1063112 ); // You pick up your last weapon.
						pm.LastEquipedWeapon = olditem as BaseWeapon;
					}
					else
					{
						pm.SendLocalizedMessage( 1063113 ); // You put your weapon into your backpack, but cannot pick up your last weapon!
					}
				}
			}
		}
	}
}