using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class AnimalTrainerSectionRepresentative : BaseLibraryRepresentative
	{
		public AnimalTrainerSectionRepresentative()
		{
			Name = NameList.RandomName( "female" );
			Title = "the Animal Trainer";
			Body = 0x191;
			Female = true;
			Hue = Utility.RandomSkinHue();

			AddItem( new TwoPigTails( Utility.RandomHairHue() ) );
			AddItem( new QuarterStaff() );
			AddItem( new ThighBoots() );
			AddItem( new Shirt( 1410 ) );
			AddItem( new Skirt( 1239 ) );
		}

		public AnimalTrainerSectionRepresentative( Serial serial )
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