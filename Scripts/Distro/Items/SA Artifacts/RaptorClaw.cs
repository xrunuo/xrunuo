using System;
using Server;

namespace Server.Items
{
	public class RaptorClaw : Boomerang
	{
		public override int LabelNumber { get { return 1112394; } } // Raptor Claw

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public RaptorClaw()
		{
			Hue = 154;

			Slayer = SlayerName.Undead;
			WeaponAttributes.HitLeechStam = 40;
			Attributes.AttackChance = 12;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 35;
		}

		public RaptorClaw( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}