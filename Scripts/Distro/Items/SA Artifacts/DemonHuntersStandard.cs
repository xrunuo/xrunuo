using System;
using Server;

namespace Server.Items
{
	public class DemonHuntersStandard : Spear
	{
		public override int LabelNumber { get { return 1113864; } } // Demon Hunter's Standard

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DemonHuntersStandard()
		{
			Hue = 0x486;

			WeaponAttributes.HitLeechStam = 50;
			WeaponAttributes.HitLightning = 40;
			WeaponAttributes.HitLowerDefend = 30;
			Attributes.CastSpeed = 1;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 50;
			Slayer = SlayerName.Demon;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chao )
		{
			phys = fire = cold = pois = nrgy = 0;
			chao = 100;
		}

		public DemonHuntersStandard( Serial serial )
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