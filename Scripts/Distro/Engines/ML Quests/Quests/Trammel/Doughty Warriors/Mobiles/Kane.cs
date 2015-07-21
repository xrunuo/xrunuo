using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Kane : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof( DoughtyWarriorsOneQuest ) }; } }

		[Constructable]
		public Kane()
			: base( "Kane", "the Master of Arms" )
		{
		}

		public Kane( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 33794;

			HairItemID = 0x2049;
			HairHue = 0x46F;
			FacialHairItemID = 0x203E;
			FacialHairHue = 0x46F;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Sandals( 1814 ) );
			AddItem( new LongPants( 1204 ) );
			AddItem( new Shirt( 1329 ) );
			AddItem( new Cloak( 443 ) );
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