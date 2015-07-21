using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class TreasureHunterSectionRepresentative : BaseLibraryRepresentative
	{
		public TreasureHunterSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Treasure Hunter";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			int hairhue = Utility.RandomHairHue();
			AddItem( new PonyTail( hairhue ) );
			AddItem( new ShortBeard( hairhue ) );
			AddItem( new FancyShirt( 1848 ) );
			AddItem( new BodySash( 1533 ) );
			AddItem( new LongPants( 1204 ) );
			AddItem( new Boots() );
			AddItem( new Broadsword() );
			AddItem( new Cloak( 1241 ) );
		}

		public TreasureHunterSectionRepresentative( Serial serial )
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