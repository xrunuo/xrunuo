using System;
using Server;

namespace Server.Items
{
	public class TwinklingScimitar : RadiantScimitar
	{
		public override int LabelNumber { get { return 1073544; } } // Twinkling Scimitar
		
		[Constructable]
		public TwinklingScimitar()
		{
			ItemID = 11559;
			Attributes.DefendChance = 6;
		}


		public TwinklingScimitar( Serial serial )
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