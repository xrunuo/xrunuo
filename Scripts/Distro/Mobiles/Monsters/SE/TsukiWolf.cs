using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a tsuki wolf corpse" )]
	public class TsukiWolf : BaseCreature
	{
		private static int m_MinTime = 10;
		private static int m_MaxTime = 25;

		private DateTime m_NextAbilityTime;

		private RageTimer m_Timer;

		[Constructable]
		public TsukiWolf()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a tsuki wolf";
			Body = 250;
			BaseSoundID = 0xE5;

			switch ( Utility.RandomMinMax( 1, 3 ) )
			{
				case 1:
					Hue = 0;
					break;
				case 2:
					Hue = 448;
					break;
				case 3:
					Hue = 2305;
					break;
			}

			SetStr( 400, 450 );
			SetDex( 150, 200 );
			SetInt( 65, 75 );

			SetHits( 380, 440 );
			SetMana( 40, 40 );

			SetDamage( 15, 21 );

			SetDamageType( ResistanceType.Physical, 90 );
			SetDamageType( ResistanceType.Cold, 5 );
			SetDamageType( ResistanceType.Energy, 5 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.Anatomy, 65.1, 75.0 );
			SetSkill( SkillName.MagicResist, 65.1, 70.0 );
			SetSkill( SkillName.Tactics, 95.1, 110.0 );
			SetSkill( SkillName.Wrestling, 98.1, 108.0 );

			Fame = 200;
			Karma = -200;

			Tamable = false;

			PackGold( 400, 500 );
			PackItem( new Bone( Utility.RandomMinMax( 2, 3 ) ) );
			PackMagicItems( 1, 5 );
			PackItem( BaseLootHelperSE.RandomBodyPart() );
			PackItem( Loot.RandomArmor() );

			if ( Utility.RandomDouble() < .33 )
				PackItem( Engines.Plants.Seed.RandomPeculiarSeed( 1 ) );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( DateTime.UtcNow >= m_NextAbilityTime )
			{
				if ( BaseAttackHelperSE.IsUnderEffect( defender, BaseAttackHelperSE.m_RageTable ) )
					return;

				BaseAttackHelperSE.RageAttack( this, defender, ref m_Timer );

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}
		}

		public override void GenerateLoot()
		{
		}

		public override int Meat { get { return 4; } }
		public override int Hides { get { return 25; } }
		public override HideType HideType { get { return HideType.Regular; } }
		public override int TreasureMapLevel { get { return 3; } }

		public TsukiWolf( Serial serial )
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