using System;
using Server;

namespace Server.Items
{
	public class DrakovsJournal : BlueBook
	{
		[Constructable]
		public DrakovsJournal()
			: base( "Drakov's Journal", "Drakov", 9, false )
		{
			Pages[0].Lines = new string[] { "My Master", "", "This journal was", "found on one of", "our controllers.  It", "seems he has lost", "faith in you.  Know", "that he has been" };

			Pages[1].Lines = new string[] { "dealth with and will", "never again speak", "ill of you or our", "cause.", "          -Galzon" };

			Pages[2].Lines = new string[] { "We have completted", "construction of the", "devices needed to", "build the clockwork", "overseers and minions", "as per the request of", "the Master.  The", "gargoyles have been" };

			Pages[3].Lines = new string[] { "most useful and their", "knowledge of the", "techniques for the", "construction of these", "creatures will serve", "us well.", "        -----", "I am not one to" };

			Pages[4].Lines = new string[] { "criticize the Master,", "but I believe he may", "have erred in his", "decision to destroy", "the wingless ones.", "Already our forces", "are weakened by the", "constant attacks of" };

			Pages[5].Lines = new string[] { "the humans  Their", "strength and", "unquestioning", "compliance would", "have made them very", "useful in the fight", "against the humans.", "But the Master felt" };

			Pages[6].Lines = new string[] { "their presence to be", "an annoyance and", "a distraction to the", "winged ones.  It was", "not difficult at all", "to remove them from", "this world.  But now", "I fear without more" };

			Pages[7].Lines = new string[] { "allies, willing or", "not, we stand", "little chance of", "defeating the foul", "humans from our", "lands.  Perhaps if", "the Master had", "shown a little" };

			Pages[8].Lines = new string[] { "mercy and forsight", "we would not be", "in such dire peril." };
		}

		public DrakovsJournal( Serial serial )
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