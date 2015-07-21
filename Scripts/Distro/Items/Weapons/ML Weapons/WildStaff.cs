using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D25, 0x2D31 )]
	public class WildStaff : BaseStaff
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Block; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ForceOfNature; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Bash1H; } }

		public override int StrengthReq { get { return 15; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 9; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public WildStaff()
			: base( 0x2D25 )
		{
			Weight = 8.0;
			Layer = Layer.OneHanded;

		}

		public WildStaff( Serial serial )
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