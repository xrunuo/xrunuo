using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardPants : LongPants, ICollectionItem
	{
		public override int LabelNumber { get { return 1073349; } } // Friends of the Library Pants

		[Constructable]
		public LibraryRewardPants()
		{
			Hue = 400;
		}

		public LibraryRewardPants( Serial serial )
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