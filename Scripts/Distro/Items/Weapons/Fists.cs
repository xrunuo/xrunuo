using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class Fists : BaseMeleeWeapon
	{
		public static void Initialize()
		{
			Mobile.DefaultWeapon = new Fists();
		}

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Disarm; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ParalyzingBlow; } }

		public override int Speed { get { return 8; } }

		public override int StrengthReq { get { return 0; } }
		public override int MinDamage { get { return 1; } }
		public override int MaxDamage { get { return 4; } }

		public override int HitSound { get { return -1; } }
		public override int MissSound { get { return -1; } }

		public override SkillName Skill { get { return SkillName.Wrestling; } }
		public override WeaponType Type { get { return WeaponType.Fists; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Wrestle; } }

		public Fists()
			: base( 0 )
		{
			Visible = false;
			Movable = false;
		}

		public Fists( Serial serial )
			: base( serial )
		{
		}

		public override double GetDefendSkillValue( Mobile attacker, Mobile defender )
		{
			double wresValue = defender.Skills[SkillName.Wrestling].Value;
			double anatValue = defender.Skills[SkillName.Anatomy].Value;
			double evalValue = defender.Skills[SkillName.EvalInt].Value;
			double incrValue = ( anatValue + evalValue + 20.0 ) * 0.5;

			if ( incrValue > 120.0 )
				incrValue = 120.0;

			if ( wresValue > incrValue )
				return wresValue;
			else
				return incrValue;
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
			base.PlaySwingAnimation( attacker );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			Delete();
		}
	}
}