using System;
using Server;

namespace Server.Items
{
	public class AbyssalHorn : Item
	{
		public override int LabelNumber { get { return 1031703; } } // Horn of Abyssal Infernal

		[Constructable]
		public AbyssalHorn()
			: base( 0x2DB7 )
		{
			Weight = 1.0;
			Stackable = false;
		}

		public AbyssalHorn( Serial serial )
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