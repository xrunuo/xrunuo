using System;
using Server;
using Server.Mobiles;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
	/// <summary>
	/// If you are on foot, dismounts your opponent and damages the ethereal's rider or the live mount (which must be healed before ridden again).  If you are mounted, damages and stuns the mounted opponent. Requires Bushido skill.
	/// </summary>
	public class RidingSwipe : SEWeaponAbility
	{
		public static readonly TimeSpan BlockMountDuration = TimeSpan.FromSeconds( 10.0 ); // TODO: Taken from bola script, needs to be verified

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !defender.Mounted && !AnimalForm.UnderTransformation( defender ) && !defender.Flying )
			{
				attacker.SendLocalizedMessage( 1060848 ); // This attack only works on mounted or flying targets
				ClearCurrentAbility( attacker );
				return;
			}

			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			IMount mount = defender.Mount;

			if ( !attacker.Mounted )
			{
				defender.FixedParticles( 0x376A, 9, 32, 0x13AF, 0, 0, EffectLayer.RightFoot );

				if ( mount != null )
					mount.Rider = null;
				else if ( defender.Flying )
					defender.Flying = false;
				else
					AnimalForm.RemoveContext( defender, true );

				if ( mount is Mobile )
				{
					Mobile m = (Mobile) mount;

					AOS.Damage( m, attacker, (int) ( 0.4 * m.Hits ), 100, 0, 0, 0, 0 );
				}
			}
			else
			{
				int amount = 10 + (int) ( 10.0 * ( attacker.Skills[SkillName.Bushido].Value - 50.0 ) / 70.0 + 5 );

				AOS.Damage( defender, attacker, amount, 100, 0, 0, 0, 0 );

				ParalyzingBlow pb = new ParalyzingBlow();
				pb.IsBladeweaveAttack = true;
				pb.OnHit( attacker, defender, damage );
			}
		}
	}
}