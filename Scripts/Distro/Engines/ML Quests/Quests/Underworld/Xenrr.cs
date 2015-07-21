using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class ScrapingTheBottomQuest : BaseQuest
	{
		/* Scraping the Bottom */
		public override object Title { get { return 1095059; } }

		/* Catch a Mud Puppy from the underground river in the Abyss and bring
		 * it to Xenrr for your reward.
		 * -----
		 * Green Goblins have big problem.  If you help, me help you.  Important
		 * fish live at bottom of river.  Goblins call it Mud Puppy.  Very strange
		 * fish.  Have to fish very far down.
		 * 
		 * You bring Mud Puppy to Xenrr, me give you special reward.
		 */
		public override object Description { get { return 1095061; } }

		/* You think about it and come back.  Xenrr believe you are chosen one.
		 */
		public override object Refuse { get { return 1095062; } }

		/* You must fish deeper.  Mud Puppy is on bottom.
		 */
		public override object Uncomplete { get { return 1095063; } }

		/* Good job.  Mud Puppy have good mud.  Xenrr use mud to fix broken walls.
		 * Good for everyone.  Walls not fall down.  Just have to squeeze Mud Puppy
		 * and mud comes out.
		 * 
		 * Well, a deal is a deal.  Here is something good for you.
		 */
		public override object Complete { get { return 1095065; } }

		public override bool DoneOnce { get { return true; } }

		public ScrapingTheBottomQuest()
		{
			AddObjective( new ObtainObjective( typeof( MudPuppy ), "Mud Puppy", 1 ) );

			AddReward( new BaseReward( typeof( XenrrsFishingPole ), 1095066 ) ); // Xenrr's Fishing Pole
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

	public class Xenrr : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( ScrapingTheBottomQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
		}

		[Constructable]
		public Xenrr()
			: base( "Xenrr" )
		{
		}

		public Xenrr( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Body = 723;
			Hue = 0;

			CantWalk = true;
		}

		public override void InitOutfit()
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