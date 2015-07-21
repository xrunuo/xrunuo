using System;
using Server;

namespace Server.Items
{
	public class SolesOfProvidence : Sandals
	{
		public override int LabelNumber { get { return 1113376; } } // Soles of Providence

		[Constructable]
		public SolesOfProvidence()
		{
			Hue = 1177;

			Attributes.Luck = 80;
		}

		public SolesOfProvidence( Serial serial )
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