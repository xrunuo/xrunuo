using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishButcherKnife ) )]
	[FlipableAttribute( 0x13F6, 0x13F7 )]
	public class ButcherKnife : BaseKnife
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.InfectiousStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 9; } }
		public override int MaxDamage { get { return 11; } }
		public override int Speed { get { return 9; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 40; } }

		[Constructable]
		public ButcherKnife()
			: base( 0x13F6 )
		{
			Weight = 1.0;
		}

		public ButcherKnife( Serial serial )
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