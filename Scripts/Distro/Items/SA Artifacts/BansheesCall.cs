using System;
using Server;

namespace Server.Items
{
	public class BansheesCall : Cyclone
	{
		public override int LabelNumber { get { return 1113529; } } // Banshee's Call

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BansheesCall()
		{
			Hue = 1284;

			WeaponAttributes.Velocity = 35;
			WeaponAttributes.HitHarm = 40;
			WeaponAttributes.HitLeechHits = 45;
			Attributes.BonusStr = 5;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 50;
		}

		public BansheesCall( Serial serial )
			: base( serial )
		{
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = fire = pois = nrgy = 0;
			cold = 100;
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