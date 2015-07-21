using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class TravestysFineTeakwoodTray : Item
	{
		public override int LabelNumber { get { return 1075094; } } // Travesty's Fine Teakwood Tray

		[Constructable]
		public TravestysFineTeakwoodTray()
			: base( 0x992 )
		{
			Weight = 1.0;
		}

		public TravestysFineTeakwoodTray( Serial serial )
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