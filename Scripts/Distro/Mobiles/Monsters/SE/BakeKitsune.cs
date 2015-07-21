using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a bake kitsune corpse" )]
	public class BakeKitsune : BaseCreature
	{
		private static readonly int MinTime = 12;
		private static readonly int MaxTime = 20;
		
		private DateTime m_NextAbilityTime;
		
		private RageTimer m_Timer;
		
		private static double TurnChance = 0.05;
		private static double ReturnChance = 0.02;

		private ArrayList m_Items = new ArrayList();
		
		public override int GetAngerSound()
		{
			return 0x4DE;
		}

		public override int GetIdleSound()
		{
			return 0x4DD;
		}

		public override int GetAttackSound()
		{
			return 0x4DC;
		}

		public override int GetHurtSound()
		{
			return 0x4DF;
		}

		public override int GetDeathSound()
		{
			return 0x4DB;
		}

		[Constructable]
		public BakeKitsune()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bake kitsune";
			Body = 246;

			SetStr( 171, 220 );
			SetDex( 126, 145 );
			SetInt( 376, 425 );

			SetHits( 301, 350 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Energy, 30 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 70, 90 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 40, 60 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 80.1, 90.0 );
			SetSkill( SkillName.Meditation, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 80.1, 100.0 );
			SetSkill( SkillName.Tactics, 70.1, 90.0 );
			SetSkill( SkillName.Wrestling, 50.1, 55.0 );

			Fame = 8000;
			Karma = -8000;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 80.7;

			if ( Utility.RandomDouble() < .25 )
				PackItem( Engines.Plants.Seed.RandomBonsaiSeed() );
		}
		
		public BakeKitsune( Serial serial )
			: base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override int Meat { get { return 5; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override FoodType FavoriteFood { get { return FoodType.Fish; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( DateTime.Now >= m_NextAbilityTime )
			{
				if ( BaseAttackHelperSE.IsUnderEffect( defender, BaseAttackHelperSE.m_RageTable ) )
					return;

				BaseAttackHelperSE.RageAttack( this, defender, ref m_Timer );

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( MinTime, MaxTime ) );
			}

		}

		private void InitOutfit()
		{
			int[] hues = new int[] {0x1A8, 0xEC, 0x99, 0x90, 0xB5, 0x336, 0x89};

			m_Items.Add( new Kasa() );
			m_Items.Add( new TattsukeHakama( hues[ Utility.Random( hues.Length ) ] ) );
			m_Items.Add( new HakamaShita( 0x2C3 ) );
			m_Items.Add( new NinjaTabi( 0x2C3 ) );
		}

		public override void OnThink()
		{
			if ( Controlled )
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
					BaseMobileHelper.Turn( this, m_Items, 0x190, Utility.RandomSkinHue(), NameList.RandomName( "male" ), "the mystic traveller", true );
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