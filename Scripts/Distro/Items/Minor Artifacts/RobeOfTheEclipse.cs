using System;
using Server;

namespace Server.Items
{
	public class RobeOfTheEclipse : Robe
	{
		public override int LabelNumber { get { return 1075082; } } // Robe of the Eclipse

		[Constructable]
		public RobeOfTheEclipse()
		{
			Hue = 1158;
			Attributes.Luck = 95;
		}

		public RobeOfTheEclipse( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}