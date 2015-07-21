using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class SamuraiSectionRepresentative : BaseLibraryRepresentative
	{
		public SamuraiSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Samurai";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			AddItem( new Waraji( 1432 ) );
			AddItem( new DecorativePlateKabuto() );
			AddItem( new LeatherDo() );
			AddItem( new LeatherSuneate() );
			AddItem( new LeatherHiroSode() );
			AddItem( new Wakizashi() );
		}

		public SamuraiSectionRepresentative( Serial serial )
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