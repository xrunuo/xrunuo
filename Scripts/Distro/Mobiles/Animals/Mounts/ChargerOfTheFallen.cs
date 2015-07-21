using System;
using Server.Items;

namespace Server.Mobiles
{
	public class ChargerOfTheFallen : EtherealMount
	{
		public override int LabelNumber { get { return 1074816; } } // Charger of the Fallen Statuette

		[Constructable]
		public ChargerOfTheFallen()
			: base( 11676, 0x3E92 )
		{
			SolidHue = true;
		}

		public ChargerOfTheFallen( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
				SolidHue = true;
		}
	}
}