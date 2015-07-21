using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x27A8, 0x27F3 )]
	public class Bokuto : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Feint; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.NerveStrike; } }

		public override SkillName AbilitySkill { get { return SkillName.Bushido; } }

		public override int StrengthReq { get { return 20; } }
		public override int MinDamage { get { return 9; } }
		public override int MaxDamage { get { return 11; } }
		public override int Speed { get { return 8; } }

		public override int HitSound { get { return 0x536; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 25; } }
		public override int InitMaxHits { get { return 50; } }

		[Constructable]
		public Bokuto()
			: base( 0x27A8 )
		{
			Weight = 7.0;
		}

		public Bokuto( Serial serial )
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
	}
}
