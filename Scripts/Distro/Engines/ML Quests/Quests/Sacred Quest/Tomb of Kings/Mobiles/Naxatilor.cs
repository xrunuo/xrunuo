using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Naxatilor : MondainQuester
	{
		public override Type[] Quests { get { return null; } }

		[Constructable]
		public Naxatilor()
			: base( "Naxatilor", "the Seer" )
		{
		}

		public Naxatilor( Serial serial )
			: base( serial )
		{
		}

		public override void Advertise()
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Race = Race.Gargoyle;

			Hue = 0x86E8;
			HairItemID = 0x425B;
			HairHue = 0x320;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new GlassStaff() );
			AddItem( new GargishClothKilt( 0x51B ) );
			AddItem( new GargishClothChest( 0x51B ) );
			AddItem( new GargishClothArms( 0x53C ) );
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