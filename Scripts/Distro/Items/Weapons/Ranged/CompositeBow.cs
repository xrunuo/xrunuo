using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x26C2, 0x26CC )]
	public class CompositeBow : BaseRanged
	{
		public override int EffectID { get { return 0xF42; } }
		public override Type AmmoType { get { return typeof( Arrow ); } }
		public override Item Ammo { get { return new Arrow(); } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MovingShot; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 16; } }

		public override int MaxRange { get { return 10; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.ShootBow; } }

		[Constructable]
		public CompositeBow()
			: base( 0x26C2 )
		{
			Weight = 5.0;
		}

		public CompositeBow( Serial serial )
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
