using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( DualPointedSpear ) )]
	[FlipableAttribute( 0x26BF, 0x26C9 )]
	public class DoubleBladedStaff : BaseSpear
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.InfectiousStrike; } }

		public override int StrengthReq { get { return 50; } }
		public override int MinDamage { get { return 12; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 9; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public DoubleBladedStaff()
			: base( 0x26BF )
		{
			Weight = 2.0;
		}

		public DoubleBladedStaff( Serial serial )
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
