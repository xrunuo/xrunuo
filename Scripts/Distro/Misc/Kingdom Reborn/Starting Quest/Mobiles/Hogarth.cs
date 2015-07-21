using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.KRStartingQuest
{
	public class Hogarth : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[]
					{ 
						typeof( ThreeFeetOfSteelQuest )
					};
			}
		}

		[Constructable]
		public Hogarth()
			: base( "Hogarth", "the Keeper of Old Haven" )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 33779;
			HairItemID = 0x203D;
			FacialHairItemID = 0x203F;
		}

		public override void InitOutfit()
		{
			AddItem( new ThighBoots( 443 ) );
			AddItem( new LeatherGloves() );
			AddItem( new StuddedArms() );
			AddItem( new PlateChest() );
			AddItem( new LeatherLegs() );
			AddItem( new VikingSword() );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from is PlayerMobile )
			{
				if ( ( (PlayerMobile) from ).KRStartingQuestStep > 0 )
					base.OnDoubleClick( from );
			}
		}

		public Hogarth( Serial serial )
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