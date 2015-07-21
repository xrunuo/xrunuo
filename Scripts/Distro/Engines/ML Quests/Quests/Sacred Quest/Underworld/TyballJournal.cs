using System;
using Server;

namespace Server.Items
{
	public class TyballJournal : BaseBook
	{
		[Constructable]
		public TyballJournal()
			: base( 0xFF2, "a journal", "Tyball", 20, false )
		{
			Hue = 0x485;
			Movable = false;

			Pages[0].Lines = new string[]
				{
					"He speaks to me... I can",
					"hear him. The beast... He",
					"knows of our plan. But",
					"how?",
					"",
					"Such wonders, such",
					"powers and such wealth",
					"he promises. But at what"
				};
			Pages[1].Lines = new string[]
				{
					"cost? That it could ask",
					"me to sacrifice my",
					"beloved brother. I cannot.",
					"I will not. No knowledge,",
					"no greatness could",
					"warrant such infamy.",
					"",
					"But the seed of a doubt"
				};
			Pages[2].Lines = new string[]
				{
					"is gnawing at me. Did the",
					"Slasher make the same",
					"offer to my brother? Is",
					"he playing us against the",
					"other? Would Garamon",
					"even entertain the",
					"thought? No, not my",
					"virtuous brother.  If he"
				};
			Pages[3].Lines = new string[]
				{
					"did, I would need to",
					"strike first. Only to",
					"protect myself, of course.",
					"",
					"I must banish the doubts",
					"from my mind. They are",
					"like poison. We cannot let",
					"this fiend divide us."
				};
		}

		public TyballJournal( Serial serial )
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