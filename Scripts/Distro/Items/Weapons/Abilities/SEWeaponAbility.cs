using System;
using Server;

namespace Server.Items
{
	public abstract class SEWeaponAbility : WeaponAbility
	{
		public virtual double SkillRequirement { get { return 50.0; } }
		public override int BaseMana { get { return 30; } }

		public override bool CheckSkills( Mobile from )
		{
			if ( !base.CheckSkills( from ) )
				return false;

			BaseWeapon weapon = from.Weapon as BaseWeapon;

			if ( weapon != null )
			{
				SkillName req = weapon.AbilitySkill;
				
				Skill bushido = from.Skills[SkillName.Bushido];
				Skill ninjitsu = from.Skills[SkillName.Ninjitsu];

				bool enoughBushido = bushido != null && bushido.Value >= SkillRequirement;
				bool enoughNinjitsu = ninjitsu != null && ninjitsu.Value >= SkillRequirement;

				bool ok = false;
				
				int message = -1;

				if ( req == SkillName.Bushido )
				{
					message = 1070768; // You need ~1_SKILL_REQUIREMENT~ Bushido skill to perform that attack!
					ok = enoughBushido;
				}
				else if ( req == SkillName.Ninjitsu )
				{
					message = 1063352; // You need ~1_SKILL_REQUIREMENT~ Ninjitsu skill to perform that attack!
					ok = enoughNinjitsu;
				}
				else
				{
					message = 1063347; // You need ~1_SKILL_REQUIREMENT~ Bushido or Ninjitsu skill to perform that attack!
					ok = enoughBushido || enoughNinjitsu;
				}

				if ( !ok )
				{
					from.SendLocalizedMessage( message, SkillRequirement.ToString() );
					return false;
				}
				else
					return true;
			}
			else
				return false;
		}
	}
}