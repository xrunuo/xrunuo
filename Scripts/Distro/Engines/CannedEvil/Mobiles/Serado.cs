using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Engines.CannedEvil;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "an dark yamandon corpse" )]
	public class Serado : BaseChampion
	{
		public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

		public override Type[] UniqueList { get { return new Type[] { typeof( Pacify ) }; } }
		public override Type[] SharedList
		{
			get
			{
				return new Type[] {
					typeof( BraveKnightOfTheBritannia ),
					typeof( DetectiveBoots ),
					typeof( EmbroideredOakLeafCloak ),
					typeof( LieutenantOfTheBritannianRoyalGuard )
				};
			}
		}
		public override Type[] DecorativeList { get { return new Type[] { typeof( Futon ), typeof( SwampTile ) }; } }

		public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { }; } }

		public static int AbilityRange { get { return 10; } }

		private static int m_MinTime = 10;
		private static int m_MaxTime = 20;

		private DateTime m_NextAbilityTime;

		private static int[] m_Resist = new int[]
		{
			30,60,60,90,50
		};

		private static int[] m_ResistMax = new int[]
		{
			95,95,95,95,95
		};

		public ResistanceMod[] m_Mods = new ResistanceMod[5];

		public int m_HitsLast = -1;

		[Constructable]
		public Serado()
			: base( AIType.AI_Melee )
		{
			Name = "Serado";
			Title = "the awakened";

			Body = 249;

			Tamable = false;

			Hue = 0x96c;

			SetStr( 1000, 1000 );
			SetDex( 150, 150 );
			SetInt( 300, 300 );
			SetHits( 9000, 9000 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 10 );

			SetResistance( ResistanceType.Physical, 0, 0 );
			SetResistance( ResistanceType.Fire, 0, 0 );
			SetResistance( ResistanceType.Cold, 0, 0 );
			SetResistance( ResistanceType.Poison, 0, 0 );
			SetResistance( ResistanceType.Energy, 0, 0 );

			SetSkill( SkillName.MagicResist, 120.0, 120.0 );
			SetSkill( SkillName.Tactics, 120.0, 120.0 );
			SetSkill( SkillName.Wrestling, 70.0, 70.0 );
			SetSkill( SkillName.Poisoning, 150.0, 150.0 );

			Fame = 24000;
			Karma = -24000;

			AddItem( new Gold( 2000, 2500 ) );

			PackMagicItems( 1, 4 );

			AddItem( Seed.RandomBonsaiSeed() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Gems, 6 );
		}

		public override void OnThink()
		{
			double value;
			int i;

			if ( Hits != m_HitsLast )
			{
				if ( m_HitsLast != -1 )
				{
					for ( i = 0; i < m_Mods.Length; i++ )
					{
						RemoveResistanceMod( m_Mods[i] );
					}
				}

				for ( i = 0; i < m_Mods.Length; i++ )
				{
					value = ( (double) ( HitsMax - Hits ) ) * ( (double) ( ( m_ResistMax[i] - m_Resist[i] ) / (double) HitsMax ) );

					m_Mods[i] = new ResistanceMod( (ResistanceType) i, m_Resist[i] + (int) value );
				}

				for ( i = 0; i < m_Mods.Length; i++ )
				{
					AddResistanceMod( m_Mods[i] );
				}

				m_HitsLast = Hits;
			}

			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile target = BaseAttackHelperSE.GetRandomAttacker( this, Yamandon.AbilityRange );

				if ( target != null )
				{
					BaseAttackHelperSE.SpillAcid( target, true );
				}

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}

			base.OnThink();
		}

		public override FoodType FavoriteFood { get { return FoodType.Fish; } }

		public override int TreasureMapLevel { get { return 5; } }

		public override bool Unprovokable { get { return true; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override double HitPoisonChance { get { return 1.0; } }
		public override bool BardImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int Hides { get { return 30; } }

		public override bool ShowFameTitle { get { return false; } }
		public override bool ClickTitle { get { return false; } }

		public Serado( Serial serial )
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
