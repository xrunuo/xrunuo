using System;
using Server;

namespace Server.Items
{
	public class GaramonJournal : BaseBook
	{
		[Constructable]
		public GaramonJournal()
			: base( 0xFF2, "a journal", "Garamon", 20, false )
		{
			Hue = 0x45A;
			Movable = false;

			Pages[0].Lines = new string[]
				{
					"Today, I have hope again.",
					"It has been too many",
					"days since my brother",
					"Tyball and I inadvertently",
					"released the Slasher of",
					"Veils in this world. How",
					"could we have known our",
					"research in planar travel"
				};
			Pages[1].Lines = new string[]
				{
					"would have such dire",
					"consequences?",
					"",
					"But we have devised a",
					"plan. We completed the",
					"construction of a",
					"Chamber of Virtue.",
					"Tonight, I will lure the"
				};
			Pages[2].Lines = new string[]
				{
					"Slasher of Veils inside it",
					"so its virtuous energies",
					"may weaken the beast.",
					"Tyball will lock us in while",
					"I open a rip back to the",
					"Slasher's own plane and",
					"lead him through. A",
					"portal already awaits me"
				};
			Pages[3].Lines = new string[]
				{
					"in that foul place which",
					"will lead me back here to",
					"safety.",
					"",
					"If all goes according to",
					"plan, we will have undone",
					"the wrong we brought",
					"onto Britannia. We will"
				};
			Pages[4].Lines = new string[]
				{
					"have redeemed ourselves.",
					"May the Virtues give us",
					"strength..."
				};
		}

		public GaramonJournal( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}