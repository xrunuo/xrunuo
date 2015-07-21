using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class SlimyOintmentTrammel : BaseActivation
	{
		public override int LabelNumber { get { return 1074330; } } // slimy ointment

		[Constructable]
		public SlimyOintmentTrammel()
			: base( 0x318E )
		{
			Mapa = Map.Trammel;
			Hue = 1109;
			EnterPoint = new Point3D( 6519, 382, 0 );
		}

		public SlimyOintmentTrammel( Serial serial )
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