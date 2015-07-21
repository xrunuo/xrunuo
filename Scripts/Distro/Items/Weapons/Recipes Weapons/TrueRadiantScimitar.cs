using System;
using Server;

namespace Server.Items
{
	public class TrueRadiantScimitar : RadiantScimitar
	{
		public override int LabelNumber { get { return 1073541; } } // True Radiant Scimitar
		[Constructable]
		public TrueRadiantScimitar()
		{
			Attributes.NightSight = 1;
		}


		public TrueRadiantScimitar( Serial serial )
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