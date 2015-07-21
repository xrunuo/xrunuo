using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Cyclone : BaseThrowing
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.MovingShot; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.InfusedThrow; } }

		public override int StrengthReq { get { return 40; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 12; } }

		public override int MaxRange { get { return 9; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		[Constructable]
		public Cyclone()
			: base( 0x406C )
		{
			Weight = 6.0;
		}

		public Cyclone( Serial serial )
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
