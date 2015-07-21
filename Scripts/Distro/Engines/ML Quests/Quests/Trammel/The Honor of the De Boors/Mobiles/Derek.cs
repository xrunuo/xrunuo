using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Derek : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof( TheHonorOfTheDeBoorsQuest ) }; } }

		[Constructable]
		public Derek()
			: base( "Derek", "the Merchant" )
		{
		}

		public Derek( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 0x8406;

			HairItemID = 0x2048;
			HairHue = 0x473;
			FacialHairItemID = 0x204B;
			FacialHairHue = 0x473;
		}

		public override void InitOutfit()
		{
			AddItem( new Shoes() );
			AddItem( new LongPants( 0x901 ) );
			AddItem( new FancyShirt( 0x5F4 ) );
			AddItem( new Backpack() );
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