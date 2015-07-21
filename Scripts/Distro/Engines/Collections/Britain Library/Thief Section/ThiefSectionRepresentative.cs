using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class ThiefSectionRepresentative : BaseLibraryRepresentative
	{
		public ThiefSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Thief";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			int hairhue = Utility.RandomHairHue();
			AddItem( new Afro( hairhue ) );
			AddItem( new ShortBeard( hairhue ) );
			AddItem( new FancyShirt( 1215 ) );
			AddItem( new LongPants( 443 ) );
			AddItem( new ThighBoots() );
			AddItem( new Kryss() );
		}

		public ThiefSectionRepresentative( Serial serial )
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