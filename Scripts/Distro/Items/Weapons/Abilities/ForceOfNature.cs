using System;

namespace Server.Items
{
	/// <summary>
	/// Enfuses the attacker with Nature's Fury, granting them unparalleled strength. This dangerous channeling causes leafy vines to violently erupt from beneath the attacker's skin, dealing substantial physical and poison damage to them.
	/// </summary>
	public class ForceOfNature : WeaponAbility
	{
		public ForceOfNature()
		{
		}

		public override int BaseMana { get { return 40; } }

		public override double GetDamageScalar( Mobile attacker, Mobile defender )
		{
			return 1.65;
		}

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			attacker.SendLocalizedMessage( 1074374 ); // You attack your enemy with the force of nature!
			defender.SendLocalizedMessage( 1074375 ); // You are assaulted with great force!

			AOS.Damage( attacker, Utility.RandomMinMax( 25, 35 ), 0, 0, 0, 100, 0 );

			defender.PlaySound( 0x5BE );

			Effects.SendLocationEffect( defender.Location, defender.Map, 0x374A, 17, 1, 0x45, 0x5 );
		}
	}
}