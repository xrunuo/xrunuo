using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class MasteringTheSoulforgeQuest : BaseQuest
	{
		/* Mastering the Soulforge */
		public override object Title { get { return 1112537; } }

		/* Stand near a soulforge and use the Imbuing skill to unravel magical items. Retrieve
		 * Relic Fragments and give them to Ansikart. There are three magical elements that
		 * the soulforge can unravel from a magic item: Magical Residue, Enchanted Essence,
		 * and Relic Fragments. Each Imbuing recipe includes a quantity of one of these ingredients.
		 * <br><center>------</center><br>Greetings! To complete your training, you must learn to
		 * unravel the most powerful magic items. You must have a magic item. Stand near a
		 * soulforge and unravel the magic item into magical ingredients until you obtain Relic
		 * Fragments.<BR><BR>Return to me with the Relic Fragments, and I will reward you with
		 * a scroll of power. */
		public override object Description { get { return 1112529; } }

		/* May your life be filled with knowledge and wisdom. Until we meet again. */
		public override object Refuse { get { return 1112549; } }

		/* Welcome back! You have not done as I've instructed. Remember to stand near a soulforge
		 * and unravel magic items into ingredients. I believe in your ability. Do what I instruct,
		 * and bring me the result. */
		public override object Uncomplete { get { return 1112550; } }

		/* You have mastered the art of magic item unraveling. Your reward will be great.
		 * Good journey to you.  */
		public override object Complete { get { return 1112551; } }

		public MasteringTheSoulforgeQuest()
		{
			AddObjective( new ObtainObjective( typeof( RelicFragment ), "Relic Fragment", 50, 0x2DB3 ) );

			AddReward( new BaseReward( 1112530 ) ); // Knowledge
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Backpack pack = new Backpack();
			PowerScroll ps = new PowerScroll( SkillName.Imbuing, 120.0 );
			pack.Hue = Utility.RandomDyedHue();
			ps.LootType = LootType.Regular;
			pack.AddItem( ps );

			Owner.AddToBackpack( pack );
			Owner.SendLocalizedMessage( 1074360, "backpack" ); // You receive a reward: backpack
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

	public class Ansikart : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( MasteringTheSoulforgeQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
			Say( 1112528 ); // Master the art of unraveling magic.
		}

		[Constructable]
		public Ansikart()
			: base( "Ansikart", "the Artificer" )
		{
			SetSkill( SkillName.ItemID, 60.0, 83.0 );
			SetSkill( SkillName.Imbuing, 60.0, 83.0 );
		}

		public Ansikart( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			CantWalk = true;
			Race = Race.Gargoyle;

			Hue = 0x86DF;
			HairItemID = 0x425D;
			HairHue = 0x321;
		}

		public override void InitOutfit()
		{
			AddItem( new SerpentstoneStaff() );
			AddItem( new GargishClothChest( 1428 ) );
			AddItem( new GargishClothArms( 1445 ) );
			AddItem( new GargishClothKilt( 1443 ) );
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