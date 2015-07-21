using System;
using Server;

namespace Server.Items
{
	public class FropozJournal : RedBook
	{
		[Constructable]
		public FropozJournal()
			: base( "Journal", "Fropoz", 13, false )
		{
			Pages[0].Lines = new string[] { "I have done as my", "Master has", "instructed me.", "", "The painted humans", "have been driven into", "Britannia and are even", "now wreaking havoc" };

			Pages[1].Lines = new string[] { "across the land,", "providing us with the", "distraction my Master", "requested.  We", "have provided them", "with the masks", "necessary to defeat", "the orcs, thus" };

			Pages[2].Lines = new string[] { "causing even more", "distress for the people", "of Britannia.  The", "unsuspecting fools", "are too busy dealing", "with the orc hordes to", "continue their", "exploration of our" };

			Pages[3].Lines = new string[] { "lands.  We are", "safe...for now.", "     ----", "The attacks", "continue exactly as", "planned.  My Master", "is pleased with my", "work and we are" };

			Pages[4].Lines = new string[] { "closer to our goals than", "ever before.  The", "gargoyles have proven", "to be more troublesome", "than we first", "anticipated, but I", "believe we can", "subjugate them fully" };

			Pages[5].Lines = new string[] { "given enough time.  It's", "unfortunate that we", "did not discover their", "knowledge sooner.", "Even now they", "prepare our armies", "for battle, but not", "without resistance." };

			Pages[6].Lines = new string[] { "Now that some of", "them know of the", "other lands and of", "humans, they will", "double their efforts to", "seek help.  This", "cannot be allowed.", "    -----" };

			Pages[7].Lines = new string[] { "Damn them!!  The", "humans proved", "more resourcefull than", "we thought them", "capable of.  Already", "their homes are free", "of orcs and savages", "and they once again" };

			Pages[8].Lines = new string[] { "are treading in our", "lands.  We may have to", "move sooner than we", "thought.  I will", "prepar my brethern", "and our golems.", "Hopefully, we can", "buy our Master some" };

			Pages[9].Lines = new string[] { "more time before the", "humans discover us.", "     -----", "It's too late.  The", "gargoyles whom have", "evaded our capture", "have opened the doors", "to our land." };

			Pages[10].Lines = new string[] { "They pray the", "humans will help", "them, despite the", "actions of their", "cousins in Britannia.  I", "fear they are right.", "I must go to warn", "the MastKai Hohiro," };

			Pages[12].Lines = new string[] { "10.11.2001", "first one to be here", "", "Congrats. I didn't really", "care to log on earlier,", "nor did I come straight", "here. 2pm, Magus" };
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( "Fropoz's Journal" );
		}

		public FropozJournal( Serial serial )
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