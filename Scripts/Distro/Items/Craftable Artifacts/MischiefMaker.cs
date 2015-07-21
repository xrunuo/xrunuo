using System;
using Server;

namespace Server.Items
{
	public class MischiefMaker : MagicalShortbow
	{
		public override int LabelNumber { get { return 1072910; } } // Mischief Maker

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public MischiefMaker()
		{
			WeaponAttributes.Balanced = 1;
			Hue = 1349;
			Slayer = SlayerName.Undead;
			Attributes.WeaponSpeed = 35;
			Attributes.WeaponDamage = 45;
		}
		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = nrgy = fire = pois = 0;
			cold = 100;
		}

		public MischiefMaker( Serial serial )
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