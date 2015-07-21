using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Maribel : BaseErrand
	{
		/* You, in the grey cloak, art thou hungry? */
		public override int BarkMessage { get { return 1075754; } }

		/* I feedeth any who come here with the means to pay.  Be they noblemen or
		 * commoners, peaceful or aggressive, artist or barbarian, tis not my place
		 * to judge. I believeth there is value in everyone, and thus serve all. */
		public override int GumpMessage { get { return 1075755; } }

		/* All that I wilt ask for, is a ~1_desire~. */
		public override int DesireMessage { get { return 1075756; } }

		/* This ~1_gift~ was given to me and I cannot use it. I wilt happily trade
		 * it for the right item. */
		public override int GiftMessage { get { return 1075757; } }

		/* Thanks to thee! This ~1_desire~ is just right. Here, this ~2_gift~ is for thee. */
		public override int GiftGivenMessage { get { return 1075758; } }

		[Constructable]
		public Maribel()
			: base( "Maribel", "the Waitress" )
		{
		}

		public override void InitBody()
		{
			Female = true;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x2046; // Buns
			HairHue = 0x455;
		}

		public override void InitOutfit()
		{
			AddItem( new Shoes() );
			AddItem( new HalfApron(), 0x47E );
			AddItem( new PlainDress(), 0x212 );
		}

		public Maribel( Serial serial )
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