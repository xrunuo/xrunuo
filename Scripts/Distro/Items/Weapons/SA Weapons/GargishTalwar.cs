using System;
using Server;

namespace Server.Items
{
	public class GargishTalwar : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 40; } }
		public override int MinDamage { get { return 16; } }
		public override int MaxDamage { get { return 18; } }
		public override int Speed { get { return 14; } }

		public override int HitSound { get { return 0x232; } }
		public override int MissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 40; } }
		public override int InitMaxHits { get { return 50; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Slash2H; } }

		[Constructable]
		public GargishTalwar()
			: base( 0x4075 )
		{
			Layer = Layer.TwoHanded;
			Weight = 4.0;
		}

		public GargishTalwar( Serial serial )
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