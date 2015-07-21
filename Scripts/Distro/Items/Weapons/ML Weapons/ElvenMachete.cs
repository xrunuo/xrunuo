using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D29, 0x2D35 )]
	public class ElvenMachete : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Bladeweave; } }

		public override int StrengthReq { get { return 20; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 15; } }
		public override int Speed { get { return 11; } }

		public override int HitSound { get { return 0x237; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 40; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public ElvenMachete()
			: base( 0x2D29 )
		{
			Weight = 6.0;
			Layer = Layer.OneHanded;
		}

		public ElvenMachete( Serial serial )
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
