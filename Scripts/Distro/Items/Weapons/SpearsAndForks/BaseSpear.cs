using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseSpear : BaseMeleeWeapon
	{
		public override int HitSound { get { return 0x23C; } }
		public override int MissSound { get { return 0x238; } }

		public override SkillName Skill { get { return SkillName.Fencing; } }
		public override WeaponType Type { get { return WeaponType.Piercing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce2H; } }

		public BaseSpear( int itemID )
			: base( itemID )
		{
		}

		public BaseSpear( Serial serial )
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
	}
}
