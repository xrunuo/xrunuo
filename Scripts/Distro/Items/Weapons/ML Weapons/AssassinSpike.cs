using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( Shortblade ) )]
	[FlipableAttribute( 0x2D21, 0x2D2D )]
	public class AssassinSpike : BaseSpear
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.InfectiousStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ShadowStrike; } }

		public override int StrengthReq { get { return 15; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 8; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public AssassinSpike()
			: base( 0x2D21 )
		{
			Weight = 2.0;
			Layer = Layer.OneHanded;
		}

		public AssassinSpike( Serial serial )
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
