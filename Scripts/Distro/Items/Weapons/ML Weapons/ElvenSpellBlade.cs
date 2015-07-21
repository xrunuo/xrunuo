using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( DualPointedSpear ) )]
	[FlipableAttribute( 0x2D20, 0x2D2C )]
	public class ElvenSpellBlade : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.PsychicAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.BleedAttack; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 12; } }
		public override int MaxDamage { get { return 14; } }
		public override int Speed { get { return 10; } }

		public override int HitSound { get { return 0x23C; } }
		public override int MissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 90; } }

		public override SkillName Skill { get { return SkillName.Fencing; } }
		public override WeaponType Type { get { return WeaponType.Piercing; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public ElvenSpellBlade()
			: base( 0x2D20 )
		{
			Weight = 7.0;
			Layer = Layer.TwoHanded;
		}

		public ElvenSpellBlade( Serial serial )
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
