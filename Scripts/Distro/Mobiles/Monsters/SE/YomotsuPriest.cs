using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "a Glowing Yomotsu corpse" )]
	public class YomotsuPriest : BaseCreature
	{
		private static double m_TurnChance = 0.6;

		private static int m_MinTime = 8;
		private static int m_MaxTime = 16;

		private DateTime m_NextAbilityTime;

		[Constructable]
		public YomotsuPriest()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an yomotsu priest";
			Body = 253;

			SetStr( 480, 530 );
			SetDex( 100, 115 );
			SetInt( 600, 675 );

			SetHits( 480, 530 );

			SetDamage( 13, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 65, 85 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 45, 65 );
			SetResistance( ResistanceType.Poison, 35, 55 );
			SetResistance( ResistanceType.Energy, 25, 50 );

			SetSkill( SkillName.EvalInt, 93.1, 108.0 );
			SetSkill( SkillName.Magery, 105.1, 115.0 );
			SetSkill( SkillName.Meditation, 95.1, 110.0 );
			SetSkill( SkillName.MagicResist, 112.1, 122.0 );
			SetSkill( SkillName.Tactics, 55.1, 60.0 );
			SetSkill( SkillName.Wrestling, 48.1, 58.0 );

			Fame = 15000;
			Karma = -15000;

			Tamable = false;

			PackGold( 700, 1000 );
			PackItem( new ExecutionersAxe() );
			PackItem( BaseLootHelperSE.RandomFootWears() );
			PackItem( new ShortPants() );
			PackMagicItems( 1, 5 );

			AddItem( Seed.RandomBonsaiSeed() );
		}

		public override int GetAngerSound()
		{
			return 0x453;
		}

		public override int GetIdleSound()
		{
			return 0x452;
		}

		public override int GetAttackSound()
		{
			return 0x454;
		}

		public override int GetHurtSound()
		{
			return 0x455;
		}

		public override int GetDeathSound()
		{
			return 0x456;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, 4 );
		}

		public override bool CanRummageCorpses { get { return true; } }

		private void ShowEffect()
		{
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );

			Effects.PlaySound( this, Map, 0x201 );
		}

		private void TryToChangeShape()
		{
			int[] bodys = new int[]
			{
				245,255,246,242,247,252,241,250,240,
			};

			int[] hues = new int[]
			{
				-1,-1,-1,-1,-1,-1,-1,-1,-1
			};

			if ( m_TurnChance < Utility.RandomDouble() ) return;

			if ( BodyMod != 0 )
			{
				ShowEffect();

				BodyMod = 0;
				HueMod = -1;

				return;
			}

			if ( Combatant == null ) return;

			int index = Utility.Random( bodys.Length );

			ShowEffect();

			BodyMod = bodys[index];

			HueMod = hues[index];
		}

		public override void OnThink()
		{
			if ( DateTime.UtcNow >= m_NextAbilityTime )
			{
				TryToChangeShape();

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}

			base.OnThink();
		}

		public YomotsuPriest( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}