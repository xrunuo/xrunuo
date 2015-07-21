using System;
using Server;

namespace Server.Items
{
	public class GlassStaff : BaseStaff
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MortalStrike; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 20; } }
		public override int MinDamage { get { return 11; } }
		public override int MaxDamage { get { return 14; } }
		public override int Speed { get { return 9; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public GlassStaff()
			: base( 0x0905 )
		{
			Weight = 4.0;
		}

		public GlassStaff( Serial serial )
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