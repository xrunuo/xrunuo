using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishTalwar ) )]
	[FlipableAttribute( 0x27A2, 0x27ED )]
	public class NoDachi : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.RidingSwipe; } }

		public override SkillName AbilitySkill { get { return SkillName.Bushido; } }

		public override int StrengthReq { get { return 40; } }
		public override int MinDamage { get { return 16; } }
		public override int MaxDamage { get { return 18; } }
		public override int Speed { get { return 14; } }

		public override int HitSound { get { return 0x23B; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 90; } }

		[Constructable]
		public NoDachi()
			: base( 0x27A2 )
		{
			Weight = 10.0;
			Layer = Layer.TwoHanded;
		}

		public NoDachi( Serial serial )
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
