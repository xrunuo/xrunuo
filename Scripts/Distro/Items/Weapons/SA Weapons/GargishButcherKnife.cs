using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48B7, 0x48B6 )]
	public class GargishButcherKnife : BaseKnife
	{
		public override int LabelNumber { get { return 1097486; } } // gargish butcher knife

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.InfectiousStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 9; } }
		public override int MaxDamage { get { return 11; } }
		public override int Speed { get { return 9; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 40; } }

		[Constructable]
		public GargishButcherKnife()
			: base( 0x48B7 )
		{
			Weight = 1.0;
		}

		public GargishButcherKnife( Serial serial )
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