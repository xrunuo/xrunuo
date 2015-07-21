using System;
using Server;

namespace Server.Items
{
	public class DualShortAxes : BaseAxe
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.InfectiousStrike; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 14; } }
		public override int MaxDamage { get { return 16; } }
		public override int Speed { get { return 12; } }

		public override int InitMinHits { get { return 90; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public DualShortAxes()
			: base( 0x4068 )
		{
			Weight = 4.0;
		}

		public DualShortAxes( Serial serial )
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