using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class FriendOfTheLibraryToken : BaseTalisman
	{
		public override int LabelNumber { get { return 1073136; } } // Friend of the Library Token (allows donations to be made)

		[Constructable]
		public FriendOfTheLibraryToken()
			: base( 0x2F58 )
		{
			Weight = 1.0;
			Hue = 650;
		}

		public FriendOfTheLibraryToken( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
