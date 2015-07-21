using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Nelson : BaseErrand
	{
		/* I doth have a similar cloak.*/
		public override int BarkMessage { get { return 1075749; } }

		/* Greetings. I tend my flock and provide guidance to my fellow citizens.
		 * I seek not a life full of profit. For to strive for personal recognition
		 * or social position is not a worthy cause. A humble man is happy with his
		 * place. */
		public override int GumpMessage { get { return 1075750; } }

		/* All that I needest is a ~1_desire~. */
		public override int DesireMessage { get { return 1075751; } }

		/* I useth this ~1_gift~ not at all. If ye bringeth me the right item,
		 * I would make a trade. */
		public override int GiftMessage { get { return 1075752; } }

		/* Ah, ‘tis a perfect ~1_desire~. Please take this ~2_gift~ in trade. */
		public override int GiftGivenMessage { get { return 1075753; } }

		[Constructable]
		public Nelson()
			: base( "Nelson", "the Shepherd" )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x203C; // Long Hair
			FacialHairItemID = 0x204C; // Medium Long Beard
		}

		public override void InitOutfit()
		{
			AddItem( new ShepherdsCrook(), 0x386 );
			AddItem( new Sandals(), 0x8FD );
			AddItem( new Robe(), 0x364 );
		}

		public Nelson( Serial serial )
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