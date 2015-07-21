using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class MageSectionRepresentative : BaseLibraryRepresentative
	{
		public MageSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Mage";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			int hairhue = Utility.RandomHairHue();
			AddItem( new LongHair( hairhue ) );
			AddItem( new ShortBeard( hairhue ) );
			AddItem( new Robe( 1157 ) );
			AddItem( new Shoes( 1109 ) );
		}

		public MageSectionRepresentative( Serial serial )
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