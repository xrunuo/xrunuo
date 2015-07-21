using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	[CorpseName( "a young ninja's corpse" )]
	public class YoungNinja : BaseCreature
	{
		[Constructable]
		public YoungNinja()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			InitStats( 45, 30, 5 );
			SetHits( 20, 30 );

			Hue = 0x8419;

			Body = 0x190;

			Name = "a young ninja";

			Utility.AssignRandomHair( this );
			Utility.AssignRandomFacialHair( this );

			AddItem( new NinjaTabi() );
			AddItem( new LeatherNinjaPants() );
			AddItem( new LeatherNinjaJacket() );
			AddItem( new Bandana() );
			AddItem( new LeatherNinjaBelt() );

			switch ( Utility.Random( 3 ) )
			{
				case 0: AddItem( new Tessen() ); break;
				case 1: AddItem( new Kama() ); break;
				default: AddItem( new Lajatang() ); break;
			}

			SetSkill( SkillName.Swords, 50.0 );
			SetSkill( SkillName.Tactics, 50.0 );
		}

		public override bool ClickTitle { get { return false; } }

		public override bool PlayerRangeSensitive { get { return false; } }
		public override bool AlwaysMurderer { get { return true; } }

		public YoungNinja( Serial serial )
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