using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Sledge : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( IngenuityQuest ),
				typeof( PointyEarsQuest )				
			};
			}
		}

		[Constructable]
		public Sledge()
			: base( "Sledge", "The Versatile" )
		{
		}

		public Sledge( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			base.InitBody();
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new ElvenBoots( 0x736 ) );
			AddItem( new LongPants( 0x521 ) );
			AddItem( new Tunic( 0x71E ) );
			AddItem( new Cloak( 0x59 ) );
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
