using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishTalwar ) )]
	[FlipableAttribute( 0x2D26, 0x2D32 )]
	public class RuneBlade : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Disarm; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Bladeweave; } }

		public override int StrengthReq { get { return 30; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 12; } }

		public override int HitSound { get { return 0x23B; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 40; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public RuneBlade()
			: base( 0x2D26 )
		{
			Weight = 7.0;
			Layer = Layer.TwoHanded;
		}

		public RuneBlade( Serial serial )
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
