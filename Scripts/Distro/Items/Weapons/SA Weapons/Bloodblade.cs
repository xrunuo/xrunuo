using System;
using Server;

namespace Server.Items
{
	public class Bloodblade : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ParalyzingBlow; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 8; } }

		public override int HitSound { get { return 0x23B; } }
		public override int MissSound { get { return 0x239; } }

		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override SkillName Skill { get { return SkillName.Fencing; } }
		public override WeaponType Type { get { return WeaponType.Piercing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public Bloodblade()
			: base( 0x4072 )
		{
			Weight = 4.0;
		}

		public Bloodblade( Serial serial )
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