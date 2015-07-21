using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Boomerang : BaseThrowing
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.MysticArc; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ConcussionBlow; } }

		public override int StrengthReq { get { return 25; } }
		public override int MinDamage { get { return 8; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 8; } }

		public override int MaxRange { get { return 7; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		[Constructable]
		public Boomerang()
			: base( 0x4067 )
		{
			Weight = 4.0;
		}

		public Boomerang( Serial serial )
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
