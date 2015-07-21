using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xE87, 0xE88 )]
	public class Pitchfork : BaseSpear
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }

		public override int StrengthReq { get { return 55; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 14; } }
		public override int Speed { get { return 10; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public Pitchfork()
			: base( 0xE87 )
		{
			Weight = 11.0;
		}

		public Pitchfork( Serial serial )
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

			if ( Weight == 10.0 )
			{
				Weight = 11.0;
			}
		}
	}
}
