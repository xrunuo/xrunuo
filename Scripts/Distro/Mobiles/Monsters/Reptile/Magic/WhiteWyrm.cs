using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a white wyrm corpse" )]
	public class WhiteWyrm : BaseCreature
	{
		private static readonly double TurnChance = 0.05;
		private static readonly double ReturnChance = 0.02;

		private ArrayList m_Items;

		[Constructable]
		public WhiteWyrm()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 180;
			Name = "a white wyrm";
			BaseSoundID = 362;

			SetStr( 721, 760 );
			SetDex( 101, 130 );
			SetInt( 386, 425 );

			SetHits( 433, 456 );

			SetDamage( 17, 25 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 55, 70 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 80, 90 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 99.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 100.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 18000;
			Karma = -18000;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 96.3;

			m_Items = new ArrayList();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems, Utility.Random( 1, 5 ) );
		}

		public override int TreasureMapLevel { get { return 4; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override int Scales { get { return 9; } }
		public override ScaleType ScaleType { get { return ScaleType.White; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public WhiteWyrm( Serial serial )
			: base( serial )
		{
		}

		private void InitOutfit()
		{
			Item hair = BaseMobileHelper.GetRandomHair();
			m_Items.Add( hair );

			if ( Utility.RandomBool() )
				m_Items.Add( BaseMobileHelper.GetRandomBeard( hair.Hue ) );

			m_Items.Add( BaseMobileHelper.GetRandomShirt() );
			m_Items.Add( BaseMobileHelper.GetRandomPants() );
			m_Items.Add( BaseMobileHelper.GetRandomFeet() );
		}

		public override void OnThink()
		{
			if ( Controlled || Combatant != null )
			{
				if ( BodyMod != 0 )
					BaseMobileHelper.Return( this, m_Items );

				return;
			}

			if ( BodyMod != 0 )
			{
				if ( Utility.RandomDouble() < ReturnChance )
					BaseMobileHelper.Return( this, m_Items );
			}
			else
			{
				if ( Utility.RandomDouble() < TurnChance )
				{
					InitOutfit();
					BaseMobileHelper.Turn( this, m_Items, 0x190, Utility.RandomSkinHue(), null, "the mystic llamaherder", true );
				}
			}

			base.OnThink();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );

			writer.Write( (int) BodyMod );
			writer.WriteItemList( m_Items );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
					{
						BodyMod = reader.ReadInt();
						goto case 1;
					}
				case 1:
					{
						m_Items = reader.ReadItemList();
						break;
					}
				case 0:
					{
						m_Items = new ArrayList();
						break;
					}
			}
		}
	}
}