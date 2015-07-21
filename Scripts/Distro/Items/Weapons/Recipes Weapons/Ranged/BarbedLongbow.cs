using System;
using Server;

namespace Server.Items
{
	public class BarbedLongbow : ElvenCompositeLongBow
	{
		public override int LabelNumber { get { return 1073505; } } // Barbed Longbow

		[Constructable]
		public BarbedLongbow()
		{
			Attributes.ReflectPhysical = 12;
		}


		public BarbedLongbow( Serial serial )
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