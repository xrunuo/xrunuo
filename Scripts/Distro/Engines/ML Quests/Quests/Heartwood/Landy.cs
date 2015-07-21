using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Landy : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( CreepyCrawliesQuest ),
				typeof( MongbatMenaceQuest ),
				typeof( SpecimensQuest ),
				typeof( ThinningTheHerdQuest )
			};
			}
		}

		[Constructable]
		public Landy()
			: base( "Landy", "the soil nurturer" )
		{
			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			SetSkill( SkillName.Focus, 60.0, 83.0 );
		}

		public Landy( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Elf;

			Hue = 0x8384;
			HairItemID = 0x2FCE;
			HairHue = 0x91;
		}

		public override void InitOutfit()
		{
			AddItem( new Sandals( 0x901 ) );
			AddItem( new Tunic( 0x719 ) );
			AddItem( new ShortPants( 0x1BB ) );

			Item item;

			item = new LeafGloves();
			item.Hue = 0x1BB;
			AddItem( item );
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