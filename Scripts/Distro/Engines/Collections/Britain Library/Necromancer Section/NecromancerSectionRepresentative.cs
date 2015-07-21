using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public class NecromancerSectionRepresentative : BaseLibraryRepresentative
	{
		public NecromancerSectionRepresentative()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Necromancer";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			Utility.AssignRandomHair( this );

			if ( Utility.RandomBool() )
				Utility.AssignRandomFacialHair( this, HairHue );

			AddItem( new WizardsHat( 2305 ) );
			AddItem( new Robe( 1109 ) );
			AddItem( new Shoes( 1157 ) );
		}

		public NecromancerSectionRepresentative( Serial serial )
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