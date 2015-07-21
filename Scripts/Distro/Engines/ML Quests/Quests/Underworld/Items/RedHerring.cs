using System;
using Server;

namespace Server.Items
{
	public class RedHerring : BigFish
	{
		public override int LabelNumber { get { return 1095046; } } // Red Herring

		[Constructable]
		public RedHerring()
		{
			Hue = 0x676;
		}

		public RedHerring( Serial serial )
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