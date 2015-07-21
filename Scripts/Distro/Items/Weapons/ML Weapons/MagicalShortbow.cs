using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D1F, 0x2D2B )]
	public class MagicalShortbow : BaseRanged
	{
		public override int EffectID { get { return 0xF42; } }
		public override Type AmmoType { get { return typeof( Arrow ); } }
		public override Item Ammo { get { return new Arrow(); } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.LightningArrow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.PsychicAttack; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 9; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 12; } }

		public override int MaxRange { get { return 10; } }

		public override int InitMinHits { get { return 40; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public MagicalShortbow()
			: base( 0x2D1F )
		{
			Weight = 10.0;
			Layer = Layer.TwoHanded;
		}

		public MagicalShortbow( Serial serial )
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
