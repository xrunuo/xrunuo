using System;
using Server;

namespace Server.Items
{
	public class ClawsOfTheBerserker : Tekagi
	{
		public override int LabelNumber { get { return 1113758; } } // Claws of the Berserker

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public ClawsOfTheBerserker()
		{
			Hue = 0x6F2;

			WeaponAttributes.HitLightning = 45;
			WeaponAttributes.HitLowerDefend = 50;
			WeaponAttributes.BattleLust = 1;
			Attributes.CastSpeed = 1;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 60;

		}

		public ClawsOfTheBerserker( Serial serial )
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