using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class MasterOfTradesSectionRepresentative : BaseLibraryRepresentative
	{
		public MasterOfTradesSectionRepresentative()
		{
			Name = NameList.RandomName( "female" );
			Title = "the Master of Trades";
			Body = 0x191;
			Female = true;
			Hue = Utility.RandomSkinHue();

			AddItem( new TwoPigTails( Utility.RandomHairHue() ) );
			AddItem( new Doublet( 443 ) );
			AddItem( new HalfApron( 1408 ) );
			AddItem( new Kilt( 1845 ) );
			AddItem( new Boots() );
		}

		public MasterOfTradesSectionRepresentative( Serial serial )
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