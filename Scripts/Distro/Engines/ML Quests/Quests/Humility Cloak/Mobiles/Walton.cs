using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Walton : BaseErrand
	{
		/* A horse blanket would offer more comfort than thine cloak, mayhaps. */
		public override int BarkMessage { get { return 1075739; } }

		/* Hello friend. Yes, I work with yon horses each day. I carry bails of hay,
		 * feed, and even shovel manure for those beautiful beasts of burden.
		 * Everyone who rideth my animals enjoy their temperament and health.
		 * Yet, I do not ask for any recognition. */
		public override int GumpMessage { get { return 1075740; } }

		/* All I need is but a simple ~1_desire~. */
		public override int DesireMessage { get { return 1075741; } }

		/* If I ever receive that which I need, I wilt gladly trade it
		 * for this ~1_gift~. */
		public override int GiftMessage { get { return 1075742; } }

		/* Ah, thank you! This ~1_desire~ is just what I needed. Please taketh
		 * this ~2_gift~ - I hope it will be of use to thee. */
		public override int GiftGivenMessage { get { return 1075743; } }

		[Constructable]
		public Walton()
			: base( "Walton", "the Horse Trainer" )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x203B; // Short
			HairHue = 0x7DC;
			FacialHairItemID = 0x2040; // Goatee
			FacialHairHue = 0x7DC;
		}

		public override void InitOutfit()
		{
			AddItem( new Boots() );
			AddItem( new LongPants(), 0x1F4 );
			AddItem( new FancyShirt() );
			AddItem( new GoldBracelet(), 0x455 );
			AddItem( new Doublet(), 0x386 );
		}

		public Walton( Serial serial )
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