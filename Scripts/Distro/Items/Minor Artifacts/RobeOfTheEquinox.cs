using System;
using Server;

namespace Server.Items
{
	public class RobeOfTheEquinox : Robe
	{
		public override int LabelNumber { get { return 1075042; } } // Robe of the Equinox

		public override Race RequiredRace { get { return Race.Elf; } }

		[Constructable]
		public RobeOfTheEquinox()
		{
			Hue = 214;
			Attributes.Luck = 95;
		}

		public RobeOfTheEquinox( Serial serial )
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