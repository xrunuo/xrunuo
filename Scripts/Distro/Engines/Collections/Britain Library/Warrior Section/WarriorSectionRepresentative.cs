using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class WarriorSectionRepresentative : BaseLibraryRepresentative
	{
		public WarriorSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Warrior";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			AddItem( new BodySash( 1638 ) );
			AddItem( new NorseHelm() );
			AddItem( new PlateChest() );
			AddItem( new PlateLegs() );
			AddItem( new PlateGloves() );
			AddItem( new PlateArms() );
			AddItem( new StuddedGorget() );
			AddItem( new DoubleAxe() );
		}

		public WarriorSectionRepresentative( Serial serial )
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