using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class YoungRonin : BaseCreature
	{
		[Constructable]
		public YoungRonin()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			InitStats( 35, 30, 5 );

			Hue = 0x83FF;

			Body = 0x190;

			Name = "a young ronin";

			Utility.AssignRandomHair( this );
			Utility.AssignRandomFacialHair( this );

			AddItem( new SamuraiTabi() );
			AddItem( new LeatherSuneate() );
			AddItem( new LeatherDo() );
			AddItem( new LeatherHiroSode() );
			AddItem( new Bandana() );
			switch ( Utility.Random( 3 ) )
			{
				case 0: AddItem( new NoDachi() ); break;
				case 1: AddItem( new Lajatang() ); break;
				default: AddItem( new Wakizashi() ); break;
			}

			SetSkill( SkillName.Swords, 50.0 );
			SetSkill( SkillName.Tactics, 50.0 );
		}

		public override bool ClickTitle { get { return false; } }

		public override bool PlayerRangeSensitive { get { return false; } }
		public override bool AlwaysMurderer { get { return true; } }

		public YoungRonin( Serial serial )
			: base( serial )
		{
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
