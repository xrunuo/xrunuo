using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Sean : BaseErrand
	{
		/* That grey cloak is very nice. */
		public override int BarkMessage { get { return 1075769; } }

		/* Greetings to thee, friend! Art thou on some sort of quest? Ye have
		 * that look about ye, and that cloak looks somewhat familiar. Ah, no
		 * matter. A break from my blacksmithing work is always welcome! I
		 * canst only talk for a little while though, there are a few things
		 * I promised to have done for the township today. After all, a
		 * community is much like a long chain, and we can only be as stronger
		 * as our weakest link! */
		public override int GumpMessage { get { return 1075770; } }

		/* I do have a humble desire or two, though. I seem to have trouble
		 * finding a ~1_desire~. */
		public override int DesireMessage { get { return 1075771; } }

		public override int GiftMessage { get { return -1; } }

		/* Wonderul!  A ~1_desire~!  Surely thou hast gone through much trouble
		 * to bring this for me. Please take this iron chain that I made for
		 * Gareth. ‘Tis something we once talked of for some time, and he had
		 * suggested a new method of metalworking that I have only just
		 * accomplished. */
		public override int GiftGivenMessage { get { return 1075772; } }

		[Constructable]
		public Sean()
			: base( "Sean", "the Blacksmith" )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x2048;
			HairHue = 0x2E0;
			FacialHairItemID = 0x204B;
			FacialHairHue = 0x2E0;
		}

		public override void InitOutfit()
		{
			AddItem( new SmithHammer() );
			AddItem( new ThighBoots() );
			AddItem( new LongPants(), 0x386 );
			AddItem( new Shirt(), 0x2B8 );
			AddItem( new FullApron(), 0x901 );
		}

		protected override void SayThanks( int desire, int gift )
		{
			Say( GiftGivenMessage, String.Format( "#{0}", desire ) );
		}

		public Sean( Serial serial )
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