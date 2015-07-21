using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Mielan : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( CircleOfLifeQuest )
			};
			}
		}

		[Constructable]
		public Mielan()
			: base( "Mielan", "the arcanist" )
		{
			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			SetSkill( SkillName.Focus, 60.0, 83.0 );
		}

		public Mielan( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Elf;

			Hue = 0x8376;
			HairItemID = 0x2FCE;
			HairHue = 0x368;
		}

		public override void InitOutfit()
		{
			AddItem( new ElvenBoots( 0x901 ) );
			AddItem( new ElvenPants( 0x901 ) );
			AddItem( new ElvenShirt() );
			AddItem( new GemmedCirclet() );
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