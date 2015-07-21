using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Deirdre : BaseErrand
	{
		/* The cloak thou wearest looks warm. */
		public override int BarkMessage { get { return 1075744; } }

		/* Good tidings to thee. I live on scraps in the shadow of Lord British's
		 * Castle. I am so close to nothing, that surely, thou canst not help but
		 * see I live a humble life. */
		public override int GumpMessage { get { return 1075745; } }

		/* One ~1_desire~ wilt make my life so much nicer. */
		public override int DesireMessage { get { return 1075746; } }

		/* I have no need for this ~1_gift~. For the right item, I would trade it. */
		public override int GiftMessage { get { return 1075747; } }

		/* *gasp* A ~1_desire~, 'tis perfect. I doth have plans for this. Here,
		 * I have no need for this ~2_gift~ now. */
		public override int GiftGivenMessage { get { return 1075748; } }

		[Constructable]
		public Deirdre()
			: base( "Deirdre", "the Beggar" )
		{
		}

		public override void InitBody()
		{
			Female = true;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x203D; // Ponytail
			HairHue = 0x386;
		}

		public override void InitOutfit()
		{
			AddItem( new FancyShirt(), 0x34E );
			AddItem( new Skirt(), 0x34E );
		}

		public Deirdre( Serial serial )
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