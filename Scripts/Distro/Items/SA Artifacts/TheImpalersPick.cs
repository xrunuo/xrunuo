using System;
using Server;

namespace Server.Items
{
	public class TheImpalersPick : HammerPick
	{
		public override int LabelNumber { get { return 1113822; } } // The Impaler's Pick

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public TheImpalersPick()
		{
			Hue = 0x835;

			Slayer = SlayerName.Repond;
			WeaponAttributes.HitManaDrain = 10;
			WeaponAttributes.HitLightning = 40;
			WeaponAttributes.HitLowerDefend = 40;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 45;
		}

		public TheImpalersPick( Serial serial )
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