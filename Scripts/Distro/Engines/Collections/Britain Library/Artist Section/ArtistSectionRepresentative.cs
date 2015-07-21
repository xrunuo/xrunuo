using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class ArtistSectionRepresentative : BaseLibraryRepresentative
	{
		public ArtistSectionRepresentative()
		{
			Name = NameList.RandomName( "female" );
			Title = "the Artist";
			Body = 0x191;
			Female = true;
			Hue = Utility.RandomSkinHue();

			AddItem( new LongHair( Utility.RandomHairHue() ) );
			AddItem( new FancyShirt( 1201 ) );
			AddItem( new HalfApron( 2301 ) );
			AddItem( new Skirt( 1234 ) );
			AddItem( new Shoes( 1811 ) );
		}

		public ArtistSectionRepresentative( Serial serial )
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