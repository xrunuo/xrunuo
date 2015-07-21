using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class HireBardArcher : BaseHire
	{
		[Constructable]
		public HireBardArcher()
			: base( AIType.AI_Archer )
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );

				switch ( Utility.Random( 2 ) )
				{
					case 0: AddItem( new Skirt( Utility.RandomDyedHue() ) ); break;
					case 1: AddItem( new Kilt( Utility.RandomNeutralHue() ) ); break;
				}
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}
			Title = "the bard";
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

			SetStr( 16, 16 );
			SetDex( 26, 26 );
			SetInt( 26, 26 );

			SetDamage( 5, 10 );

			SetSkill( SkillName.Tactics, 35, 57 );
			SetSkill( SkillName.Magery, 22, 22 );
			SetSkill( SkillName.Swords, 45, 67 );
			SetSkill( SkillName.Archery, 36, 67 );
			SetSkill( SkillName.Parry, 45, 60 );
			SetSkill( SkillName.Musicianship, 66.0, 97.5 );
			SetSkill( SkillName.Peacemaking, 65.0, 87.5 );

			Fame = 100;
			Karma = 100;

			AddItem( new Shoes( Utility.RandomNeutralHue() ) );

			switch ( Utility.Random( 2 ) )
			{
				case 0: AddItem( new Doublet( Utility.RandomDyedHue() ) ); break;
				case 1: AddItem( new Shirt( Utility.RandomDyedHue() ) ); break;
			}
			switch ( Utility.Random( 4 ) )
			{
				case 0: PackItem( new Harp() ); break;
				case 1: PackItem( new Lute() ); break;
				case 2: PackItem( new Drums() ); break;
				case 3: PackItem( new Tambourine() ); break;
			}

			PackItem( new Longsword() );
			AddItem( new Bow() );
			PackItem( new Arrow( 100 ) );
			PackGold( 10, 50 );
		}
		public override bool ClickTitle { get { return false; } }
		public HireBardArcher( Serial serial )
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
