using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Aurelia : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof( AemaethOneQuest ) }; } }

		[Constructable]
		public Aurelia()
			: base( "Aurelia", "the architect's daughter" )
		{
		}

		public Aurelia( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = true;
			Race = Race.Human;

			Hue = 0x83F7;
			HairItemID = 0x2047;
			HairHue = 0x457;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Sandals( 0x4B7 ) );
			AddItem( new Skirt( 0x4B4 ) );
			AddItem( new FancyShirt( 0x659 ) );
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