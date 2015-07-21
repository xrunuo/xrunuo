using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishTalwar ) )]
	[FlipableAttribute( 0x26C1, 0x26CB )]
	public class CrescentBlade : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MortalStrike; } }

		public override int StrengthReq { get { return 55; } }
		public override int MinDamage { get { return 11; } }
		public override int MaxDamage { get { return 14; } }
		public override int Speed { get { return 10; } }

		public override int HitSound { get { return 0x23B; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 51; } }
		public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public CrescentBlade()
			: base( 0x26C1 )
		{
			Weight = 1.0;
		}

		public CrescentBlade( Serial serial )
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