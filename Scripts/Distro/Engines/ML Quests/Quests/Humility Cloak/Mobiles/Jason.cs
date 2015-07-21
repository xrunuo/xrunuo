using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Jason : BaseErrand
	{
		/* Thou looketh like a fellow healer in that cloak. */
		public override int BarkMessage { get { return 1075764; } }

		/* I am the sort of person who wandereth the countryside for weeks,
		 * my friend. All that time I spend healing good people who art in
		 * need of my services. All of this done without any reward, save
		 * the knowledge that I leadeth a life of virtue. */
		public override int GumpMessage { get { return 1075765; } }

		/* I have my needs, however.  A ~1_desire~ would certainly help me. */
		public override int DesireMessage { get { return 1075766; } }

		/* This ~1_gift~ was sent to me by mistake, I wouldest like to
		 * trade it for what I need. */
		public override int GiftMessage { get { return 1075767; } }

		/* A ~1_desire~! Many thanks to thee. Please accept this ~2_gift~. */
		public override int GiftGivenMessage { get { return 1075768; } }

		[Constructable]
		public Jason()
			: base( "Jason", "the Healer" )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			Hue = 0x83EA;

			HairItemID = 0x2048; // Receding
			HairHue = 0x45C;
		}

		public override void InitOutfit()
		{
			AddItem( new Robe(), 0x95 );
			AddItem( new Sandals(), 0x8FD );
		}

		public Jason( Serial serial )
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