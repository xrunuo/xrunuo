using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseBashing : BaseMeleeWeapon
	{
		public override int HitSound { get { return 0x233; } }
		public override int MissSound { get { return 0x239; } }

		public override SkillName Skill { get { return SkillName.Macing; } }
		public override WeaponType Type { get { return WeaponType.Bashing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Bash1H; } }

		public BaseBashing( int itemID )
			: base( itemID )
		{
		}

		public BaseBashing( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );

			defender.Stam -= Utility.Random( 3, 3 ); // 3-5 points of stamina loss
		}
	}
}
