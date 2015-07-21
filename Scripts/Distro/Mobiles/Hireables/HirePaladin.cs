using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class HirePaladin : BaseHire
	{
		[Constructable]
		public HirePaladin()
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}
			Title = "the paladin";
			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNeutralHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			if ( Utility.RandomBool() && !this.Female )
			{
				Item beard = new Item( Utility.RandomList( 0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D ) );

				beard.Hue = hair.Hue;
				beard.Layer = Layer.FacialHair;
				beard.Movable = false;

				AddItem( beard );
			}
			switch ( Utility.Random( 5 ) )
			{
				case 0: break;
				case 1: AddItem( new Bascinet() ); break;
				case 2: AddItem( new CloseHelm() ); break;
				case 3: AddItem( new NorseHelm() ); break;
				case 4: AddItem( new Helmet() ); break;

			}

			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 10, 23 );

			SetSkill( SkillName.Swords, 66.0, 97.5 );
			SetSkill( SkillName.Anatomy, 65.0, 87.5 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );
			SetSkill( SkillName.Healing, 65.0, 87.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.Wrestling, 15.0, 37.5 );
			SetSkill( SkillName.Parry, 45.0, 60.5 );
			SetSkill( SkillName.Chivalry, 85, 100 );

			Fame = 100;
			Karma = 250;

			AddItem( new Shoes( Utility.RandomNeutralHue() ) );
			AddItem( new Shirt() );
			AddItem( new VikingSword() );
			AddItem( new MetalKiteShield() );


			AddItem( new PlateChest() );
			AddItem( new PlateLegs() );
			AddItem( new PlateArms() );
			AddItem( new LeatherGorget() );
			PackGold( 20, 100 );

		}
		public override bool ClickTitle { get { return false; } }

		public HirePaladin( Serial serial )
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
