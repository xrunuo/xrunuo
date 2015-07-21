using System;
using Server;

namespace Server.Items
{
	public class WindsEdge : Tessen
	{
		public override int LabelNumber { get { return 1070965; } } // Wind's Edge

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public WindsEdge()
		{
			WeaponAttributes.HitLeechMana = 40;
			Attributes.DefendChance = 10;
			Attributes.WeaponSpeed = 50;
			Attributes.WeaponDamage = 50;
		}

		public WindsEdge( Serial serial )
			: base( serial )
		{
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			nrgy = 100;

			fire = pois = phys = cold = 0;
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

			if ( WeaponAttributes.HitLeechMana == 0 )
			{
				WeaponAttributes.HitLeechMana = 40;
			}
		}
	}
}
