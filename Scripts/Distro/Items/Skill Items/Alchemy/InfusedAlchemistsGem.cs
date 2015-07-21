using System;
using Server;
using Server.Gumps;

namespace Server.Items
{
	public class InfusedAlchemistsGem : ScrollOfTranscendence
	{
		public override int LabelNumber { get { return 1113072; } } // Infused Alchemist's Gem

		[Constructable]
		public InfusedAlchemistsGem()
			: base( SkillName.Alchemy, 0.1 )
		{
			LootType = LootType.Regular;
			ItemID = 0x1EA7;
			Weight = 1.0;
			Hue = 2405;
		}

		public InfusedAlchemistsGem( Serial serial )
			: base( serial )
		{
		}

		public override Gump BuildGump( Mobile from, ScrollOfTranscendence scroll )
		{
			return new AlchemistGemGump( from, scroll );
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

		public class AlchemistGemGump : ScrollOfTranscendence.InternalGump
		{
			/* Infused Alchemist's Gem */
			public override int TitleMsg { get { return 1113072; } }

			/* Do you wish to use this gem? */
			public override int QuestionMsg { get { return 1113070; } }

			/* Using an Infused Gem for a Alchemy will permanently increase
			 * your current skill in Alchemy by the amount of points displayed
			 * on the gem. As you may not gain skills beyond your maximum skill
			 * cap, any excess points will be lost. */
			public override int InfoMsg { get { return 1113071; } }

			public AlchemistGemGump( Mobile from, ScrollOfTranscendence scroll )
				: base( from, scroll )
			{
			}
		}
	}
}