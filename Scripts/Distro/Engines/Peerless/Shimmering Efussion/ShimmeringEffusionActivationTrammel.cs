using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class ShimmeringEffusionActivationTrammel : BaseActivation
	{
		public override int LabelNumber { get { return 1074348; } } // master key

		[Constructable]
		public ShimmeringEffusionActivationTrammel()
			: base( 0xE27 )
		{
			Hue = 0;
			Mapa = Map.Trammel;
			EnterPoint = new Point3D( 6523, 139, -20 );
		}

		public ShimmeringEffusionActivationTrammel( Serial serial )
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