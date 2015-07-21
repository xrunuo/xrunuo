using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class DreadHornActivation : BaseActivation
	{
		public override int LabelNumber { get { return 1074337; } } // essence of the wind

		[Constructable]
		public DreadHornActivation()
			: base( 0xEFE )
		{
			Hue = 1150;
			Mapa = Map.Ilshenar;
			EnterPoint = new Point3D( 2154, 1253, -60 );
		}

		public DreadHornActivation( Serial serial )
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