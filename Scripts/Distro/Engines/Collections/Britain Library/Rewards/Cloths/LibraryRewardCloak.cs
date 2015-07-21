using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardCloak : Cloak, ICollectionItem
	{
		public override int LabelNumber { get { return 1073350; } } // Friends of the Library Cloak

		[Constructable]
		public LibraryRewardCloak()
		{
			Hue = 400;
		}

		public LibraryRewardCloak( Serial serial )
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