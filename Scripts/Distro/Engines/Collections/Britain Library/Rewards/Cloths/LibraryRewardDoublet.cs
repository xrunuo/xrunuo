using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardDoublet : Doublet, ICollectionItem
	{
		public override int LabelNumber { get { return 1073351; } } // Friends of the Library Doublet

		[Constructable]
		public LibraryRewardDoublet()
		{
			Hue = 400;
		}

		public LibraryRewardDoublet( Serial serial )
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