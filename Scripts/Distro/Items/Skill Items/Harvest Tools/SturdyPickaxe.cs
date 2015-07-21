using System;
using Server;
using Server.Engines.Harvest;

namespace Server.Items
{
	public class SturdyPickaxe : BaseAxe, IUsesRemaining
	{
		public override int LabelNumber { get { return 1045126; } } // sturdy pickaxe

		public override HarvestSystem HarvestSystem { get { return Mining.System; } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override int StrengthReq { get { return 50; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 15; } }
		public override int Speed { get { return 12; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Slash1H; } }

		[Constructable]
		public SturdyPickaxe()
			: this( 180 )
		{
		}

		[Constructable]
		public SturdyPickaxe( int uses )
			: base( 0xE86 )
		{
			Weight = 11.0;
			Hue = 0x973;

			UsesRemaining = uses;
			ShowUsesRemaining = true;
		}

		public SturdyPickaxe( Serial serial )
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
