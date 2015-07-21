using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishTessen ) )]
	[FlipableAttribute( 0x27A3, 0x27EE )]
	public class Tessen : BaseBashing
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Feint; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Block; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 8; } }

		public override int HitSound { get { return 0x232; } }
		public override int MissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 55; } }
		public override int InitMaxHits { get { return 60; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Bash2H; } }

		[Constructable]
		public Tessen()
			: base( 0x27A3 )
		{
			Weight = 6.0;
			Layer = Layer.TwoHanded;
		}

		public Tessen( Serial serial )
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
