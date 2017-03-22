using System;
using Server;

namespace Server.Mobiles
{
	public abstract class SpecialAbility
	{
		public static SpecialAbility FlammableGoo = new FlammableGoo();
		public static SpecialAbility BloodDisease = new BloodDisease();

		public virtual void OnGotMeleeAttack( Mobile m, Mobile attacker )
		{
		}

		public virtual void OnGaveMeleeAttack( Mobile m, Mobile defender )
		{
		}
	}
}
