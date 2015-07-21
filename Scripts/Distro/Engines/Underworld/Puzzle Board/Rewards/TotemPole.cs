using System;
using Server;

namespace Server.Items
{
	public class TotemPole : Item
	{
		public override int LabelNumber { get { return 1032289; } } // totem

		[Constructable]
		public TotemPole()
			: base( 0x3001 )
		{
			Weight = 5.0;
		}

		public TotemPole( Serial serial )
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