using System;
using Server;

namespace Server.Items
{
	public class StoneSlithClaw : Cyclone
	{
		public override int LabelNumber { get { return 1112393; } } // Stone Slith Claw

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public StoneSlithClaw()
		{
			Hue = 0xCE; // TODO (SA): check hue

			Slayer = SlayerName.Demon;
			WeaponAttributes.HitHarm = 40;
			WeaponAttributes.HitLowerDefend = 40;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 45;
		}

		public StoneSlithClaw( Serial serial )
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