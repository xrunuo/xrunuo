using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class KnowledgeOfTheSoulforgeQuest : BaseQuest
	{
		/* Knowledge of the Soulforge */
		public override object Title { get { return 1112536; } }

		/* Stand near a soulforge and use the Imbuing skill to unravel magical items. 
		 * Retrieve Enchanted Essence and give it to Aurvidlem. There are three magical
		 * elements that the soulforge can unravel from a magic item: Magical Residue,
		 * Enchanted Essence, and Relic Fragments. Each Imbuing recipe includes a quantity
		 * of one of these ingredients.<br><center>------</center><br>Well met! To continue
		 * your training, you must learn to unravel more powerful magic items. You must
		 * have a magic item. Stand near a soulforge and unravel the magic item into magical
		 * ingredients until you obtain Enchanted Essence.<BR><BR>Return to me with the
		 * Enchanted Essence, and I will reward you with a scroll of power. */
		public override object Description { get { return 1112526; } }

		/* Take care and be safe. Many great dangers surround us. */
		public override object Refuse { get { return 1112546; } }

		/* We meet again. Remember to stand near a soulforge and unravel magic items into
		 * ingredients. You have the aptitude to be a great Artificer. Do what I instruct,
		 * and bring me the result. */
		public override object Uncomplete { get { return 1112547; } }

		/* Well done! You are becoming a skilled Artificer. Come back if you wish to continue to learn. */
		public override object Complete { get { return 1112548; } }

		public KnowledgeOfTheSoulforgeQuest()
		{
			AddObjective( new ObtainObjective( typeof( EnchantedEssence ), "Enchanted Essence", 50, 0x2DB2 ) );

			AddReward( new BaseReward( 1112530 ) ); // Knowledge
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Backpack pack = new Backpack();
			PowerScroll ps = new PowerScroll( SkillName.Imbuing, 115.0 );
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

	public class Aurvidlem : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( KnowledgeOfTheSoulforgeQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
			Say( 1112525 ); // Come to me, Artificer. I have a task for you.
		}

		[Constructable]
		public Aurvidlem()
			: base( "Aurvidlem", "the Artificer" )
		{
			SetSkill( SkillName.ItemID, 60.0, 83.0 );
			SetSkill( SkillName.Imbuing, 60.0, 83.0 );
		}

		public Aurvidlem( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			CantWalk = true;
			Race = Race.Gargoyle;

			Hue = 0x86DE;
			HairItemID = 0x4259;
			HairHue = 0x0;
		}

		public override void InitOutfit()
		{
			AddItem( new SerpentstoneStaff() );
			AddItem( new GargishClothChest( 1307 ) );
			AddItem( new GargishClothArms( 1330 ) );
			AddItem( new GargishClothKilt( 1307 ) );
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