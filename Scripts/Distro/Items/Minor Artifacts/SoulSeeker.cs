using System;
using Server;

namespace Server.Items
{
	public class SoulSeeker : RadiantScimitar
	{
		public override int LabelNumber { get { return 1075046; } } // Soul Seeker

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public SoulSeeker()
		{
			Hue = 908;
			Slayer = SlayerName.Repond;
			Attributes.WeaponSpeed = 60;
			WeaponAttributes.HitLeechHits = 40;
			WeaponAttributes.HitLeechMana = 40;
			WeaponAttributes.HitLeechStam = 40;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			cold = 100;

			fire = phys = pois = nrgy = 0;
		}

		public SoulSeeker( Serial serial )
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