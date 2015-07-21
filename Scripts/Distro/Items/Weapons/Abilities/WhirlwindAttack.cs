using System;
using System.Collections;
using Server;
using Server.Spells;
using Server.Engines.PartySystem;

namespace Server.Items
{
	/// <summary>
	/// A godsend to a warrior surrounded, the Whirlwind Attack allows the fighter to strike at all nearby targets in one mighty spinning swing.
	/// </summary>
	public class WhirlwindAttack : WeaponAbility
	{
		public WhirlwindAttack()
		{
		}

		public override int BaseMana { get { return 15; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) )
				return;

			ClearCurrentAbility( attacker );

			Map map = attacker.Map;

			if ( map == null )
				return;

			BaseWeapon weapon = attacker.Weapon as BaseWeapon;

			if ( weapon == null )
				return;

			if ( !CheckMana( attacker, true ) )
				return;

			attacker.FixedEffect( 0x3728, 10, 15 );
			attacker.PlaySound( 0x2A1 );

			ArrayList list = new ArrayList();

			foreach ( Mobile m in attacker.GetMobilesInRange( 1 ) )
				list.Add( m );

			Party p = Party.Get( attacker );

			for ( int i = 0; i < list.Count; ++i )
			{
				Mobile m = (Mobile) list[i];

				if ( m != defender && m != attacker && SpellHelper.ValidIndirectTarget( attacker, m ) && ( p == null || !p.Contains( m ) ) )
				{
					if ( m == null || m.Deleted || attacker.Deleted || m.Map != attacker.Map || !m.Alive || !attacker.Alive || !attacker.CanSee( m ) || !attacker.CanBeHarmful( m ) )
						continue;

					if ( !attacker.InRange( m, weapon.MaxRange ) )
						continue;

					if ( attacker.InLOS( m ) )
					{
						attacker.RevealingAction();

						attacker.SendLocalizedMessage( 1060161 ); // The whirling attack strikes a target!
						m.SendLocalizedMessage( 1060162 ); // You are struck by the whirling attack and take damage!

						Skill skill = attacker.Skills[SkillName.Bushido];

						int damages = (int) ( ( list.Count * skill.Value ) * ( list.Count * skill.Value ) / 3600 );

						if ( damages > 100 )
						{
							damages = 100;
						}

						AOS.Damage( m, attacker, damages, 100, 0, 0, 0, 0 );

						weapon.OnHit( attacker, m );
					}
				}
			}
		}
	}
}