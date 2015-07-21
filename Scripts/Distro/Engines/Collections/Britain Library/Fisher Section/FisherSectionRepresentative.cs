using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class FisherSectionRepresentative : BaseLibraryRepresentative
	{
		public FisherSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Fisher";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			int hairhue = Utility.RandomHairHue();
			AddItem( new TwoPigTails( hairhue ) );
			AddItem( new ShortBeard( hairhue ) );
			AddItem( new FancyShirt( 1527 ) );
			AddItem( new ShortPants( 1605 ) );
			AddItem( new Shoes( 1864 ) );
		}

		public FisherSectionRepresentative( Serial serial )
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