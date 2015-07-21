using System;
using Server;

namespace Server.Items
{
	public class KaburJournal : RedBook
	{
		[Constructable]
		public KaburJournal()
			: base( "Journal", "Kabur", 20, false )
		{
			Pages[0].Lines = new string[] { "The campaign to slaughter", "the Meer goes well.", "Although they seem to", "oppose the forces of", "ours at every turn, we", "still defeat them in", "combat.  Spies of the", "Meer have been found and" };

			Pages[1].Lines = new string[] { "slain outside of the", "fortress of ours.  The", "fools underestimate us.", "We have the power of", "Lord Exodus behind us.", "Soon they will learn to", "serve the Juka and I", "shall carry the head of" };

			Pages[2].Lines = new string[] { "the wench, Dasha, on a", "spike for all the warriors", "of ours to share triumph", "under.", "", "One of the warriors of", "the Juka died today.", "During the training" };

			Pages[3].Lines = new string[] { "exercises of ours he", "spoke out in favor of", "the warriors of the", "Meer, saying that they", "were indeed powerful and", "would provide a challenge", "to the Juka.  A Juka in", "fear is no Juka.  I gave" };

			Pages[4].Lines = new string[] { "him the death of a", "coward, outside of battle.", "", "More spies of the Meer", "have been found around", "the fortress of ours.", "Many have been seen and", "escaped the wrath of the" };

			Pages[5].Lines = new string[] { "warriors of ours.  Those", "who have been captured", "and tortured have", "revealed nothing to us,", "even when subjected to", "the spells of the females.", " I know the Meer must", "have plans against us if" };

			Pages[6].Lines = new string[] { "they send so many spies.", " I may send the troops", "of the Juka to invade", "the camps of theirs as a", "warning.", "", "I have met Dasha in", "battle this day.  The" };

			Pages[7].Lines = new string[] { "efforts of hers to draw", "me into a Black Duel", "were foolish.   Had we", "not been interrupted in", "the cave I would have", "ended the life of hers", "but I will have to wait", "for another battle.  Lord" };

			Pages[8].Lines = new string[] { "Exodus has ordered more", "patrols around the", "fortress of ours.  If", "Dasha is any indication,", "the Meer will strike soon.", "", "More Meer stand outside", "of the fortress of ours" };

			Pages[9].Lines = new string[] { "than I have ever seen at", "once.  They must seek", "vengeance for the", "destruction of their", "forest.  Many Juka stand", "ready at the base of the", "mountain to face the", "forces of theirs but" };

			Pages[10].Lines = new string[] { "today may be the final", "battle.  Exodus has", "summoned me, I must", "prepare.", "", "Dusk has passed and the", "Juka now live in a new", "age, a later time.  I have" };

			Pages[11].Lines = new string[] { "just returned from", "exploring the new world", "that surrounds the", "fortress of the Juka.", "During the attack of the", "Meer the madman", "Adranath tried to destroy", "the fortress of ours" };

			Pages[12].Lines = new string[] { "with great magic.  At", "once he was still and", "light surrounded the", "fortress.  Everything", "faded from view.  When I", "regained the senses of", "mine I saw no sign of", "the Meer but Dasha." };

			Pages[13].Lines = new string[] { "She has not been found", "since this new being,", "Blackthorn, blasted her", "from the top of the", "fortress.", "The forest was gone, now", "replaced by grasslands.", "In the far distance I" };

			Pages[14].Lines = new string[] { "could see humans that", "had covered the bodies of", "theirs in marks.  Even", "Gargoyles populate this", "place.  Exodus has", "explained to me that the", "Juka and the fortress of", "ours have been pulled" };

			Pages[15].Lines = new string[] { "forward in time.  The", "world we knew is now", "thousands of years in the", "past.  Lord Exodus say", "he has has saved the", "Juka from extinction.  I", "do not want to believe", "him.  I asked this" };

			Pages[16].Lines = new string[] { "stranger about the Meer,", "but he tells me a new", "enemy remains to be", "destroyed.  It seems the", "enemies of ours have", "passed away to dust like", "the forest." };

			Pages[17].Lines = new string[] { "I have spoken with other", "Juka and I suspect I have", "been told the truth.  All", "the Juka had powerful", "dreams.  In the dreams", "of ours the Meer invaded", "the fortress of ours and", "a great battle took place." };

			Pages[18].Lines = new string[] { " All the Juka and all the", "Meer perished and the", "fortress was destroyed", "from Adranath's spells.  I", "would not like to believe", "that the Meer could ever", "destroy us, but now it", "seems we have seen a" };

			Pages[19].Lines = new string[] { "vision of the fate of", "ours now lost in time.  I", "must now wonder if the", "Meer did not die in the", "battle with the Juka, how", "did they die?  And more", "importantly, where is", "Dasha?" };
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( "Khabur's Journal" );
		}

		public KaburJournal( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}