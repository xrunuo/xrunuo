using System;
using Server;
using Server.Items;
using Server.Mobiles;
using LevelInfo = Server.Engines.MiniChamps.MiniChampLevelInfo;
using TypeInfo = Server.Engines.MiniChamps.MiniChampTypeInfo;

namespace Server.Engines.MiniChamps
{
	public enum MiniChampType
	{
		SecretGarden,
		StygianDragon,
		CrimsonVeins,
		AbyssalLair,
		FireTempleRuins,
		LandsOfTheLich,
		SkeletalDragon,
		EnslavedGoblins,
		LavalCaldera,
		PassageOfTears,
		ClanChitter,
		ClanRibbon,
		ClanScratch,
		Minotaur
	}

	public class MiniChampTypeInfo
	{
		private int m_Required;
		private Type m_SpawnType;

		public int Required { get { return m_Required; } }
		public Type SpawnType { get { return m_SpawnType; } }

		public MiniChampTypeInfo( int required, Type spawnType )
		{
			m_Required = required;
			m_SpawnType = spawnType;
		}
	}

	public class MiniChampLevelInfo
	{
		private MiniChampTypeInfo[] m_Types;

		public MiniChampTypeInfo[] Types { get { return m_Types; } }

		public MiniChampLevelInfo( params MiniChampTypeInfo[] types )
		{
			m_Types = types;
		}
	}

	public class MiniChampInfo
	{
		private MiniChampLevelInfo[] m_Levels;
		private Type m_EssenceType;

		public MiniChampLevelInfo[] Levels { get { return m_Levels; } }
		public Type EssenceType { get { return m_EssenceType; } }

		public MiniChampInfo( Type essenceType, params MiniChampLevelInfo[] levels )
		{
			m_EssenceType = essenceType;
			m_Levels = levels;
		}

		public MiniChampLevelInfo GetLevelInfo( int level )
		{
			level--;

			if ( level >= 0 && level < m_Levels.Length )
				return m_Levels[level];

			return null;
		}

		public static MiniChampInfo[] Table { get { return m_Table; } }

		private static readonly MiniChampInfo[] m_Table = new MiniChampInfo[]
		{
			new MiniChampInfo( /* Secret Garden */
				typeof( EssenceOfFeeling ),
				new LevelInfo( new TypeInfo( 20, typeof( Pixie ) ) ),
				new LevelInfo( new TypeInfo( 15, typeof( Wisp ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( DarkWisp ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( PixieRenowned ) ) )
			),
			new MiniChampInfo( /* Stygian Dragon Lair Entrance */
				typeof( EssenceOfDiligence ),
				new LevelInfo( new TypeInfo( 25, typeof( FairyDragon ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( Wyvern ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( ForgottenServant ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( WyvernRenowned ) ) )
			),
			new MiniChampInfo( /* Crimson Veins */
				typeof( EssenceOfPrecision ),
				new LevelInfo( new TypeInfo( 20, typeof( FireAnt ) ),
							   new TypeInfo( 10, typeof( LavaLizard ) ),
							   new TypeInfo( 10, typeof( LavaSnake ) ) ),
				new LevelInfo( new TypeInfo( 5, typeof( FireGargoyle ) ),
							   new TypeInfo( 5, typeof( Efreet ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( LavaElemental ) ),
							   new TypeInfo( 5, typeof( FireDaemon ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( FireElementalRenowned ) ) )
			),
			new MiniChampInfo( /* Abyssal Lair Entrance */
				typeof( EssenceOfAchievement ),
				new LevelInfo( new TypeInfo( 20, typeof( StrongMongbat ) ),
							   new TypeInfo( 20, typeof( Imp ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( Daemon ) ) ),
				new LevelInfo( new TypeInfo( 5, typeof( PitFiend ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( DevourerOfSoulsRenowned ) ) )
			),
			new MiniChampInfo( /* Fire Temple Ruins */
				typeof( EssenceOfOrder ),
				new LevelInfo( new TypeInfo( 20, typeof( LavaSnake ) ),
							   new TypeInfo( 10, typeof( LavaLizard ) ),
							   new TypeInfo( 10, typeof( FireAnt ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( LavaSerpent ) ),
							   new TypeInfo( 10, typeof( HellCat ) ),
							   new TypeInfo( 10, typeof( HellHound ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( LavaElemental ) ),
							   new TypeInfo( 5, typeof( FireDaemon ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( FireDaemonRenowned ) ) )
			),
			new MiniChampInfo( /* Lands of the Lich */
				typeof( EssenceOfDirection ),
				new LevelInfo( new TypeInfo( 30, typeof( Skeleton ) ),
							   new TypeInfo( 20, typeof( Zombie ) ),
							   new TypeInfo( 10, typeof( Spectre ) ),
							   new TypeInfo( 5, typeof( Shade ) ),
							   new TypeInfo( 5, typeof( Wraith ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( SkeletalMage ) ),
							   new TypeInfo( 10, typeof( WailingBanshee ) ) ),
				new LevelInfo( new TypeInfo( 20, typeof( RottingCorpse ) ),
							   new TypeInfo( 5, typeof( SkeletalLich ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( AncientLichRenowned ) ) )
			),
			new MiniChampInfo( /* Skeletal Dragon */
				typeof( EssenceOfPersistence ),
				new LevelInfo( new TypeInfo( 15, typeof( Skeleton ) ),
							   new TypeInfo( 5, typeof( PatchworkSkeleton ) ) ),
				new LevelInfo( new TypeInfo( 5, typeof( BoneKnight ) ),
							   new TypeInfo( 5, typeof( SkeletalKnight ) ) ),
				new LevelInfo( new TypeInfo( 5, typeof( BoneMagi ) ),
							   new TypeInfo( 2, typeof( SkeletalMage ) ) ),
				new LevelInfo( new TypeInfo( 2, typeof( SkeletalLich ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( SkeletalDragonRenowned ) ) )
			),
			new MiniChampInfo( /* Enslaved Goblins */
				typeof( EssenceOfControl ),
				new LevelInfo( new TypeInfo( 15, typeof( EnslavedGreenGoblin ) ),
							   new TypeInfo( 10, typeof( EnslavedGrayGoblin ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( EnslavedGoblinScout ) ),
							   new TypeInfo( 10, typeof( EnslavedGoblinKeeper ) ) ),
				new LevelInfo( new TypeInfo( 5, typeof( EnslavedGoblinMage ) ),
							   new TypeInfo( 5, typeof( EnslavedGoblinAlchemist ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( GrayGoblinMageRenowned ) ),
							   new TypeInfo( 1, typeof( GreenGoblinAlchemistRenowned ) ) )
			),
			new MiniChampInfo( /* Lava Caldera */
				typeof( EssenceOfPassion ),
				new LevelInfo( new TypeInfo( 20, typeof( FireAnt ) ),
							   new TypeInfo( 10, typeof( LavaSnake ) ),
							   new TypeInfo( 10, typeof( LavaLizard ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( LavaSerpent ) ),
							   new TypeInfo( 10, typeof( HellHound ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( LavaElemental ) ),
							   new TypeInfo( 5, typeof( FireDaemon ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( FireDaemonRenowned ) ) )
			),
			new MiniChampInfo ( /* Passage of Tears */
				typeof( EssenceOfSingularity ),
				new LevelInfo( new TypeInfo( 20, typeof( CorrosiveSlime ) ),
							   new TypeInfo( 10, typeof( AcidSlug ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( ToxicElemental ) ) ),
				new LevelInfo( new TypeInfo( 3, typeof( InterredGrizzle ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( AcidElementalRenowned ) ) )
			),
			new MiniChampInfo ( /* Cavern of the Discarded: Clan Chitter */
				typeof( EssenceOfBalance ),
				new LevelInfo( new TypeInfo( 10, typeof( ClanChitterAssistant ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( ClanChitterTinkerer ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( RakktaviRenowned ) ) )
			),
			new MiniChampInfo ( /* Cavern of the Discarded: Clan Ribbon */
				typeof( EssenceOfBalance ),
				new LevelInfo( new TypeInfo( 10, typeof( ClanRibbonSupplicant ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( ClanRibbonCourtier ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( VitaviRenowned ) ) )
			),
			new MiniChampInfo ( /* Cavern of the Discarded: Clan Scratch */
				typeof( EssenceOfBalance ),
				new LevelInfo( new TypeInfo( 10, typeof( ClanScratchScrounger ) ) ),
				new LevelInfo( new TypeInfo( 10, typeof( ClanScratchHenchrat ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( TikitaviRenowned ) ) )
			),
			new MiniChampInfo ( /* Minotaur Spawn */
				null,
				new LevelInfo( new TypeInfo( 20, typeof( Minotaur ) ) ),
				new LevelInfo( new TypeInfo( 20, typeof( MinotaurScout ) ) ),
				new LevelInfo( new TypeInfo( 15, typeof( MinotaurCaptain ) ) ),
				new LevelInfo( new TypeInfo( 15, typeof( MinotaurCaptain ) ) ),
				new LevelInfo( new TypeInfo( 1, typeof( Meraktus ) ) )
			)
		};

		public static MiniChampInfo GetInfo( MiniChampType type )
		{
			int v = (int) type;

			if ( v < 0 || v >= m_Table.Length )
				v = 0;

			return m_Table[v];
		}
	}
}