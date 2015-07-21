using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Belulah : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( AllSeasonAdventurerQuest )			
			};
			}
		}

		[Constructable]
		public Belulah()
			: base( "Belulah", "The Scorned" )
		{
		}

		public Belulah( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			Female = true;
			Race = Race.Human;

			Hue = 0x83F7;
			HairItemID = 0x2046;
			HairHue = 0x463;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Boots() );
			AddItem( new LongPants( 0x6C7 ) );
			AddItem( new FancyShirt( 0x6BB ) );
			AddItem( new Cloak( 0x59 ) );
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
