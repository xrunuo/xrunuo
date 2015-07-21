using System;
using Server;

namespace Server.Items
{
	public class LongbowOfMight : ElvenCompositeLongBow
	{
		public override int LabelNumber { get { return 1073508; } } // Longbow of Might

		[Constructable]
		public LongbowOfMight()
		{
			Attributes.WeaponDamage = 5;
		}


		public LongbowOfMight( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}