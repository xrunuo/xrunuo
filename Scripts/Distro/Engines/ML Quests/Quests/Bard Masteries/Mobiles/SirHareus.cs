using System;
using Server;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
	public class SirHareus : MondainQuester
	{
		private static Type[] m_Quests = new Type[]
			{
				typeof( IndoctrinationOfABattleRouserQuest )
			};

		public override Type[] Quests
		{
			get { return m_Quests; }
		}

		[Constructable]
		public SirHareus()
			: base( "Sir Hareus", "the Battle Rouser" )
		{
		}

		public SirHareus( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			CantWalk = true;
			Race = Race.Human;
			Hue = 0x83EA;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new ChainChest() { Hue = 784 } );
			AddItem( new BodySash() { Hue = 253 } );
			AddItem( new LongPants() { Hue = 2305 } );
			AddItem( new Shoes() { Hue = 2305 } );
			AddItem( new Halberd() );
		}

		public override void Advertise()
		{
		}

		public override void OnOfferFailed()
		{
			Say( 1080107 ); // I'm sorry, I have nothing for you at this time.
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
