using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Dermott : MondainQuester
	{
		public override Type[] Quests { get { return null; } }

		[Constructable]
		public Dermott()
			: base( "Dermott", "the Weaver" )
		{
			SetSkill( SkillName.Magery, 60.0, 90.0 );
			SetSkill( SkillName.EvalInt, 60.0, 90.0 );
			SetSkill( SkillName.MagicResist, 60.0, 90.0 );
			SetSkill( SkillName.Wrestling, 60.0, 90.0 );
			SetSkill( SkillName.Meditation, 60.0, 90.0 );
		}

		public Dermott( Serial serial )
			: base( serial )
		{
		}

		public override void Advertise()
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Human;

			Hue = 0x83FC;
			HairItemID = 0x2049; // Pig Tails
			HairHue = 0x459;
			FacialHairItemID = 0x2041; // Mustache
			FacialHairHue = 0x459;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new Robe( 0x71B ) );
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