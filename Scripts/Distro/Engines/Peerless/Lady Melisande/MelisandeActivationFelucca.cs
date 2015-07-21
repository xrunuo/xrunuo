using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class MelisandeActivationFelucca : BaseActivation
	{
		public override int LabelNumber { get { return 1074346; } } // dryad's curse

		[Constructable]
		public MelisandeActivationFelucca()
			: base( 0x113D )
		{
			Hue = 1109;
			Mapa = Map.Felucca;
			EnterPoint = new Point3D( 6518, 948, 37 );
		}

		public MelisandeActivationFelucca( Serial serial )
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