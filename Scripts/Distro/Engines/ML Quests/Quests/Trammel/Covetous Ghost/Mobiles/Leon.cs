using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Leon : MondainQuester
	{
		public override Type[] Quests { get { return null; } }

		[Constructable]
		public Leon()
			: base( "Leon", "the alchemist" )
		{
		}

		public Leon( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 0x83EB;
			HairItemID = 0x203C;
			HairHue = 0x454;
			FacialHairItemID = 0x204C;
			FacialHairHue = 0x454;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Shoes( 0x901 ) );
			AddItem( new Robe( 0x657 ) );
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