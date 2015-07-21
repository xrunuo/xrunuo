using System;
using Server;

namespace Server.Items
{
	public class KnightsWarCleaver : WarCleaver
	{
		public override int LabelNumber { get { return 1073525; } } // Knight's War Cleaver

		[Constructable]
		public KnightsWarCleaver()
		{
			Attributes.RegenHits = 3;
		}


		public KnightsWarCleaver( Serial serial )
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