using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48B5, 0x48B4 )]
	public class GargishBardiche : BasePoleArm
	{
		public override int LabelNumber { get { return 1097484; } } // gargish bardiche

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ParalyzingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 17; } }
		public override int MaxDamage { get { return 18; } }
		public override int Speed { get { return 15; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 100; } }

		[Constructable]
		public GargishBardiche()
			: base( 0x48B5 )
		{
			Weight = 7.0;
		}

		public GargishBardiche( Serial serial )
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