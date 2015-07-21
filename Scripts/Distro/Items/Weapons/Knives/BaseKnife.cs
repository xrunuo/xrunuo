using System;
using Server;
using Server.Items;
using Server.Targets;

namespace Server.Items
{
	public abstract class BaseKnife : BaseMeleeWeapon
	{
		public override int HitSound { get { return 0x23B; } }
		public override int MissSound { get { return 0x238; } }

		public override SkillName Skill { get { return SkillName.Swords; } }
		public override WeaponType Type { get { return WeaponType.Slashing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Slash1H; } }

		public BaseKnife( int itemID )
			: base( itemID )
		{
		}

		public BaseKnife( Serial serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 1010018 ); // What do you want to use this item on?

			from.Target = new BladedItemTarget( this );
		}
	}
}
