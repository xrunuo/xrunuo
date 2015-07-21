using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48CF, 0x48CE )]
	public class GargishTekagi : BaseKnife
	{
		public override int LabelNumber { get { return 1097510; } } // gargish tekagi

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DualWield; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.TalonStrike; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override SkillName AbilitySkill { get { return SkillName.Ninjitsu; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 8; } }

		public override int HitSound { get { return 0x238; } }
		public override int MissSound { get { return 0x232; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 50; } }

		public override SkillName Skill { get { return SkillName.Fencing; } }
		public override WeaponType Type { get { return WeaponType.Piercing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public GargishTekagi()
			: base( 0x48CF )
		{
			Weight = 5.0;
		}

		public GargishTekagi( Serial serial )
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