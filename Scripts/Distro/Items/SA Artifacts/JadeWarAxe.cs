using System;
using Server;

namespace Server.Items
{
	public class JadeWarAxe : WarAxe
	{
		public override int LabelNumber { get { return 1115445; } } // Jade War Axe

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public JadeWarAxe()
		{
			Hue = 0x48A;

			AbsorptionAttributes.FireEater = 10;
			WeaponAttributes.HitFireball = 30;
			WeaponAttributes.HitLowerDefend = 60;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 50;
			Slayer = SlayerName.Reptile;
		}

		public JadeWarAxe( Serial serial )
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