using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishDaisho ) )]
	[FlipableAttribute( 0x27A9, 0x27F4 )]
	public class Daisho : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Feint; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.DoubleStrike; } }

		public override SkillName AbilitySkill { get { return SkillName.Bushido; } }

		public override int StrengthReq { get { return 40; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 15; } }
		public override int Speed { get { return 11; } }

		public override int HitSound { get { return 0x23B; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 45; } }
		public override int InitMaxHits { get { return 65; } }

		[Constructable]
		public Daisho()
			: base( 0x27A9 )
		{
			Weight = 8.0;
			Layer = Layer.TwoHanded;
		}

		public Daisho( Serial serial )
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
