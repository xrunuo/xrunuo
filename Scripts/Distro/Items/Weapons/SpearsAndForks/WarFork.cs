using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishWarFork ) )]
	[FlipableAttribute( 0x1405, 0x1404 )]
	public class WarFork : BaseSpear
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Pierce1H; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 12; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 10; } }

		public override int HitSound { get { return 0x236; } }
		public override int MissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 110; } }

		[Constructable]
		public WarFork()
			: base( 0x1405 )
		{
			Weight = 9.0;
		}

		public WarFork( Serial serial )
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