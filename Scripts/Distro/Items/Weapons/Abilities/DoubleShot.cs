using System;
using Server;

namespace Server.Items
{
	/// <summary>
	/// Send two arrows flying at your opponent if you're mounted. Requires Bushido or Ninjitsu skill.
	/// </summary>
	public class DoubleShot : SEWeaponAbility
	{
		public override bool Validate( Mobile from )
		{
			if ( !from.Mounted && !from.Flying )
			{
				from.SendLocalizedMessage( 1070770 ); // You can only execute this attack while mounted or flying!
				return false;
			}

			return base.Validate( from );
		}

		public override double GetDamageScalar( Mobile attacker, Mobile defender )
		{
			return 0.9;
		}

		public override double GetAccuracyScalar( Mobile attacker, Mobile defender )
		{
			return 0.7;
		}

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			Use( attacker, defender );
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
			Use( attacker, defender );
		}

		private void Use( Mobile attacker, Mobile defender )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) || attacker.Weapon == null )
				return;

			ClearCurrentAbility( attacker );

			attacker.SendLocalizedMessage( 1063348 ); // You launch two shots at once!
			defender.SendLocalizedMessage( 1063349 ); // You're attacked with a barrage of shots!

			defender.FixedParticles( 0x37B9, 1, 19, 0x251D, EffectLayer.Waist );

			attacker.Weapon.OnSwing( attacker, defender );
		}
	}
}