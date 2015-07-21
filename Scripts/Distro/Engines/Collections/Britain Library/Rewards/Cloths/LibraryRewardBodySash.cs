using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardBodySash : BodySash
	{
		public override int LabelNumber { get { return 1073346; } } // Friends of the Library Sash

		[Constructable]
		public LibraryRewardBodySash()
		{
			Hue = 400;
		}

		public LibraryRewardBodySash( Serial serial )
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