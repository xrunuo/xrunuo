using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Evan : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof( HonestBeggarQuest ) }; } }

		[Constructable]
		public Evan()
			: base( "Evan", "the beggar" )
		{
		}

		public Evan( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 0x841B;
			HairItemID = 0x204A;
			HairHue = 0x451;
			FacialHairItemID = 0x203F;
			FacialHairHue = 0x451;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Shoes( 0x737 ) );
			AddItem( new ShortPants( 0x74C ) );
			AddItem( new FancyShirt( 0x535 ) );
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