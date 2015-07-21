using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class SomethingFishyQuest : BaseQuest
	{
		/* Something Fishy */
		public override object Title { get { return 1095041; } }

		/* Catch a Red Herring from the underground river in the Abyss and bring it
		 * to Barreraak for your reward.
		 * -----
		 * You kill Green Goblins. You one of us!  Big gray goblin, you are.
		 * But not so gray.
		 * 
		 * Barreraak will come to point... Fishing is good.  Fish are in river.
		 * Gray goblins fish, fish make good meat.  Not better than rotworm,
		 * but goblin can't eat  rotworm all the time... Make goblin too fat.
		 * 
		 * Goblin King make good contest.  Catch special red fish.  Red Herring
		 * good fish to eat.  Very hard to catch.  Often Barreraak fish in wrong
		 * place.  Very strange.
		 * 
		 * Barreraak make deal with you.  You bring Red Herring to Barreraak,
		 * Barreraak give you something good.  Barreraak bring red herring fish
		 * to King and be big hero.  How that deal sound?
		 */
		public override object Description { get { return 1095043; } }

		/* That fine.  Barreraak keep trying to get for himself.
		 * Maybe get better friend who can catch fish.
		 */
		public override object Refuse { get { return 1095044; } }

		/* You not have fish.  Very important to fish for Red Herring.
		 */
		public override object Uncomplete { get { return 1095045; } }

		/* Oh, this good Red Herring.  Now Barreraak get to be new King of Gray
		 * Goblins.  You killed the old King when you first came here.  As new
		 * King, Barreraak give you special ring.  This ring make you better at
		 * killing goblins that say Barreraak not new King.
		 */
		public override object Complete { get { return 1095048; } }

		public override bool DoneOnce { get { return true; } }

		public SomethingFishyQuest()
		{
			AddObjective( new ObtainObjective( typeof( RedHerring ), "Red Herring", 1 ) );

			AddReward( new BaseReward( typeof( BarreraaksRing ), 1095049 ) ); // Barreraak’s Old Beat Up Ring
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

	public class Barreraak : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( SomethingFishyQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
		}

		[Constructable]
		public Barreraak()
			: base( "Barreraak" )
		{
		}

		public Barreraak( Serial serial )
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