using System;
using Server.Items;
using Server.Misc;
using Server.Engines.BuffIcons;

namespace Server.SkillHandlers
{
	class Meditation
	{
		public static void Initialize()
		{
			SkillInfo.Table[46].Callback = new SkillUseCallback( OnUse );
		}

		public static bool CheckOkayHolding( Item item )
		{
			if ( item == null )
				return true;

			if ( item is Spellbook || item is Runebook )
				return true;

			if ( item is BaseWeapon && ( (BaseWeapon) item ).Attributes.SpellChanneling != 0 )
				return true;

			if ( item is BaseArmor && ( (BaseArmor) item ).Attributes.SpellChanneling != 0 )
				return true;

			if ( item is WoodenShield && ( ( (WoodenShield) item ).Resource == CraftResource.Frostwood ) )
				return true;

			return false;
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.RevealingAction();

			if ( m.Target != null )
			{
				m.SendLocalizedMessage( 501845 ); // You are busy doing something else and cannot focus.

				return TimeSpan.FromSeconds( 5.0 );
			}
			else if ( m.Mana >= m.ManaMax )
			{
				m.SendLocalizedMessage( 501846 ); // You are at peace.

				return TimeSpan.FromSeconds( 10.0 );
			}
			else if ( !RegenRates.AllowMeditation( m ) )
			{
				m.SendLocalizedMessage( 500135 ); // Regenative forces cannot penetrate your armor!

				return TimeSpan.FromSeconds( 10.0 );
			}
			else
			{
				Item oneHanded = m.FindItemOnLayer( Layer.OneHanded );
				Item twoHanded = m.FindItemOnLayer( Layer.TwoHanded );

				if ( !CheckOkayHolding( oneHanded ) )
					m.AddToBackpack( oneHanded );

				if ( !CheckOkayHolding( twoHanded ) )
					m.AddToBackpack( twoHanded );

				double skillVal = m.Skills[SkillName.Meditation].Value;
				double chance = ( 75.0 + ( ( skillVal - ( m.ManaMax - m.Mana ) ) * 2 ) ) / 100;

				if ( chance > Utility.RandomDouble() )
				{
					m.CheckSkill( SkillName.Meditation, 0.0, 100.0 );

					m.SendLocalizedMessage( 501851 ); // You enter a meditative trance.
					m.Meditating = true;

					BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.ActiveMeditation, 1075657 ) );

					if ( m.Player || m.Body.IsHuman )
						m.PlaySound( 0xF9 );
				}
				else
				{
					m.SendLocalizedMessage( 501850 ); // You cannot focus your concentration.
				}

				return TimeSpan.FromSeconds( 10.0 );
			}
		}
	}
}