using System;
using Server;

namespace Server.Mobiles
{
	public abstract class SpecialAbility
	{
		public static SpecialAbility FlammableGoo = new FlammableGoo();

		public virtual void OnGaveMeleeAttack( Mobile attacker, Mobile defender )
		{
		}
	}
}
