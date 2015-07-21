using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( DualPointedSpear ) )]
	[FlipableAttribute( 0x27AF, 0x27FA )]
	public class Sai : BaseKnife
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Block; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ArmorPierce; } }

		public override SkillName AbilitySkill { get { return SkillName.Ninjitsu; } }

		public override int StrengthReq { get { return 15; } }
		public override int MinDamage { get { return 9; } }
		public override int MaxDamage { get { return 11; } }
		public override int Speed { get { return 8; } }

		public override int HitSound { get { return 0x23C; } }
		public override int MissSound { get { return 0x232; } }

		public override int InitMinHits { get { return 55; } }
		public override int InitMaxHits { get { return 60; } }

		public override SkillName Skill { get { return SkillName.Fencing; } }
		public override WeaponType Type { get { return WeaponType.Piercing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public Sai()
			: base( 0x27AF )
		{
			Weight = 7.0;
			Layer = Layer.TwoHanded;
		}

		public Sai( Serial serial )
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
