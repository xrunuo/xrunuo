using System;
using Server;

namespace Server.Items
{
	public class DualPointedSpear : BaseSpear
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 40; } }
		public override int MinDamage { get { return 12; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 9; } }

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 40; } }

		[Constructable]
		public DualPointedSpear()
			: base( 0x406D )
		{
			Weight = 2.0;
		}

		public DualPointedSpear( Serial serial )
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