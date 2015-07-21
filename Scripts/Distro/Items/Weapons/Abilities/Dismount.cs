using System;
using Server.Mobiles;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
	/// <summary>
	/// Perfect for the foot-soldier, the Dismount special attack can unseat a mounted opponent.
	/// The fighter using this ability must be on his own two feet and not in the saddle of a steed
	/// (with one exception: players may use a lance to dismount other players while mounted).
	/// If it works, the target will be knocked off his own mount and will take some extra damage from the fall!
	/// </summary>
	public class Dismount : WeaponAbility
	{
		public Dismount()
		{
		}

		public override int BaseMana { get { return 20; } }

		public override bool Validate( Mobile from )
		{
			if ( !base.Validate( from ) )
				return false;

			if ( ( from.Mounted || from.Flying ) && !( from.Weapon is Lance ) )
			{
				from.SendLocalizedMessage( 1061283 ); // You cannot perform that attack while mounted or flying!
				return false;
			}

			return true;
		}

		public static readonly TimeSpan DefenderRemountDelay = TimeSpan.FromSeconds( 8.0 );
		public static readonly TimeSpan AttackerRemountDelay = TimeSpan.FromSeconds( 8.0 );

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( attacker is LesserHiryu && 0.2 < Utility.RandomDouble() )
				return;

			if ( !Validate( attacker ) )
				return;

			if ( attacker.Mounted && !( defender.Weapon is Lance ) ) // TODO: Should there be a message here?
				return;

			ClearCurrentAbility( attacker );

			if ( defender is Neira || defender is ChaosDragoonElite )
			{
				attacker.SendLocalizedMessage( 1042047 ); // You fail to knock the rider from its mount.
				return;
			}

			if ( AnimalForm.UnderTransformation( attacker ) )
			{
				attacker.SendLocalizedMessage( 1070902 ); // You can't use this while in an animal form!
				return;
			}

			IMount mount = defender.Mount;

			if ( mount == null && !AnimalForm.UnderTransformation( defender ) && !defender.Flying )
			{
				attacker.SendLocalizedMessage( 1060848 ); // This attack only works on mounted or flying targets
				return;
			}

			if ( !CheckMana( attacker, true ) )
				return;

			attacker.SendLocalizedMessage( 1060082 ); // The force of your attack has dislodged them from their mount!

			if ( attacker.Mounted )
				defender.SendLocalizedMessage( 1062315 ); // You fall off your mount!
			else
				defender.SendLocalizedMessage( 1060083 ); // You fall off of your mount and take damage!

			defender.PlaySound( 0x140 );
			defender.FixedParticles( 0x3728, 10, 15, 9955, EffectLayer.Waist );

			if ( mount != null )
				mount.Rider = null;
			else if ( defender.Flying )
				defender.Flying = false;
			else
				AnimalForm.RemoveContext( defender, true );

			BaseMount.SetMountPrevention( defender, BlockMountType.Dazed, DefenderRemountDelay );
			BaseMount.SetMountPrevention( attacker, BlockMountType.DismountRecovery, AttackerRemountDelay );

			if ( !attacker.Mounted )
				AOS.Damage( defender, attacker, Utility.RandomMinMax( 15, 25 ), 100, 0, 0, 0, 0 );
		}
	}
}