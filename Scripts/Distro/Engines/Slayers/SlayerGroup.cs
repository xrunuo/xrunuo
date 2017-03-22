using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class SlayerGroup
	{
		private static SlayerEntry[] m_TotalEntries;
		private static SlayerGroup[] m_Groups;

		public static SlayerEntry[] TotalEntries { get { return m_TotalEntries; } }

		public static SlayerGroup[] Groups { get { return m_Groups; } }

		public static SlayerEntry GetEntryByName( SlayerName name )
		{
			int v = (int) name;

			if ( v >= 0 && v < m_TotalEntries.Length )
			{
				return m_TotalEntries[v];
			}

			return null;
		}

		public static SlayerName GetLootSlayerType( Type type )
		{
			for ( int i = 0; i < m_Groups.Length; ++i )
			{
				SlayerGroup group = m_Groups[i];
				Type[] foundOn = group.FoundOn;

				bool inGroup = false;

				for ( int j = 0; foundOn != null && !inGroup && j < foundOn.Length; ++j )
					inGroup = ( foundOn[j] == type );

				if ( inGroup )
				{
					int index = Utility.Random( 1 + group.Entries.Length );

					if ( index == 0 )
						return group.Super.Name;

					return group.Entries[index - 1].Name;
				}
			}

			return SlayerName.Undead;
		}

		private static SlayerName[] m_SuperSlayers = new SlayerName[]
			{
				SlayerName.Demon,
				SlayerName.Undead,
				SlayerName.Repond,
				SlayerName.Arachnid,
				SlayerName.Reptile,
				SlayerName.Elemental,
				SlayerName.Fey
			};

		public static bool IsSuperSlayer( SlayerName slayer )
		{
			return m_SuperSlayers.Contains( slayer );
		}

		static SlayerGroup()
		{
			SlayerGroup humanoid = new SlayerGroup();
			SlayerGroup undead = new SlayerGroup();
			SlayerGroup elemental = new SlayerGroup();
			SlayerGroup abyss = new SlayerGroup();
			SlayerGroup arachnid = new SlayerGroup();
			SlayerGroup reptilian = new SlayerGroup();
			SlayerGroup fey = new SlayerGroup();

			#region Humanoid

			humanoid.Opposition = new SlayerGroup[]
				{
					undead
				};

			humanoid.FoundOn = new Type[]
				{
					typeof( BoneKnight ),	typeof( Lich ),
					typeof( LichLord )
				};

			#endregion

			#region Undead
			undead.Opposition = new SlayerGroup[]
				{
					humanoid
				};

			#endregion

			#region Fey
			fey.Opposition = new SlayerGroup[]
				{
					abyss
				};

			fey.Super = new SlayerEntry
				(
					SlayerName.Fey,

					typeof( Centaur ), typeof( EtherealWarrior ),
					typeof( Kirin ), typeof( LordOaks ),
					typeof( Pixie ), typeof( Silvani ),
					typeof( Treefellow ), typeof( Unicorn ),
					typeof( Wisp ),

					typeof( Changeling ), typeof( InsaneDryad ),
					typeof( CuSidhe ), typeof( Satyr ),
					typeof( Dryad ), typeof( CrystalLatticeSeeker ),
					typeof( LadyMelisande ), typeof( DreadHorn ),
					typeof( Travesty ), typeof( ShimmeringEffusion ),
					typeof( Guile ), typeof( Irk ),
					typeof( TwauloOfTheGlade ), typeof( FeralTreefellow ),

					typeof( DarkWisp ), typeof( PixieRenowned )
				);

			fey.Entries = new SlayerEntry[0];
			#endregion

			#region Elemental
			elemental.Opposition = new SlayerGroup[]
				{
					abyss
				};

			elemental.FoundOn = new Type[]
				{
					typeof( Balron ),			typeof( Putrefier ),
					typeof( Daemon ),			typeof( FireDaemonRenowned )
				};

			elemental.Super = new SlayerEntry
				(
					SlayerName.Elemental,

					typeof( ToxicElemental ), typeof( AgapiteElemental ),
					typeof( AirElemental ), typeof( SummonedAirElemental ),
					typeof( BloodElemental ), typeof( BronzeElemental ),
					typeof( CopperElemental ), typeof( DullCopperElemental ),
					typeof( EarthElemental ), typeof( SummonedEarthElemental ),
					typeof( Efreet ), typeof( SummonedFireElemental ),
					typeof( FireElemental ), typeof( GoldenElemental ),
					typeof( IceElemental ), typeof( CrystalElemental ),
					typeof( KazeKemono ), typeof( RaiJu ),
					typeof( PoisonElemental ), typeof( ShadowIronElemental ),
					typeof( SandVortex ), typeof( ValoriteElemental ),
					typeof( SnowElemental ), typeof( VeriteElemental ),
					typeof( WaterElemental ), typeof( SummonedWaterElemental ),
					typeof( LavaElemental ), typeof( FireElementalRenowned ),
					typeof( AcidElementalRenowned ), typeof( Mistral ),
					typeof( Flurry ), typeof( Tempest )

					/*
					 * TODO:
					 * - Blackrock Elemental
					 */
				);

			elemental.Entries = new SlayerEntry[]
				{
					new SlayerEntry
						(
							SlayerName.BloodElemental,

							typeof( BloodElemental )
						),
					new SlayerEntry
						(
							SlayerName.EarthElemental,

							typeof( AgapiteElemental ),			typeof( BronzeElemental ),
							typeof( CopperElemental ),			typeof( EarthElemental ),
							typeof( SummonedEarthElemental ),	typeof( GoldenElemental ),
							typeof( ShadowIronElemental ),		typeof( ValoriteElemental ),
							typeof( VeriteElemental )
						),
					new SlayerEntry
						(
							SlayerName.PoisonElemental,

							typeof( PoisonElemental )
						),
					new SlayerEntry
						(
							SlayerName.FireElemental,

							typeof( FireElemental ),			typeof( SummonedFireElemental ),
							typeof( FireElementalRenowned )
						),
					new SlayerEntry
						(
							SlayerName.SnowElemental,

							typeof( IceElemental ),				typeof( SnowElemental )
						),
					new SlayerEntry
						(
							SlayerName.AirElemental,

							typeof( AirElemental ),				typeof( SummonedAirElemental ),
							typeof( Mistral ),					typeof( Flurry ),
							typeof( Tempest )
						),
					new SlayerEntry
						(
							SlayerName.WaterElemental,

							typeof( WaterElemental ),			typeof( SummonedWaterElemental )
						)
				};

			#endregion

			#region Demon
			abyss.Opposition = new SlayerGroup[]
				{
					elemental,
					fey
				};

			abyss.FoundOn = new Type[]
				{
					typeof( BloodElemental )
				};

			abyss.Super = new SlayerEntry
				(
					SlayerName.Demon,

					typeof( AbysmalHorror ), typeof( ArcaneDaemon ),
					typeof( Balron ), typeof( BoneDemon ),
					typeof( ChaosDaemon ), typeof( Daemon ),
					typeof( SummonedDaemon ), typeof( DemonKnight ),
					typeof( Devourer ), typeof( Impaler ),
					typeof( Gibberling ), typeof( Ravager ),
					typeof( FanDancer ), typeof( Oni ),
					typeof( EnslavedGargoyle ), typeof( FireGargoyle ),
					typeof( Gargoyle ), typeof( GargoyleDestroyer ),
					typeof( GargoyleEnforcer ), typeof( HordeMinion ),
					typeof( IceFiend ), typeof( Imp ),
					typeof( Moloch ), typeof( Semidar ),
					typeof( StoneGargoyle ), typeof( Succubus ),
					typeof( PatchworkSkeleton ), typeof( TsukiWolf ),
					typeof( CrystalDaemon ), typeof( ChiefParoxysmus ),
					typeof( Putrefier ), typeof( Szavetra ),
					typeof( FireDaemon ), typeof( FireDaemonRenowned ),
					typeof( DevourerOfSoulsRenowned ), typeof( AbyssalInfernal ),
					typeof( Anzuanord ), typeof( Ballem ),
					typeof( Betballem ), typeof( SkeletalLich ),
					typeof( UsagralemBallem ), typeof( EffetePutridGargoyle ),
					typeof( EffeteUndeadGargoyle ), typeof( SlasherOfVeils ),
					typeof( PitFiend )
				);

			abyss.Entries = new SlayerEntry[]
				{
					new SlayerEntry
						(
							SlayerName.Gargoyle,

							typeof( EnslavedGargoyle ),		typeof( FireGargoyle ),
							typeof( Gargoyle ),				typeof( GargoyleDestroyer ),
							typeof( GargoyleEnforcer ),		typeof( StoneGargoyle ),
							typeof( EffetePutridGargoyle ),	typeof( EffeteUndeadGargoyle )
						)
				};
			#endregion

			#region Arachnid
			arachnid.Opposition = new SlayerGroup[]
				{
					reptilian
				};

			arachnid.FoundOn = new Type[]
				{
					typeof( AncientWyrm ),			typeof( Dragon ),
					typeof( OphidianMatriarch ),	typeof( ShadowWyrm )
				};

			arachnid.Super = new SlayerEntry
				(
					SlayerName.Arachnid,

					typeof( DreadSpider ), typeof( FrostSpider ),
					typeof( GiantBlackWidow ), typeof( GiantSpider ),
					typeof( Mephitis ), typeof( Scorpion ),
					typeof( TerathanAvenger ), typeof( TerathanDrone ),
					typeof( TerathanMatriarch ), typeof( TerathanWarrior ),

					typeof( LadySabrix ), typeof( LadyLissith ),
					typeof( Silk ), typeof( Malefic ),
					typeof( Virulent ), typeof( Miasma ),
					typeof( SpeckledScorpion ), typeof( NavreyNightEyes ),
					typeof( SentinelSpider ), typeof( TrapdoorSpider ),
					typeof( Anlorzen ), typeof( Anlorlem )
				);

			arachnid.Entries = new SlayerEntry[]
				{
					new SlayerEntry
						(
							SlayerName.Scorpion,

							typeof( Scorpion ),			typeof( Miasma ),
							typeof( SpeckledScorpion )
						),
					new SlayerEntry
						(
							SlayerName.Spider,

							typeof( DreadSpider ),		typeof( FrostSpider ),
							typeof( GiantBlackWidow ),	typeof( GiantSpider ),
							typeof( Mephitis ),

							typeof( LadySabrix ),		typeof( LadyLissith ),
							typeof( Silk ),				typeof( Malefic ),
							typeof( Virulent ),			typeof( NavreyNightEyes ),
							typeof( SentinelSpider ),	typeof( Anlorzen )
						),
					new SlayerEntry
						(
							SlayerName.Terathan,

							typeof( TerathanAvenger ),		typeof( TerathanDrone ),
							typeof( TerathanMatriarch ),	typeof( TerathanWarrior ),
							typeof( Anlorlem )
						)
				};
			#endregion

			#region Reptile
			reptilian.Opposition = new SlayerGroup[]
				{
					arachnid
				};

			reptilian.FoundOn = new Type[]
				{
					typeof( TerathanAvenger ),	typeof( TerathanMatriarch )
				};

			reptilian.Super = new SlayerEntry
				(
					SlayerName.Reptile,

					typeof( AncientWyrm ), typeof( DeepSeaSerpent ),
					typeof( Dragon ), typeof( GreaterDragon ),
					typeof( Drake ), typeof( IceSerpent ),
					typeof( GiantIceWorm ), typeof( GiantSerpent ),
					typeof( Hiryu ), typeof( LesserHiryu ),
					typeof( Serado ), typeof( Yamandon ),
					typeof( IceSnake ), typeof( JukaLord ),
					typeof( JukaMage ), typeof( JukaWarrior ),
					typeof( LavaSerpent ), typeof( LavaSnake ),
					typeof( Lizardman ), typeof( OphidianMage ),
					typeof( OphidianKnight ), typeof( OphidianWarrior ),
					typeof( OphidianArchmage ), typeof( OphidianMatriarch ),
					typeof( SeaSerpent ), typeof( SerpentineDragon ),
					typeof( ShadowWyrm ), typeof( SilverSerpent ),
					typeof( SkeletalDragon ), typeof( Snake ),
					typeof( SwampDragon ), typeof( WhiteWyrm ),
					typeof( Wyvern ),

					typeof( Hydra ), typeof( CrystalHydra ),
					typeof( Reptalon ), typeof( CrystalSeaSerpent ),
					typeof( Rend ), typeof( Abscess ),
					typeof( Coil ), typeof( Raptor ),
					typeof( FairyDragon ), typeof( Kepetch ),
					typeof( KepetchAmbusher ), typeof( Skree ),
					typeof( SkeletalDrake ), typeof( SkeletalDragonRenowned ),
					typeof( CoralSnake ), typeof( StygianDragon ),
					typeof( WyvernRenowned ), typeof( Rikktor ),
					typeof( Slith ), typeof( StoneSlith ),
					typeof( ToxicSlith ),

					typeof( ColdDrake ), typeof( Grim )
				);

			reptilian.Entries = new SlayerEntry[]
				{
					new SlayerEntry
						(
							SlayerName.Dragon,

							typeof( AncientWyrm ),		typeof( Dragon ),
							typeof( GreaterDragon ),	typeof( Drake ),
							typeof( Hiryu ),			typeof( LesserHiryu ),
							typeof( SerpentineDragon ), typeof( ShadowWyrm ),
							typeof( SkeletalDragon ),	typeof( SwampDragon ),
							typeof( WhiteWyrm ),		typeof( Wyvern ),
							typeof( Rend ),				typeof( Abscess ),
							typeof( Reptalon ),			typeof( FairyDragon ),
							typeof( SkeletalDrake ),	typeof( SkeletalDragonRenowned ),
							typeof( StygianDragon ),	typeof( WyvernRenowned ),
							typeof( Rikktor ),			typeof( ColdDrake ),
							typeof( Grim )
					),
					new SlayerEntry
						(
							SlayerName.Lizardman,

							typeof( Lizardman )
						),
					new SlayerEntry
						(
							SlayerName.Ophidian,

							typeof( OphidianMage ),		typeof( OphidianKnight ),
							typeof( OphidianWarrior ),	typeof( OphidianArchmage ),
							typeof( OphidianMatriarch )
						),
					new SlayerEntry
						(
							SlayerName.Snake,

							typeof( DeepSeaSerpent ),	typeof( IceSerpent ),
							typeof( GiantIceWorm ),		typeof( GiantSerpent ),
							typeof( IceSnake ),			typeof( LavaSerpent ),
							typeof( LavaSnake ),		typeof( SeaSerpent ),
							typeof( SilverSerpent ),	typeof( Snake ),

							typeof( Yamandon ),			typeof( Serado ),
							typeof( Coil ),				typeof( CoralSnake )
						)
				};
			#endregion

			m_Groups = new[]
				{
					humanoid, undead, elemental, abyss,
					arachnid, reptilian, fey
				};

			m_TotalEntries = CompileEntries( m_Groups );
		}

		private static SlayerEntry[] CompileEntries( IEnumerable<SlayerGroup> groups )
		{
			var entries = new SlayerEntry[28];

			foreach ( var group in groups )
			{
				group.Super.Group = group;

				entries[(int) group.Super.Name] = group.Super;

				foreach ( var entry in group.Entries )
				{
					entry.Group = group;
					entries[(int) entry.Name] = entry;
				}
			}

			return entries;
		}

		public SlayerGroup[] Opposition { get; set; }
		public SlayerEntry Super { get; set; }
		public SlayerEntry[] Entries { get; set; }
		public Type[] FoundOn { get; set; }

		public bool OppositionSuperSlays( Mobile m )
		{
			return Opposition.Any( t => t.Super.Slays( m ) );
		}
	}
}
