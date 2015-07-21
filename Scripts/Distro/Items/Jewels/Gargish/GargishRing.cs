using System;
using Server;

namespace Server.Items
{
	public class GargishRing : BaseJewel
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishRing()
			: base( 0x4212, Layer.Ring )
		{
			Weight = 0.1;
		}

		public GargishRing( Serial serial )
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
