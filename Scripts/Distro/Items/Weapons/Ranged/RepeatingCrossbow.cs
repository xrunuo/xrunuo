using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x26C3, 0x26CD )]
	public class RepeatingCrossbow : BaseRanged
	{
		public override int EffectID { get { return 0x1BFE; } }
		public override Type AmmoType { get { return typeof( Bolt ); } }
		public override Item Ammo { get { return new Bolt(); } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MovingShot; } }

		public override int StrengthReq { get { return 30; } }
		public override int MinDamage { get { return 8; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 11; } }

		public override int MaxRange { get { return 7; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public RepeatingCrossbow()
			: base( 0x26C3 )
		{
			Weight = 6.0;
		}

		public RepeatingCrossbow( Serial serial )
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