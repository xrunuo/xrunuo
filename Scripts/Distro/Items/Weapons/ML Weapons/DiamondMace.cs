using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( DiscMace ) )]
	[FlipableAttribute( 0x2D24, 0x2D30 )]
	public class DiamondMace : BaseBashing
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ConcussionBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.CrushingBlow; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 14; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 12; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public DiamondMace()
			: base( 0x2D24 )
		{
			Weight = 14.0;
			Layer = Layer.OneHanded;
		}

		public DiamondMace( Serial serial )
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
