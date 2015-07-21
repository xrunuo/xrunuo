using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Samurai : BaseCreature
	{
		[Constructable]
		public Samurai()
			: base( AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0 )
		{
			Title = "the samurai";

			if ( Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
			}

			SetSkill( SkillName.Parry, 80.0, 100.0 );
			SetSkill( SkillName.Swords, 80.0, 100.0 );
			SetSkill( SkillName.ArmsLore, 80.0, 100.0 );
			SetSkill( SkillName.Bushido, 80.0, 100.0 );

			AddItem( new DecorativePlateKabuto() );
			AddItem( new LeatherDo() );
			AddItem( new LeatherDo() );
			AddItem( new PlateSuneate() );
			AddItem( new LeatherHiroSode() );
			AddItem( new Wakizashi() );

			int lowHue = GetRandomHue();

			AddItem( new SamuraiTabi( lowHue ) );

			int hairHue = Utility.RandomNondyedHue();

			Utility.AssignRandomHair( this, hairHue );

			if ( Utility.Random( 7 ) != 0 )
				Utility.AssignRandomFacialHair( this, hairHue );

			PackGold( 200, 250 );
		}

		public override bool CanTeach { get { return true; } }
		public override bool ClickTitle { get { return false; } } // Do not display 'the samurai' when single-clicking

		private static int GetRandomHue()
		{
			switch ( Utility.Random( 6 ) )
			{
				default:
				case 0:
					return 0;
				case 1:
					return Utility.RandomBlueHue();
				case 2:
					return Utility.RandomGreenHue();
				case 3:
					return Utility.RandomRedHue();
				case 4:
					return Utility.RandomYellowHue();
				case 5:
					return Utility.RandomNeutralHue();
			}
		}

		public Samurai( Serial serial )
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