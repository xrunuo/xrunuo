using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Drithen : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( TaleOfTailQuest ),
				typeof( PointyEarsQuest )
			};
			}
		}

		[Constructable]
		public Drithen()
			: base( "Drithen", "the fierce" )
		{
		}

		public Drithen( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;
			Hue = 0x840F;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new ElvenBoots( 0x723 ) );
			AddItem( new LongPants( 0x549 ) );
			AddItem( new Tunic( 0x72B ) );
			AddItem( new Cloak( 0x30 ) );
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