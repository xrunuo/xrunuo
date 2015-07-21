using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.CannedEvil
{
	public enum ChampionSpawnType
	{
		Abyss,
		Arachnid,
		ColdBlood,
		ForestLord,
		VerminHorde,
		UnholyTerror,
		SleepingDragon,
		Corrupt,
		Glade,
		Unliving,
		Pit,
	}

	public class ChampionSpawnInfo
	{
		private string m_Name;
		private Type m_Champion;
		private Type[][] m_SpawnTypes;
		private string[] m_LevelNames;

		public string Name { get { return m_Name; } }
		public Type Champion { get { return m_Champion; } }
		public Type[][] SpawnTypes { get { return m_SpawnTypes; } }
		public string[] LevelNames { get { return m_LevelNames; } }

		public ChampionSpawnInfo( string name, Type champion, string[] levelNames, Type[][] spawnTypes )
		{
			m_Name = name;
			m_Champion = champion;
			m_LevelNames = levelNames;
			m_SpawnTypes = spawnTypes;
		}

		public static ChampionSpawnInfo[] Table { get { return m_Table; } }

		private static readonly ChampionSpawnInfo[] m_Table = new ChampionSpawnInfo[]
			{
				new ChampionSpawnInfo( "Abyss", typeof( Semidar ), new string[]{ "Foe", "Assassin", "Conqueror" }, new Type[][]
				{																											// Abyss
					new Type[]{ typeof( StrongMongbat ), typeof( Imp ) },													// Level 1
					new Type[]{ typeof( Gargoyle ), typeof( Harpy ) },														// Level 2
					new Type[]{ typeof( FireGargoyle ), typeof( StoneGargoyle ) },											// Level 3
					new Type[]{ typeof( Daemon ), typeof( Succubus ) }														// Level 4
				} ),
				new ChampionSpawnInfo( "Arachnid", typeof( Mephitis ), new string[]{ "Bane", "Killer", "Vanquisher" }, new Type[][]
				{																											// Arachnid
					new Type[]{ typeof( Scorpion ), typeof( GiantSpider ) },												// Level 1
					new Type[]{ typeof( TerathanDrone ), typeof( TerathanWarrior ) },										// Level 2
					new Type[]{ typeof( DreadSpider ), typeof( TerathanMatriarch ) },										// Level 3
					new Type[]{ typeof( PoisonElemental ), typeof( TerathanAvenger ) }										// Level 4
				} ),
				new ChampionSpawnInfo( "Cold Blood", typeof( Rikktor ), new string[]{ "Blight", "Slayer", "Destroyer" }, new Type[][]
				{																											// Cold Blood
					new Type[]{ typeof( Lizardman ), typeof( Snake ) },														// Level 1
					new Type[]{ typeof( LavaLizard ), typeof( OphidianWarrior ) },											// Level 2
					new Type[]{ typeof( Drake ), typeof( OphidianArchmage ) },												// Level 3
					new Type[]{ typeof( Dragon ), typeof( OphidianKnight ) }												// Level 4
				} ),
				new ChampionSpawnInfo( "Forest Lord", typeof( LordOaks ), new string[]{ "Enemy", "Curse", "Slaughterer" }, new Type[][]
				{																											// Forest Lord
					new Type[]{ typeof( Pixie ), typeof( ShadowWisp ) },													// Level 1
					new Type[]{ typeof( Kirin ), typeof( Wisp ) },															// Level 2
					new Type[]{ typeof( Centaur ), typeof( Unicorn ) },														// Level 3
					new Type[]{ typeof( EtherealWarrior ), typeof( SerpentineDragon ) }										// Level 4
				} ),
				new ChampionSpawnInfo( "Vermin Horde", typeof( Barracoon ), new string[]{ "Adversary", "Subjugator", "Eradicator" }, new Type[][]
				{																											// Vermin Horde
					new Type[]{ typeof( GiantRat ), typeof( Slime ) },														// Level 1
					new Type[]{ typeof( DireWolf ), typeof( Ratman ) },														// Level 2
					new Type[]{ typeof( HellHound ), typeof( RatmanMage ) },												// Level 3
					new Type[]{ typeof( RatmanArcher ), typeof( SilverSerpent ) }											// Level 4
				} ),
				new ChampionSpawnInfo( "Unholy Terror", typeof( Neira ), new string[]{ "Scourge", "Punisher", "Nemesis" }, new Type[][]
				{																											// Unholy Terror
					new Type[]{ typeof( Bogle ), typeof( Ghoul ), typeof( Shade ), typeof( Spectre ), typeof( Wraith ) },	// Level 1
					new Type[]{ typeof( BoneMagi ), typeof( Mummy ), typeof( SkeletalMage ) },								// Level 2
					new Type[]{ typeof( BoneKnight ), typeof( Lich ), typeof( SkeletalKnight ) },							// Level 3
					new Type[]{ typeof( LichLord ), typeof( RottingCorpse ) }												// Level 4
				} ),
				new ChampionSpawnInfo( "Sleeping Dragon", typeof( Serado ), new string[]{ "Rival", "Challenger", "Antagonist" }, new Type[][]
				{																											// Sleeping Dragon
					new Type[]{ typeof( DeathWatchBeetleHatchling ), typeof( Lizardman ) },									// Level 1
					new Type[]{ typeof( DeathWatchBeetle ), typeof( Kappa ) },												// Level 2
					new Type[]{ typeof( LesserHiryu ), typeof( RevenantLion ) },											// Level 3
					new Type[]{ typeof( Hiryu ), typeof( Oni ) }															// Level 4
				} ),
				new ChampionSpawnInfo( "Corrupt", typeof( IlhenirTheStained ), new string[]{ "Cleanser", "Expunger", "Depurator" }, new Type[][]
				{																											// Corrupt
					new Type[]{ typeof( PlagueSpawn ), typeof( Bogling ) },													// Level 1
					new Type[]{ typeof( PlagueBeast ), typeof( BogThing ) },												// Level 2
					new Type[]{ typeof( PlagueBeastLord ), typeof( InterredGrizzle ) },										// Level 3
					new Type[]{ typeof( FetidEssence ), typeof( PestilentBandage ) }										// Level 4
				} ),
				new ChampionSpawnInfo( "Glade", typeof( TwauloOfTheGlade ), new string[]{ "Banisher", "Enforcer", "Eradicator" }, new Type[][]
				{																											// Glade
					new Type[]{ typeof( ShadowWisp ), typeof( Pixie ) },													// Level 1
					new Type[]{ typeof( Dryad ), typeof( Centaur ) },														// Level 2
					new Type[]{ typeof( CuSidhe ), typeof( Satyr ) },														// Level 3
					new Type[]{ typeof( RagingGrizzlyBear ), typeof( FeralTreefellow ) }									// Level 4
				} ),
				new ChampionSpawnInfo( "Unliving", typeof( PrimevalLich ), new string[]{ "Despair", "Curse", "Woe" }, new Type[][]
				{																											// Unliving
					new Type[]{ typeof( GoreFiend ), typeof( VampireBat ) },												// Level 1
					new Type[]{ typeof( FleshGolem ), typeof( DarkWisp ) },													// Level 2
					new Type[]{ typeof( UndeadGargoyle ), typeof( Wight ) },												// Level 3
					new Type[]{ typeof( SkeletalDrake ), typeof( DreamWraith ) }											// Level 4
				} ),
				new ChampionSpawnInfo( "Pit", typeof( AbyssalInfernal ), new string[]{ "Agony", "Torment", "Havoc" }, new Type[][]
				{																											// Pit
					new Type[]{ typeof( Familiar ), typeof( ChaosDaemon ) },												// Level 1
					new Type[]{ typeof( StoneHarpy ), typeof( ArcaneDaemon ) },												// Level 2
					new Type[]{ typeof( PitFiend ), typeof( Moloch ) },														// Level 3
					new Type[]{ typeof( Archdemon ), typeof( AbyssmalAbomination ) }										// Level 4
				} ),
			};

		public static ChampionSpawnInfo GetInfo( ChampionSpawnType type )
		{
			int v = (int) type;

			if ( v < 0 || v >= m_Table.Length )
				v = 0;

			return m_Table[v];
		}
	}
}