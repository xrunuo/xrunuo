using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Egwexem : MondainQuester
	{
		private static Type[] m_Quests = new Type[] { typeof( RumorsAboundQuest ) };

		public override Type[] Quests { get { return m_Quests; } }

		[Constructable]
		public Egwexem()
			: base( "Egwexem", "the Noble" )
		{
		}

		public Egwexem( Serial serial )
			: base( serial )
		{
		}

		public override void Advertise()
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = true;
			Race = Race.Gargoyle;

			Hue = 0x86E8;
			HairItemID = 0x4261;
			HairHue = 0x31E;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new GlassStaff() );
			AddItem( new FemaleGargishClothKilt( 0x6A9 ) );
			AddItem( new FemaleGargishClothChest( 0x6BA ) );
			AddItem( new FemaleGargishClothArms( 0x75D ) );
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