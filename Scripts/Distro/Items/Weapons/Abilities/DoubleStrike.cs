using System;

namespace Server.Items
{
	/// <summary>
	/// The highly skilled warrior can use this special attack to make two quick swings in succession.
	/// Landing both blows would be devastating! 
	/// </summary>
	public class DoubleStrike : WeaponAbility
	{
		public override int BaseMana { get { return 30; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			Use( attacker, defender );
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
			Use( attacker, defender );
		}

		public override double GetDamageScalar( Mobile attacker, Mobile defender )
		{
			return 0.9;
		}

		public override double GetAccuracyScalar( Mobile attacker, Mobile defender )
		{
			return 0.7;
		}

		private void Use( Mobile attacker, Mobile defender )
		{
			if ( !IsBladeweaveAttack )
			{
				if ( !Validate( attacker ) )
					return;
				if ( !CheckMana( attacker, true ) )
					return;
			}

			ClearCurrentAbility( attacker );

			attacker.SendLocalizedMessage( 1060084 ); // You attack with lightning speed!
			defender.SendLocalizedMessage( 1060085 ); // Your attacker strikes with lightning speed!

			defender.PlaySound( 0x3BB );
			defender.FixedEffect( 0x37B9, 244, 25 );

			attacker.Weapon.OnSwing( attacker, defender );
		}
	}
}