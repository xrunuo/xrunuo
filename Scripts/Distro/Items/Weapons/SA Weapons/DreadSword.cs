using System;
using Server;

namespace Server.Items
{
	public class DreadSword : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ConcussionBlow; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 16; } }
		public override int Speed { get { return 14; } }

		public override int HitSound { get { return 0x23C; } }
		public override int MissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public DreadSword()
			: base( 0x090B )
		{
			Weight = 7.0;
		}

		public DreadSword( Serial serial )
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