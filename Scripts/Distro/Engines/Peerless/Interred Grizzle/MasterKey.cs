using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class MasterKey : BaseActivation
	{
		public override int LabelNumber { get { return 1074348; } } // master key

		[Constructable]
		public MasterKey()
			: base( 0xFF3 )
		{
			Mapa = Map.Malas;
			Hue = 0x4A7;
			EnterPoint = new Point3D( 172, 1743, 50 );
		}

		public MasterKey( Serial serial )
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