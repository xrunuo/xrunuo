using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	[Alterable( typeof( DefBlacksmithy ), typeof( GargishCleaver ) )]
	[FlipableAttribute( 0xEC3, 0xEC2 )]
	public class Cleaver : BaseKnife
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.InfectiousStrike; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 11; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 10; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 50; } }

		[Constructable]
		public Cleaver()
			: base( 0xEC3 )
		{
			Weight = 2.0;
		}

		public Cleaver( Serial serial )
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