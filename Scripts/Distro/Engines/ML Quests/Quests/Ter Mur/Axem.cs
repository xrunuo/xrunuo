using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Loyalty;

namespace Server.Engines.Quests
{
	public class BrokenVaseQuest : BaseQuest
	{
		/* A Broken Vase */
		public override object Title { get { return 1112795; } }

		/* *Axem appears to be studying an old scroll*<br><br>...if I take the third letter
		 * in every tenth word, then add them together backwards, I should... Oh, hello there,
		 * I didn't see you come in. You look like an adventurous type, perhaps you have come
		 * across things in your travels that might look like bits of junk. Things like broken
		 * pottery laying around. I'm sure you have, but did not know what you were looking
		 * for.<br><br>I'll tell you what, if you bring me some ancient pottery fragments,
		 * I'll see what I can dig up for your trouble. What do you say? */
		public override object Description { get { return 1112917; } }

		/* Well, ok then. It is really your loss, as no merchant is going to give you a single
		 * gold piece for those fragments. */
		public override object Refuse { get { return 1112918; } }

		/* I really need at least ten fragments to be able to glean anything useful from them.
		 * Please come back when you have ten. */
		public override object Uncomplete { get { return 1112919; } }

		/* Aaah! I knew that you were one of those adventurous types. Quickly now, hand those
		 * over, I do not have all day to stand here chatting if I am going to start studying
		 * those fragments. */
		public override object Complete { get { return 1112920; } }

		public BrokenVaseQuest()
		{
			AddObjective( new ObtainObjective( typeof( AncientPotteryFragments ), "Ancient Pottery Fragments", 10 ) );

			AddReward( new BaseReward( typeof( MeagerMuseumBag ), 1112993 ) ); // Meager Museum Bag
			AddReward( new LoyaltyReward( LoyaltyGroup.GargoyleQueen, 5 ) );
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

	public class PuttingThePiecesTogetherQuest : BaseQuest
	{
		/* Putting the Pieces Together */
		public override object Title { get { return 1112796; } }

		/* *As you approach, Axem seems completely unaware of you*<br><br>Ok, now if I
		 * substitute each symbol by the one thirteen places in front of it, I get...
		 * what? Who are... oh, my, forgive me. Greetings! It is good to see you again.
		 * I must apologize, I have been so busy trying to figure out the meaning of
		 * the symbols on the pottery fragments you brought me. So far, I have not
		 * gotten very far, but perhaps if I were able to cross reference these symbols
		 * with another source.<br><br>Let me think... yes, I think that would do. Have
		 * you seen any remnants of old scrolls laying about during your travels? I would
		 * reward you, of course, if you would bring them to me. Is it a deal? */
		public override object Description { get { return 1112921; } }

		/* I hope you change your mind! I have a feeling there is something important
		 * contained in these fragments that we have forgotten over time. Something very
		 * mportant, indeed. */
		public override object Refuse { get { return 1112922; } }

		/* I will need at least five remnants of an ancient scroll to be able to piece
		 * together anything useful from it. */
		public override object Uncomplete { get { return 1112923; } }

		/* *Once again, Axem is so completely engrossed in his work that he fails to
		 * notice you*<br><br>...sliding the penultimate symbol back to this position,
		 * subtracting the number of symbols in a row, leads to... hmm, absolutely
		 * nothing. Again.<br><br>*Axem jumps as he realizes you're standing next to
		 * him*<br><br>I thought you were my ex-wife there for a second. Whew! So, you
		 * have returned? With all that I have asked for? Fabulous, I'll get to work on
		 * these now. Here is your reward, now please leave me be. */
		public override object Complete { get { return 1112924; } }

		public PuttingThePiecesTogetherQuest()
		{
			AddObjective( new ObtainObjective( typeof( TatteredAncientScroll ), "Tattered Remnants of an Ancient Scroll", 5 ) );

			AddReward( new BaseReward( typeof( DustyMuseumBag ), 1112994 ) ); // Dusty Museum Bag
			AddReward( new LoyaltyReward( LoyaltyGroup.GargoyleQueen, 15 ) );
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

	public class YeOldeGargishQuest : BaseQuest
	{
		/* Ye Olde Gargish */
		public override object Title { get { return 1112797; } }

		/* *As you approach Axem, you realize that he is looking right at you for a change*
		 * <br><br>You thought you would sneak up on me again, did you? I saw you coming,
		 * so you will have to find amusement elsewhere today.<br><br>Alas, I could use
		 * your help though. I believe that I have the meanings for some of the symbols
		 * found on these ancient fragments, there is some relation to our modern alphabet,
		 * but there is just too much that has changed to say with absolute certainty.<br>
		 * <br>While you were off gallivanting about, have you noticed any old tomes in out
		 * of the way places? We have a few here already, but they've already been translated
		 * and are not from the same era as the rest of what you have brought me. These bits
		 * and pieces will only allow me to get so far, I'm afraid. If you know where to find
		 * anything like that, will you bring it to me? */
		public override object Description { get { return 1112925; } }

		/* I am severely disappointed in you. I tell you that I am very close to solving the
		 * puzzle, and you say you are too busy to help? Pfah! */
		public override object Refuse { get { return 1112926; } }

		/* Are you going to bring me an untranslated ancient tome, or not? Now, where did I
		 * put my magnifying glass... */
		public override object Uncomplete { get { return 1112927; } }

		/* *Axem has his back towards you, so you walk quietly up to him and tap him on the
		 * shoulder.*<br><br>AAAAAAHHHHHH! Get away! You can't have... oh... my... it is you.
		 * I have told you that is not very nice, and yet you persist in attempting to frighten
		 * me. Well, it will not work, so go away.<br><br>Wait, what is that you have there?
		 * Is that... yes, I think it is! Give it here, now! I mean... will you please give that
		 * to me? I have something to give you in exchange that I think you will find was worth
		 * your efforts. */
		public override object Complete { get { return 1112928; } }

		public YeOldeGargishQuest()
		{
			AddObjective( new ObtainObjective( typeof( UntranslatedAncientTome ), "Untranslated Ancient Tome", 1 ) );

			AddReward( new BaseReward( typeof( BulgingMuseumBag ), 1112995 ) ); // Bulging Museum Bag
			AddReward( new LoyaltyReward( LoyaltyGroup.GargoyleQueen, 50 ) );
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

	public class Axem : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( BrokenVaseQuest ),
				typeof( PuttingThePiecesTogetherQuest ),
				typeof( YeOldeGargishQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		[Constructable]
		public Axem()
			: base( "Axem", "the Curator" )
		{
		}

		public Axem( Serial serial )
			: base( serial )
		{
		}

		public override void Advertise()
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Gargoyle;

			Hue = 0x86E8;
			HairItemID = 0x4261;
			HairHue = 0x31E;
		}

		public override void InitOutfit()
		{
			AddItem( new GargishClothKilt( 0x57A ) );
			AddItem( new GargishClothChest( 0x519 ) );
			AddItem( new GargishClothArms( 0x5ED ) );
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
