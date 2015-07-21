using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48B9, 0x48B8 )]
	public class GargishGnarledStaff : BaseStaff
	{
		public override int LabelNumber { get { return 1097488; } } // gargish gnarled staff

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ConcussionBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ParalyzingBlow; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 20; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 13; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 50; } }

		[Constructable]
		public GargishGnarledStaff()
			: base( 0x48B9 )
		{
			Weight = 3.0;
		}

		public GargishGnarledStaff( Serial serial )
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