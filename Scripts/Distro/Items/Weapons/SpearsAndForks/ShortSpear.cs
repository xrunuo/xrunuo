using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( DualPointedSpear ) )]
	[FlipableAttribute( 0x1403, 0x1402 )]
	public class ShortSpear : BaseSpear
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ShadowStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MortalStrike; } }

		public override int StrengthReq { get { return 40; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 8; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public ShortSpear()
			: base( 0x1403 )
		{
			Weight = 4.0;
		}

		public ShortSpear( Serial serial )
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