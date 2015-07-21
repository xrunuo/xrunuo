using System;
using Server;

namespace Server.Items
{
	public class SerpentstoneStaff : BaseStaff
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 13; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public SerpentstoneStaff()
			: base( 0x406F )
		{
			Weight = 3.0;
		}

		public SerpentstoneStaff( Serial serial )
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