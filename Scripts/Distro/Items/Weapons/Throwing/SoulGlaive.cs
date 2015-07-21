using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class SoulGlaive : BaseThrowing
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MortalStrike; } }

		public override int StrengthReq { get { return 60; } }
		public override int MinDamage { get { return 18; } }
		public override int MaxDamage { get { return 22; } }
		public override int Speed { get { return 16; } }

		public override int MaxRange { get { return 11; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		[Constructable]
		public SoulGlaive()
			: base( 0x406B )
		{
			Weight = 8.0;
		}

		public SoulGlaive( Serial serial )
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
