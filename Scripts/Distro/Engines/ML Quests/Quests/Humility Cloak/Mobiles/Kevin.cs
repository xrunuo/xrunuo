using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Kevin : BaseErrand
	{
		/* Art thou hiding under that cloak? */
		public override int BarkMessage { get { return 1075759; } }

		/* Greetings, friend. Didst thou know I work all day, preparing and
		 * storing all sorts of meat? My cleaver is not dainty or at all
		 * particular, but if ye bringeth something to me then I will render
		 * it useful for food. Every creature can be thought of as useful in
		 * life... or in death. Death comes to us all, my friend. I hath
		 * learned that, for certain, in this humble profession. */
		public override int GumpMessage { get { return 1075760; } }

		/* Speaking of useful, if ye findeth me a nice ~1_desire~, I wilt
		 * be grateful. */
		public override int DesireMessage { get { return 1075761; } }

		/* I received this ~1_gift~ as a gift. I hath no need for it, but wert
		 * ye to bring me something interesting, I would trade it, perhaps. */
		public override int GiftMessage { get { return 1075762; } }

		/* Thou broughtest me a ~1_desire~! That will do nicely. Here, take
		 * this ~2_gift~ as thanks. */
		public override int GiftGivenMessage { get { return 1075763; } }

		[Constructable]
		public Kevin()
			: base( "Kevin", "the Butcher" )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x2048;
			HairHue = 0x45C;
			FacialHairItemID = 0x2040;
			FacialHairHue = 0x45C;
		}

		public override void InitOutfit()
		{
			AddItem( new Shoes(), 0x75C );
			AddItem( new ShortPants(), 0x3E9 );
			AddItem( new FancyShirt(), 0x34E );
			AddItem( new HalfApron(), 0x456 );
		}

		public Kevin( Serial serial )
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