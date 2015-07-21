using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Lissbet : BaseEscort
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( ResponsibilityQuest )				
			};
			}
		}

		[Constructable]
		public Lissbet()
			: base()
		{
			Name = "Lissbet";
			Title = "The Flower Girl";
		}

		public Lissbet( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			Female = false;
			Race = Race.Human;

			Hue = 0x8411;
			HairItemID = 0x203D;
			HairHue = 0x1BB;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Sandals() );
			AddItem( new FancyShirt( 0x6BF ) );
			AddItem( new Kilt( 0x6AA ) );
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
