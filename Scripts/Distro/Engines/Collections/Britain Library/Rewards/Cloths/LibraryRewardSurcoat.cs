using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardSurcoat : Surcoat
	{
		public override int LabelNumber { get { return 1073348; } } // Friends of the Library Surcoat

		[Constructable]
		public LibraryRewardSurcoat()
		{
			Hue = 400;
		}

		public LibraryRewardSurcoat( Serial serial )
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