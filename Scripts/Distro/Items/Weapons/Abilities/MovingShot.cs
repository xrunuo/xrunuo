using System;

namespace Server.Items
{
	/// <summary>
	/// Available on some crossbows, this special move allows archers to fire while on the move.
	/// This shot is somewhat less accurate than normal, but the ability to fire while running is a clear advantage.
	/// </summary>
	public class MovingShot : WeaponAbility
	{
		public MovingShot()
		{
		}

		public override int BaseMana { get { return 15; } }

		public override double GetAccuracyScalar( Mobile attacker, Mobile defender )
		{
			return 0.75;
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			attacker.SendLocalizedMessage( 1060089 ); // You fail to execute your special move
		}

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			attacker.SendLocalizedMessage( 1060216 ); // Your shot was successful
		}
	}
}