using System;
using Server;

namespace Server.Items
{
	public class CrushedCrystals : TransientItem, ICrystal
	{
		public override int LabelNumber { get { return 1074262; } } // crushed crystal pieces

		[Constructable]
		public CrushedCrystals()
			: base( 0x223C, TimeSpan.FromSeconds( 21600.0 ) )
		{
			LootType = LootType.Blessed;
			Weight = 1;
			Hue = 0x47E;
		}

		public CrushedCrystals( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}

