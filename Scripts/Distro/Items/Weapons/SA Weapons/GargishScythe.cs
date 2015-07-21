using System;
using Server.Network;
using Server.Items;
using Server.Engines.Harvest;

namespace Server.Items
{
	[FlipableAttribute( 0x48C5, 0x48C4 )]
	public class GargishScythe : BasePoleArm
	{
		public override int LabelNumber { get { return 1097500; } } // gargish scythe

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ParalyzingBlow; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 18; } }
		public override int Speed { get { return 14; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 100; } }

		public override HarvestSystem HarvestSystem { get { return null; } }

		[Constructable]
		public GargishScythe()
			: base( 0x48C5 )
		{
			Weight = 5.0;
		}

		public GargishScythe( Serial serial )
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