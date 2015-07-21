using System;
using Server.Misc;

namespace Server.Items
{
	/// <summary>
	/// Fires a snake at the target, poisoning them in addition to normal damage with a successful hit. 
	/// The archer must be skilled in poisoning and nimble of hand to achieve success.
	/// </summary>
	public class SerpentArrow : WeaponAbility
	{
		public SerpentArrow()
		{
		}

		public override int BaseMana { get { return 40; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) )
				return;

			ClearCurrentAbility( attacker );

			BaseWeapon weapon = attacker.Weapon as BaseWeapon;

			if ( weapon == null )
				return;

			if ( !CheckMana( attacker, true ) )
				return;

			int level = attacker.Skills[SkillName.Poisoning].Fixed / 250;
			Poison p = Poison.GetPoison( level );

			defender.PlaySound( 0xDD );
			defender.FixedParticles( 0x3728, 244, 25, 9941, 1266, 0, EffectLayer.Waist );
			
			if ( defender.ApplyPoison( attacker, p ) != ApplyPoisonResult.Immune )
			{
				attacker.SendLocalizedMessage( 1008096, true, defender.Name ); // You have poisoned your target : 
				defender.SendLocalizedMessage( 1008097, false, attacker.Name ); //  : poisoned you!

				Titles.AwardKarma( attacker, -10, true );
			}
		}
	}
}