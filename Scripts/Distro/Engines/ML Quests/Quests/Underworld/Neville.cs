using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class TheLostBrightwhistleQuest : BaseQuest
	{
		/* The Lost Brightwhistle */
		public override object Title { get { return 1095003; } }

		/* Escort Neville Brightwhistle to Elder Dugan. After Neville is safe, speak
		 * to Elder Dugan for your reward.
		 * 
		 * I was separated from my brothers when the goblins attacked. I am a member
		 * of the Society of Ariel Haven, come to colonize these halls that we had
		 * though abandoned. I must get out of here and warn Elder Dugan that these
		 * creatures live here and are very dangerous! Will you show me the way out? */
		public override object Description { get { return 1095005; } }

		/* Oh, have mercy on me!  I will have to make it on my own. */
		public override object Refuse { get { return 1095006; } }

		/* Is it much farther to the camp? */
		public override object Uncomplete { get { return 1095007; } }

		/* You have done me and my people a great service, traveler. I had assumed
		 * the worst had befallen Neville and I do not doubt it would have soon if
		 * you had not intervened.
		 * 
		 * The goblins are a creature of legend; it was thought that they had all
		 * been killed. They are not as strong as they are evil, but their blight
		 * can become a plague. Take this talisman, it was worn by those sent to
		 * end the previous goblin menace. It has magical powers and will aid you
		 * in defending yourself against the goblins. I know where I can get more
		 * of them and I will send for some for the rest of my people. */
		public override object Complete { get { return 1095008; } }

		public TheLostBrightwhistleQuest()
		{
			AddObjective( new EscortObjective( "Ariel Haven Camp" ) );

			AddReward( new BaseReward( typeof( GoblinSlayingTalisman ), 1095011 ) );
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

	public class Neville : BaseEscort
	{
		private static Type[] m_Quests = new Type[] { typeof( TheLostBrightwhistleQuest ) };

		public override Type[] Quests { get { return m_Quests; } }

		[Constructable]
		public Neville()
		{
			Name = "Neville Brightwhistle";
		}

		public Neville( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			Hue = 0x83EA;
			HairItemID = 0x203B; // Short Hair
			HairHue = 0x6C9;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Shoes( 0x70A ) );
			AddItem( new LongPants( 0x1BB ) );
			AddItem( new FancyShirt( 0x588 ) );
		}

		public override void Advertise()
		{
			Say( 1095004 ); // Please help me, where am I?
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