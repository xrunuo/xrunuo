using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Emilio : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof( UnfadingMemoriesOneQuest ) }; } }

		[Constructable]
		public Emilio()
			: base( "Emilio", "the tortured artist" )
		{
		}

		public Emilio( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 0x83EB;
			HairItemID = 0x2048;
			HairHue = 0x470;
			FacialHairItemID = 0x204C;
			FacialHairHue = 0x470;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Sandals( 0x721 ) );
			AddItem( new LongPants( 0x51B ) );
			AddItem( new FancyShirt( 0x517 ) );
			AddItem( new FloppyHat( 0x584 ) );
			AddItem( new BodySash( 0x13 ) );
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