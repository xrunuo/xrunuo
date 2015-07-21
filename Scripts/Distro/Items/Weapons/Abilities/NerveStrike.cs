using System;
using Server;

namespace Server.Items
{
	/// <summary>
	/// Does damage and paralyzes your opponent for a short time. Requires Bushido skill.
	/// </summary>
	public class NerveStrike : SEWeaponAbility
	{
		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			defender.Damage( (int) ( 15.0 * ( attacker.Skills[SkillName.Bushido].Value - 50.0 ) / 70.0 + Utility.RandomMinMax( 1, 10 ) ), attacker );

			double chance = (double) ( ( ( 4 * attacker.Skills[SkillName.Bushido].Value ) + 150 ) / 700 );

			if ( chance < Utility.RandomDouble() )
			{
				attacker.SendLocalizedMessage( 1070804 ); // Your target resists paralysis.
				return;
			}

			if ( Server.Items.ParalyzingBlow.IsInmune( defender ) )
			{
				attacker.SendLocalizedMessage( 1070804 ); // Your target resists paralysis.
				defender.SendLocalizedMessage( 1070813 ); // You resist paralysis.

				return;
			}

			attacker.SendLocalizedMessage( 1063356 ); // You cripple your target with a nerve strike!
			defender.SendLocalizedMessage( 1063357 ); // Your attacker dealt a crippling nerve strike!

			attacker.FixedParticles( 0x37C4, 1, 8, 0x13AF, 0, 0, EffectLayer.Waist );
			defender.FixedEffect( 0x376A, 9, 32 );
			defender.PlaySound( 0x204 );

			defender.Freeze( TimeSpan.FromSeconds( 2.0 ) );

			Server.Items.ParalyzingBlow.BeginInmunity( defender, TimeSpan.FromSeconds( 10.0 ) );
		}
	}
}