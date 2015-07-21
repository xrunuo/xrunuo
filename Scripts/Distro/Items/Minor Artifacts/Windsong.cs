using System;
using Server;

namespace Server.Items
{
	public class Windsong : MagicalShortbow
	{
		public override int LabelNumber { get { return 1075031; } } // Windsong

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public Windsong()
		{
			Hue = 172;
			WeaponAttributes.Velocity = 25;
			WeaponAttributes.SelfRepair = 3;
			Attributes.WeaponDamage = 35;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = 100;

			cold = phys = pois = nrgy = 0;
		}

		public Windsong( Serial serial )
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