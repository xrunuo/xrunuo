using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48AF, 0x48AE )]
	public class GargishCleaver : BaseKnife
	{
		public override int LabelNumber { get { return 1097478; } } // gargish cleaver

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.InfectiousStrike; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 11; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 10; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 50; } }

		[Constructable]
		public GargishCleaver()
			: base( 0x48AF )
		{
			Weight = 2.0;
		}

		public GargishCleaver( Serial serial )
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