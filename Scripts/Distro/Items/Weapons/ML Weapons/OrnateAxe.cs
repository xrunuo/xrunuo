using System;
using Server.Items;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( DualShortAxes ) )]
	[FlipableAttribute( 0x2D28, 0x2D34 )]
	public class OrnateAxe : BaseAxe
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 18; } }
		public override int MaxDamage { get { return 20; } }
		public override int Speed { get { return 14; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public OrnateAxe()
			: base( 0x2D28 )
		{
			Weight = 4.0;
			Layer = Layer.TwoHanded;

		}

		public OrnateAxe( Serial serial )
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
