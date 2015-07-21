using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class PaladinSectionRepresentative : BaseLibraryRepresentative
	{
		public PaladinSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Paladin";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			Item item = new PlateArms();
			item.Hue = 2214;
			AddItem( item );

			item = new PlateChest();
			item.Hue = 2214;
			AddItem( item );

			item = new PlateGloves();
			item.Hue = 2214;
			AddItem( item );

			item = new PlateLegs();
			item.Hue = 2214;
			AddItem( item );

			item = new PlateGorget();
			item.Hue = 2214;
			AddItem( item );

			item = new PlateHelm();
			item.Hue = 2214;
			AddItem( item );

			AddItem( new Cloak( 1254 ) );
			AddItem( new Halberd() );
		}

		public PaladinSectionRepresentative( Serial serial )
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