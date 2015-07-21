using System;
using Server;

namespace Server.Items
{
	public class FairyDragonWing : Item
	{
		public override int LabelNumber { get { return 1112899; } } // Fairy Dragon Wing

		[Constructable]
		public FairyDragonWing()
			: base( 0x1084 )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public FairyDragonWing( Serial serial )
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
