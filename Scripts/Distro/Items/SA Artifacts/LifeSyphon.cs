using System;
using Server;

namespace Server.Items
{
	public class LifeSyphon : Bloodblade
	{
		public override int LabelNumber { get { return 1113524; } } // Life Syphon

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public LifeSyphon()
		{
			Hue = 332;

			WeaponAttributes.BloodDrinker = 1;
			WeaponAttributes.HitHarm = 30;
			WeaponAttributes.HitLeechHits = 100;
			Attributes.BonusHits = 10;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 50;
		}

		public LifeSyphon( Serial serial )
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