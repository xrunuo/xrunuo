using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Ben : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof( GhostOfCovetousQuest ) }; } }

		[Constructable]
		public Ben()
			: base( "Ben", "the apprentice necromancer" )
		{
		}

		public Ben( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 0x83FD;
			HairItemID = 0x2048;
			HairHue = 0x463;
			FacialHairItemID = 0x204C;
			FacialHairHue = 0x463;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Shoes( 0x901 ) );
			AddItem( new LongPants( 0x1BB ) );
			AddItem( new FancyShirt( 0x756 ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}