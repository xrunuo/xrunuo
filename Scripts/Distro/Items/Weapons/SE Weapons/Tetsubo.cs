using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x27A6, 0x27F1 )]
	public class Tetsubo : BaseBashing
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.FrenziedWhirlwind; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.CrushingBlow; } }

		public override SkillName AbilitySkill { get { return SkillName.Bushido; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 12; } }
		public override int MaxDamage { get { return 14; } }
		public override int Speed { get { return 10; } }

		public override int HitSound { get { return 0x233; } }
		public override int MissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 65; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Bash2H; } }

		[Constructable]
		public Tetsubo()
			: base( 0x27A6 )
		{
			Weight = 8.0;
			Layer = Layer.TwoHanded;
		}

		public Tetsubo( Serial serial )
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
