using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
	/// <summary>
	/// The player infuses their throwing projectile with mystical power. The infused
	/// projectile will dismount the target if possible; otherwise it will temporarily
	/// stun the target. The target will be hit with chaos damage regardless of whether
	/// they were dismounted or paralyzed.
	/// </summary>
	public class InfusedThrow : WeaponAbility
	{
		public override int BaseMana { get { return 25; } }

		public override bool Validate( Mobile from )
		{
			if ( !base.Validate( from ) )
				return false;

			if ( AnimalForm.UnderTransformation( from ) )
			{
				from.SendLocalizedMessage( 1063024 ); // You cannot perform this special move right now.
				return false;
			}

			return true;
		}

		public override double GetDamageScalar( Mobile attacker, Mobile defender )
		{
			return 1.25 + ( Math.Max( 0.0, Math.Max( attacker.Skills[SkillName.Mysticism].Value, attacker.Skills[SkillName.Imbuing].Value ) - 80.0 ) / 100.0 );
		}

		public override void AlterDamageType( Mobile attacker, Mobile defender, ref int phys, ref int fire, ref int cold, ref int pois, ref int nrgy )
		{
			// Infused Throw overrides damage type, it's always Chaos Damage
			int[] types = new int[4];
			types[Utility.Random( types.Length )] = 100;

			phys = 0;
			fire = types[0];
			cold = types[1];
			pois = types[2];
			nrgy = types[3];
		}

		public static readonly TimeSpan DefenderRemountDelay = TimeSpan.FromSeconds( 8.0 );
		public static readonly TimeSpan AttackerRemountDelay = TimeSpan.FromSeconds( 10.0 );

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			if ( AnimalForm.UnderTransformation( attacker ) )
			{
				attacker.SendLocalizedMessage( 1063024 ); // You cannot perform this special move right now.
				return;
			}

			if ( !attacker.Flying && ( defender.Mounted || defender.Flying || AnimalForm.UnderTransformation( defender ) ) )
			{
				attacker.SendLocalizedMessage( 1060082 ); // The force of your attack has dislodged them from their mount!
				defender.SendLocalizedMessage( 1060083 ); // You fall off of your mount and take damage!

				defender.PlaySound( 0x140 );
				defender.FixedParticles( 0x3728, 10, 15, 9955, EffectLayer.Waist );

				IMount mount = defender.Mount;

				if ( mount != null )
					mount.Rider = null;
				else if ( defender.Flying )
					defender.Flying = false;
				else
					AnimalForm.RemoveContext( defender, true );

				BaseMount.SetMountPrevention( defender, BlockMountType.Dazed, DefenderRemountDelay );
				BaseMount.SetMountPrevention( attacker, BlockMountType.DismountRecovery, AttackerRemountDelay );

				AOS.Damage( defender, attacker, Utility.RandomMinMax( 15, 25 ), 100, 0, 0, 0, 0 );
			}
			else
			{
				if ( !Items.ParalyzingBlow.IsInmune( defender ) )
				{
					attacker.SendLocalizedMessage( 1060163 ); // You deliver a paralyzing blow!
					defender.SendLocalizedMessage( 1072221 ); // You have been hit by a paralyzing blow!

					defender.Freeze( TimeSpan.FromSeconds( 2.0 ) );

					Items.ParalyzingBlow.BeginInmunity( defender );
				}
				else
				{
					attacker.SendLocalizedMessage( 1070804 ); // Your target resists paralysis.
					defender.SendLocalizedMessage( 1070813 ); // You resist paralysis.
				}

				defender.FixedEffect( 0x376A, 9, 32 );
				defender.PlaySound( 0x204 );
			}
		}
	}
}