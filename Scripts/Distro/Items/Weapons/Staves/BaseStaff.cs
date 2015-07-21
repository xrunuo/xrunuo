using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseStaff : BaseMeleeWeapon
	{
		public override int HitSound { get { return 0x233; } }
		public override int MissSound { get { return 0x239; } }

		public override SkillName Skill { get { return SkillName.Macing; } }
		public override WeaponType Type { get { return WeaponType.Staff; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Bash2H; } }

		public BaseStaff( int itemID )
			: base( itemID )
		{
		}

		public BaseStaff( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );

			defender.Stam -= Utility.Random( 3, 3 ); // 3-5 points of stamina loss
		}
	}
}
