using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardFeatheredHat : FeatheredHat
	{
		public override int LabelNumber { get { return 1073347; } } // Friends of the Library Feathered Hat

		[Constructable]
		public LibraryRewardFeatheredHat()
		{
			Hue = 400;
		}

		public LibraryRewardFeatheredHat( Serial serial )
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