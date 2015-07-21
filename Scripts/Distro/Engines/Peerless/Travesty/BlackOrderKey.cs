using System;
using Server;

namespace Server.Items
{
	public class BlackOrderKey : BaseActivation
	{
		public override int LabelNumber { get { return 1074344; } } // black order key

		[Constructable]
		public BlackOrderKey()
			: base( 0x100F )
		{
			Weight = 2.0;
			Hue = 1161;
			Mapa = Map.Malas;
			EnterPoint = new Point3D( 103, 1955, 0 );
		}

		public BlackOrderKey( Serial serial )
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