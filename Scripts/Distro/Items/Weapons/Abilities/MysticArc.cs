using System;
using Server;
using System.Collections;

namespace Server.Items
{
	/// <summary>
	/// The thrower augments the spinning of their projectile with mystic force
	/// causing it to strike a second target after first hitting the primary target.
	/// Some of the energy used to increase the projectiles spin is transferred to
	/// each target causing additional energy damage.
	/// </summary>
	public class MysticArc : WeaponAbility
	{
		public override int BaseMana { get { return 30; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			/*
			 * - Upon striking the primary target with full force the projectile is deflected to one additional target which is struck for half damage.
			 * - The primary target receives a 100% damage and the secondary target receives 50% damage strike.
			 * - Additional chaos damage is dealt to each target, modified by the player’s Mysticism or Imbuing skill
			 *     o The chaos damage is a random roll between 15-27
			 *     o If the character has Mysticism or Imbuing above 80, they will receive additional bonus to the range of their potential chaos damage of 1% per point above 80, up to a maximum 40% increase to the minimum and maximums in the range
			 *          + The range 15 - 27 can be raised up to 21 – 38
			 * - Player must already have established combat with the secondary target: either by attacking or having been a victim of the target
			 *     o Will not initiate combat with otherwise valid targets who aren’t already in combat with you
			 */

			double imbuing = attacker.Skills[SkillName.Imbuing].Value;
			double mysticism = attacker.Skills[SkillName.Mysticism].Value;

			double skillBonus = Math.Max( 0, Math.Max( imbuing, mysticism ) - 80.0 );

			int additionalDamage = AOS.Scale( Utility.RandomMinMax( 15, 27 ), (int) skillBonus );

			int[] types = new int[4];
			types[Utility.Random( types.Length )] = 100;

			AOS.Damage( defender, attacker, additionalDamage, 0, types[0], types[1], types[2], types[3] );

			BaseWeapon weapon = attacker.Weapon as BaseWeapon;
			Mobile secondTarget = null;

			foreach ( Mobile m in attacker.GetMobilesInRange( weapon.MaxRange ) )
			{
				if ( m != defender && m.Combatant == attacker )
				{
					secondTarget = m;
					break;
				}
			}

			if ( secondTarget != null )
			{
				int phys, fire, cold, pois, nrgy;
				weapon.GetDamageTypes( attacker, out phys, out fire, out cold, out pois, out nrgy );

				AOS.Damage( secondTarget, attacker, damage / 2, phys, fire, cold, pois, nrgy );

				additionalDamage = AOS.Scale( Utility.RandomMinMax( 15, 27 ), (int) skillBonus );

				types = new int[4];
				types[Utility.Random( types.Length )] = 100;

				AOS.Damage( secondTarget, attacker, additionalDamage, 0, types[0], types[1], types[2], types[3] );
			}
		}
	}
}