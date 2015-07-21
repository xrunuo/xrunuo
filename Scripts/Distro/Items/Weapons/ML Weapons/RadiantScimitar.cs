using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D27, 0x2D33 )]
	public class RadiantScimitar : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Bladeweave; } }

		public override int StrengthReq { get { return 20; } }
		public override int MinDamage { get { return 12; } }
		public override int MaxDamage { get { return 14; } }
		public override int Speed { get { return 10; } }

		public override int HitSound { get { return 0x237; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 40; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public RadiantScimitar()
			: base( 0x2D33 )
		{
			Weight = 9.0;
			Layer = Layer.OneHanded;
		}

		public RadiantScimitar( Serial serial )
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
