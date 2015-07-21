using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardKilt : Kilt
	{
		public override int LabelNumber { get { return 1073352; } } // Friends of the Library Kilt

		[Constructable]
		public LibraryRewardKilt()
		{
			Hue = 400;
		}

		public LibraryRewardKilt( Serial serial )
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