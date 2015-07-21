using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x27A5, 0x27F0 )]
	public class Yumi : BaseRanged
	{
		public override int EffectID { get { return 0xF42; } }
		public override Type AmmoType { get { return typeof( Arrow ); } }
		public override Item Ammo { get { return new Arrow(); } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorPierce; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.DoubleShot; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.ShootBow; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 16; } }
		public override int MaxDamage { get { return 20; } }
		public override int Speed { get { return 18; } }

		public override int MaxRange { get { return 10; } }

		public override int InitMinHits { get { return 55; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public Yumi()
			: base( 0x27A5 )
		{
			Weight = 9.0;
			Layer = Layer.TwoHanded;
		}

		public Yumi( Serial serial )
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
