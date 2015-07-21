using System;
using Server;

namespace Server.Items
{
	public class Exiler : Tetsubo
	{
		public override int LabelNumber { get { return 1070913; } } // Exiler

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public Exiler()
		{
			Slayer = SlayerName.Demon;
			WeaponAttributes.HitDispel = 33;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 40;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			nrgy = 100;

			pois = phys = fire = cold = 0;
		}

		public Exiler( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
