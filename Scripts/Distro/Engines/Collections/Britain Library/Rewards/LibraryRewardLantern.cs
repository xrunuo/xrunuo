using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardLantern : Lantern
	{
		public override int LabelNumber { get { return 1073339; } } // Friends of the Library Reading Lantern

		[Constructable]
		public LibraryRewardLantern()
		{
			Hue = 450;
		}

		public LibraryRewardLantern( Serial serial )
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