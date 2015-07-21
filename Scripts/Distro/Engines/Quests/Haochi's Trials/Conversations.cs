using System;
using Server;

namespace Server.Engines.Quests.SE
{
	public class DaimyoHaochiBeginConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* Greetings. I am Daimyo Haochi, the Feudal Lord of this
				 * region. <BR><BR>
				 * 
				 * Since you are here at my side, you must wish to learn
				 * the ways of the Samurai.<BR><BR>
				 * 
				 *  Wielding a blade is easy, anyone can grasp a sword's
				 * hilt. Learning how to fight properly and skillfully is to
				 * become an Armsman.<BR><BR>
				 * 
				 *  Learning how to master weapons, and even more
				 * importantly when not to use them, is the Way of the
				 * Warrior. The Way of the Samurai. The Code of the
				 * Bushido. That is why you are here. <BR><BR>
				 *
				 * You will go through 7 trials to prove your adherence to
				 * the Samurai code. <BR><BR>
				 *
				 *  The first trial will test your decision making skills. You
				 * only have to enter the area beyond the green
				 * passageway. <BR><BR>
				 *
				 * Do not attempt to hurry your trials. The guards will only
				 * let you through to each trial when I have deemed you
				 * ready.<br><br>
				 *
				 * As a last resort you may use the golden teleporter tiles
				 * in each trial area but do so at your own risk. You may
				 * not be able to return and complete your trials once you
				 * have chosen to escape.
				 */
				return 1063029;
			}
		}

		public override bool Logged { get { return false; } }

		public DaimyoHaochiBeginConversation()
		{
		}
	}

	public class GreenPathConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* Beyond you are two troubled groups.<BR><BR>
				 *
				 *  The Cursed Souls were once proud warriors that were
				 * ensorcelled by an evil mage. The mage trapped and killed
				 * them later but the spell has not lifted from their souls
				 * in death. <BR><BR>
				 *
				 * The Young Ronin were former Samurai in training who
				 * lost their way. They are loyal only to those with enough
				 * coin in their pocket. <BR><BR>
				 *
				 * You must decide who needs to be fought to the death.
				 * You may wish to <a href="?ForceTopic27">review combat techniques</a> as well as
				 * <a href = "?ForceTopic29">information on healing yourself</a>. <BR><BR>
				 *
				 * Return to Daimyo Haochi after you have finished with
				 * your trial.<br><br>
				 *
				 * If you should die during any of your trials, visit one of
				 * the Ankh Shrines and you will be resurrected. You should
				 * retrieve your belongings from your body before returning
				 * to the Daimyo or you may not be able to return to
				 * your corpse.
				 */
				return 1063031;
			}
		}

		public override bool Logged { get { return false; } }

		public GreenPathConversation()
		{
		}
	}

	public class GainKarmaForRoninConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* You have just gained some <a href="?ForceTopic45">Karma</a> for killing a Young
				 * Ronin.
				 */
				return 1063041;
			}
		}

		public override bool Logged { get { return false; } }

		public GainKarmaForRoninConversation()
		{
		}
	}

	public class GainKarmaForSoulsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* You have just gained some <a href="?ForceTopic45">Karma</a> for killing a Cursed
				 * Soul.
				 */
				return 1063040;
			}
		}

		public override bool Logged { get { return false; } }

		public GainKarmaForSoulsConversation()
		{
		}
	}

	public class ContinueSlayingSoulsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				// Continue slaying the Cursed Souls!
				return 1063042;
			}
		}

		public override bool Logged { get { return false; } }

		public ContinueSlayingSoulsConversation()
		{
		}
	}

	public class ContinueSlayingRoninsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				// Continue slaying the Young Ronin!
				return 1063043;
			}
		}

		public override bool Logged { get { return false; } }

		public ContinueSlayingRoninsConversation()
		{
		}
	}

	public class ThanksForSoulsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* It is good that you rid the land of the Cursed Souls so
				 * they can be at peace in death. They had been cursed for
				 * doing what they thought was an honorable deed. Now they
				 * can have respect in their death.<BR><BR>
				 *
				 *  I have placed a reward in your pack. <BR><BR>
				 * 
				 * The second trial will test your courage. You only have to
				 * follow the yellow path to see what awaits you.
				 */
				return 1063045;
			}
		}

		public override bool Logged { get { return false; } }

		public ThanksForSoulsConversation()
		{
		}
	}

	public class ThanksForRoninsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* It is good that you rid the land of those dishonorable
				 * Samurai. Perhaps they will learn a greater lesson in
				 * death.<BR><BR>
				 * 
				 *  I have placed a reward in your pack.<BR><BR>
				 * 
				 *  The second trial will test your courage. You only have
				 * to follow the yellow path to see what awaits you.
				 */
				return 1063046;
			}
		}

		public override bool Logged { get { return false; } }

		public ThanksForRoninsConversation()
		{
		}
	}


	public class YellowPathConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* Beyond the guards is a test of courage.  You must face
				 * your fear and attack a great beast. You must choose
				 * which beast to slay for there is more than one beyond
				 * the courtyard doors. <BR><BR>
				 *
				 * The imp entered the courtyard unaware of its
				 * surroundings. The dragon came knowingly, hunting for the
				 * flesh of humans - A feast for the beast. <BR><BR>
				 * 
				 * You must rid the courtyard of these beasts but you
				 * may only choose one to attack. Go and choose wisely.
				 */
				return 1063057;
			}
		}

		public override bool Logged { get { return false; } }

		public YellowPathConversation()
		{
		}
	}

	public class DragonConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* You faced the dragon knowing it would be your certain
				 * death. That is the courage of a Samurai. <BR><BR>
				 * 
				 * Your spirit speaks as a Samurai already. <BR><BR>
				 *
				 * In these lands, death is not forever. The shrines can
				 * make you whole again as can a helpful mage or healer. <BR><BR>
				 * 
				 * Seek them out when you have been mortally wounded. <BR><BR>
				 * 
				 * The next trial will test your benevolence. You only have
				 * to walk the blue path.
				 */
				return 1063060;
			}
		}

		public override bool Logged { get { return false; } }

		public DragonConversation()
		{
		}
	}

	public class ImpConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* Fear remains in your eyes but you have learned that not
				 * all is what it appears to be. <BR><BR>
				 * 
				 * You must have known the dragon would slay you instantly.
				 * You elected the weaker opponent though the imp did not
				 * come here to destroy. You have much to learn. <BR><BR>
				 *
				 * In these lands, death is not forever. The shrines can
				 * make you whole again as can a helpful mage or healer. <BR><BR>
				 *
				 * Seek them out when you have been mortally wounded. <BR><BR>
				 *
				 * The next trial will test your benevolence. You only have
				 * to walk the blue path.
				 */
				return 1063059;
			}
		}

		public override bool Logged { get { return false; } }

		public ImpConversation()
		{
		}
	}

	public class WolfsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* A pack of wolves circle your feet. They have been
				 * injured and are in pain. A quick death will end their
				 * suffering.<br><br>
				 * 
				 * Use your Honorable Execution skill or other means to
				 * finish off a wounded wolf. Do so and return to Daimyo
				 * Haochi.
				 */
				return 1063062;
			}
		}

		public override bool Logged { get { return false; } }

		public WolfsConversation()
		{
		}
	}

	public class HaochiSmilesConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* <I>Daimyo Haochi smiles as you walk up to him.
				 * Quietly he says:</I><BR><BR>
				 * 
				 *  A Samurai understands the need to help others even as
				 * he wields a blade against them. <BR><BR>
				 * 
				 * You have shown compassion. A true Samurai is benevolent
				 * even to an enemy. For this you have been rewarded. <BR><BR>
				 *
				 * And now you must prove yourself again. Walk the red
				 * path.  We will talk again later.
				 */
				return 1063065;
			}
		}

		public override bool Logged { get { return false; } }

		public HaochiSmilesConversation()
		{
		}
	}

	public class ApproachGypsyConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* <I>You approach a disheveled gypsy standing near a
				 * small shed. You sense that she has not eaten nor
				 * bathed in quite some time. <BR><BR>
				 * 
				 * Around her is a large colony of mangy and 
				 * diseased cats. It appears she has spent what little 
				 * money she's earned to feed the cats instead of
				 * herself. <BR><BR>
				 * You have a decision to make. You can give her
				 * gold so she can buy some food for her animals
				 * and herself. You can also remove the necessity of
				 * the extra mouths to feed so she may concentrate
				 * on saving herself.</i><br><br>
				 *
				 * If you elect to give the gypsy money, you can do so by
				 * clicking your stack of gold and selecting '1'. Then
				 * dragging it and dropping it on the Gypsy.
				 */
				return 1063067;
			}
		}

		public override bool Logged { get { return false; } }

		public ApproachGypsyConversation()
		{
		}
	}

	public class RespectForGoldConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* You showed respect by helping another out while allowing
				 * the gypsy what little dignity she has left. <BR><BR>
				 *
				 * Now she will be able to feed herself and gain enough
				 * energy to walk to her camp. <BR><BR>
				 * 
				 * The cats are her family members- cursed by an evil
				 * mage. <BR><BR>
				 *
				 * Once she has enough strength to walk back to the camp,
				 * she will be able to undo the spell. <BR><BR>
				 *
				 * You have been rewarded for completing your trial. And
				 * now you must prove yourself again. <BR><BR>
				 *
				 * Please retrieve my katana from the treasure room and
				 * return it to me.
				 */
				return 1063070;
			}
		}

		public override bool Logged { get { return false; } }

		public RespectForGoldConversation()
		{
		}
	}

	public class RespectForCatsConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* Respect comes from allowing another to make their own
				 * decisions. By denying the gypsy her animals, you negate
				 * the respect she is due. Perhaps you will have learned
				 * something to use next time a similar situation arises. <BR><BR>
				 * 
				 *  And now you must prove yourself again. Please retrieve
				 * my katana from the treasure room and return it to me.
				 */
				return 1063071;
			}
		}

		public override bool Logged { get { return false; } }

		public RespectForCatsConversation()
		{
		}
	}

	public class SpotSwordConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* <i>The guards let you through without question, and
				 * pay you no mind as you walk into the Daimyo's
				 * treasure cache.  A vast fortune in gold,
				 * gemstones, and jewelry is stored here!  Surely,
				 * the Daimyo wouldn't miss a single small item...<br><br>
				 * 
				 * You spot the sword quickly amongst the cache of
				 * gemstones and other valuables.  In one quick
				 * motion you retrieve it and stash it in your pack.</i>
				 */
				return 1063248;
			}
		}

		public override bool Logged { get { return false; } }

		public SpotSwordConversation()
		{
		}
	}

	public class WithoutSwordConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* What? You have returned without the sword? Go back
				 * and look for it again.
				 */
				return 1063074;
			}
		}

		public override bool Logged { get { return false; } }

		public WithoutSwordConversation()
		{
		}
	}

	public class ThanksForSwordConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* Thank you for returning this sword to me and leaving
				 * the remaining treasure alone. <BR><BR>
				 * 
				 * Your training is nearly complete. Before you have your
				 * final trial, you should pay homage to Samurai who came
				 * before you.  <BR><BR>
				 * 
				 * Go into the Altar Room and light a candle for them.
				 * Afterwards, return to me.
				 */
				return 1063076;
			}
		}

		public ThanksForSwordConversation()
		{
		}
	}

	public class WellDoneConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* You have done well young Samurai. There is but one thing
				 * left to do. <BR><BR>
				 * 
				 * In the final room is the holding cell containing young
				 * Ninjas who came to take my life. They were caught and
				 * placed in my custody. <BR><BR>
				 * 
				 * Take care of these miscreants and show them where
				 * your loyalty lies. <BR><BR>
				 *
				 * This is your final act as a Samurai in training.
				 */
				return 1063079;
			}
		}

		public WellDoneConversation()
		{
		}
	}

	public class FirewellConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				/* You have proven yourself young one. You will continue to
				 * improve as your skills are honed with age. <BR><BR>
				 * 
				 * Now it is time for you to explore the lands. Beyond this
				 * path lies Zento City, your future home.  On these
				 * grounds you will find a golden oval object known as a
				 * Moongate, step through it and you'll find yourself in
				 * Zento.<BR><BR>
				 * 
				 * You may want to visit Ansella Gryen when you arrive. <BR><BR>
				 *
				 * You have learned the ways. You are an honorable warrior,
				 * a Samurai in the highest regards. <BR><BR>
				 *
				 * Please accept the gifts I have placed in your pack. You
				 * have earned them. Farewell for now.
				 */
				return 1063125;
			}
		}

		public FirewellConversation()
		{
		}
	}

}