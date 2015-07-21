using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class MusicianSectionRepresentative : BaseLibraryRepresentative
	{
		public MusicianSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Musician";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			int hairhue = Utility.RandomHairHue();
			AddItem( new ShortHair( hairhue ) );
			AddItem( new LongBeard( hairhue ) );
			AddItem( new FancyShirt( 1443 ) );
			AddItem( new LongPants( 1637 ) );
			AddItem( new Shoes( 182 ) );
		}

		public MusicianSectionRepresentative( Serial serial )
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