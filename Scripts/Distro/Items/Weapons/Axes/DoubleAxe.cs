using System;
using Server.Items;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( DualShortAxes ) )]
	[FlipableAttribute( 0xf4b, 0xf4c )]
	public class DoubleAxe : BaseAxe
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.WhirlwindAttack; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 13; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 110; } }

		[Constructable]
		public DoubleAxe()
			: base( 0xF4B )
		{
			Weight = 8.0;
		}

		public DoubleAxe( Serial serial )
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