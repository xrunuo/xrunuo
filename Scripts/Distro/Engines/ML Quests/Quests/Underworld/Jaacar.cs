using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class BadCompanyQuest : BaseQuest
	{
		/* Bad Company */
		public override object Title { get { return 1095022; } }

		public override QuestChain ChainID { get { return QuestChain.JaacarChain; } }
		public override bool DoneOnce { get { return true; } }
		public override Type NextQuest { get { return typeof( TangledWebQuest ); } }

		/* Travel to the Green Goblin area and kill Green Goblins until they fear
		 * you.  Return to Jaacar for your reward.
		 * 
		 * Jaacar make friends with your kind.  Not want violence... not eat each
		 * other! Jaacar eat rotworms... Rotworm stew good! Make Jaacar smart!
		 * 
		 * We can be friends, yes? Outside kind and inside kind be friends?
		 * This is good, yes?  Jaacar knows who hates your kind; Gray Goblins not
		 * hate you. We want to be friends! Jaacar want to warn you! Yes,
		 * friends give good warnings!
		 * 
		 * Green Goblins bad, very bad. Green Goblins building up piles of weapons!
		 * When green goblins get enough weapons, they make war with the outside
		 * kind... Your kind! They come in the night and stab my new friend with
		 * own sword!  They will! I swear!
		 * 
		 * Gray Goblins know this, that is why we fight them! We protect our friend!
		 * You, our friend! Will you help stop Green Goblins? If you help, Jaacar
		 * give some of smart knowledge! Help each other, yes?
		 */
		public override object Description { get { return 1095024; } }

		/* Oh poor friend. Not believe Jaacar. You will see.
		 * Maybe too late, but you will see.
		 */
		public override object Refuse { get { return 1095025; } }

		/* Friend make Green Goblins dead yet? Make them go squish? If no green
		 * squish, they kill you when you sleep! They will!
		 */
		public override object Uncomplete { get { return 1095026; } }

		/* Green Goblins not kill you with their pointy weapons? Err... That very
		 * good! Jaacar very proud to be your friend! You squish the Green Goblins
		 * good, yes? Make them pay for killing Gray Goblins! Err... Make them pay
		 * for stealing from outside kind!
		 * 
		 * Jaacar give special gift, very good recipe for rotworm stew!  You take,
		 * make you smart! Smart like Jaacar!
		 */
		public override object Complete { get { return 1095030; } }

		public BadCompanyQuest()
		{
			AddObjective( new SlayObjective( typeof( GreenGoblin ), "green goblins", 10 ) );

			AddReward( new BaseReward( 1095031 ) ); // Recipe for Rotworm Stew
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Owner.SendLocalizedMessage( 1074360, "#1095031" ); // You receive a reward: Recipe for Rotworm Stew
			Owner.AddToBackpack( new RecipeScroll( (int) Engines.Craft.CookingRecipe.BowlOfRotwormStew ) );
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

	public class TangledWebQuest : BaseQuest
	{
		/* A Tangled Web */
		public override object Title { get { return 1095032; } }

		public override QuestChain ChainID { get { return QuestChain.JaacarChain; } }
		public override bool DoneOnce { get { return true; } }

		/* Jaacar's barrel is completely full. Return to Jaacar for your reward. */
		public override int CompleteMessage { get { return 1095038; } }

		/* Kill Bloodworms and Blood Elementals to fill Jaacar's barrel. Return
		 * to Jaacar with the filled barrel for your reward.
		 * 
		 * Will friend help Jaacar with small errand for big friend? Jaacar need
		 * big barrel full of blood. Can friend do that? Best place to get blood
		 * is blood elementals and bloodworms nearby. If you do, Jaacar give to
		 * you special present! More special than favorite recipe! */
		public override object Description { get { return 1095034; } }

		/* Filling barrel not gross! Filling barrel helps friend! You think and
		 * then come back and help. Yes, friend is big help! */
		public override object Refuse { get { return 1095035; } }

		/* Jaacar need barrel filled all the way to the top! Good friend, go fill
		 * the barrel for Jaacar. */
		public override object Uncomplete { get { return 1095036; } }

		/* Oh, you filled it! That very surprising... Err... That very impressive!
		 * You good friend to Jaacar, me give to you good present! */
		public override object Complete { get { return 1095039; } }

		public TangledWebQuest()
		{
			AddObjective( new InternalObjective() );

			AddReward( new BaseReward( typeof( TreasureBag ), 1072583 ) );
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

		private class InternalObjective : SlayObjective
		{
			public InternalObjective()
				: base( new Type[] { typeof( Bloodworm ), typeof( BloodElemental ) }, "Blood Creatures", 12, "Underworld" )
			{
			}

			public override void OnKill( Mobile killed )
			{
				base.OnKill( killed );

				// Blood from the creature goes into Jaacar’s barrel.
				Quest.Owner.SendLocalizedMessage( 1095037 );
			}
		}
	}

	public class Jaacar : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( BadCompanyQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
			Say( 1095023 ); // Smasher of skulls!  Hear what I have to say!
		}

		[Constructable]
		public Jaacar()
			: base( "Jaacar" )
		{
		}

		public Jaacar( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Body = 723;
			Hue = 2301;

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