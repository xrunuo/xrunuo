using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishTekagi ) )]
	[FlipableAttribute( 0x27Ab, 0x27F6 )]
	public class Tekagi : BaseKnife
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DualWield; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.TalonStrike; } }

		public override SkillName AbilitySkill { get { return SkillName.Ninjitsu; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 8; } }

		public override int HitSound { get { return 0x238; } }
		public override int MissSound { get { return 0x232; } }

		public override int InitMinHits { get { return 35; } }
		public override int InitMaxHits { get { return 60; } }

		public override SkillName Skill { get { return SkillName.Fencing; } }
		public override WeaponType Type { get { return WeaponType.Piercing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public Tekagi()
			: base( 0x27AB )
		{
			Weight = 5.0;
			Layer = Layer.TwoHanded;
		}

		public Tekagi( Serial serial )
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