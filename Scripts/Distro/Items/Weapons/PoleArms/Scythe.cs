using System;
using Server.Network;
using Server.Items;
using Server.Engines.Harvest;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishScythe ) )]
	[FlipableAttribute( 0x26BA, 0x26C4 )]
	public class Scythe : BasePoleArm
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ParalyzingBlow; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 18; } }
		public override int Speed { get { return 14; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 100; } }

		public override HarvestSystem HarvestSystem { get { return null; } }

		[Constructable]
		public Scythe()
			: base( 0x26BA )
		{
			Weight = 5.0;
		}

		public Scythe( Serial serial )
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