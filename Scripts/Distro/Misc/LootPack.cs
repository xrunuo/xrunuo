using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.PartySystem;

namespace Server
{
	public class LootPack
	{
		public static int GetLuckChance( Mobile killer, Mobile victim )
		{
			int luck = killer.Luck;

			PlayerMobile pmKiller = killer as PlayerMobile;

			if ( pmKiller != null && pmKiller.SentHonorContext != null && pmKiller.SentHonorContext.Target == victim )
				luck += pmKiller.SentHonorContext.PerfectionLuckBonus;

			if ( luck < 0 )
				return 0;

			return (int) ( Math.Pow( luck, 1 / 1.8 ) * 100 );
		}

		public static int GetLuckChanceForKiller( Mobile dead )
		{
			List<DamageStore> list = BaseCreature.GetLootingRights( dead.DamageEntries, dead.HitsMax );

			DamageStore highest = null;

			for ( int i = 0; i < list.Count; ++i )
			{
				DamageStore ds = list[i];

				if ( ds.HasRight && ( highest == null || ds.Damage > highest.Damage ) )
					highest = ds;
			}

			if ( highest == null )
				return 0;

			return GetLuckChance( highest.Mobile, dead );
		}

		public static bool CheckLuck( int chance )
		{
			return ( chance > Utility.Random( 10000 ) );
		}

		private LootPackEntry[] m_Entries;

		public LootPack( LootPackEntry[] entries )
		{
			m_Entries = entries;
		}

		public void Generate( Mobile from, Container cont, bool spawning, int luckChance )
		{
			if ( cont == null )
				return;

			bool checkLuck = true;

			for ( int i = 0; i < m_Entries.Length; ++i )
			{
				LootPackEntry entry = m_Entries[i];

				bool shouldAdd = ( entry.Chance > Utility.Random( 10000 ) );

				if ( !shouldAdd && checkLuck )
				{
					checkLuck = false;

					if ( LootPack.CheckLuck( luckChance ) )
						shouldAdd = ( entry.Chance > Utility.Random( 10000 ) );
				}

				if ( !shouldAdd )
					continue;

				Item item = entry.Construct( from, luckChance, spawning );

				if ( item != null )
				{
					if ( !item.Stackable || !cont.TryDropItem( from, item, false ) )
						cont.DropItem( item );
				}
			}
		}

		public static readonly LootPackItem[] Gold = new LootPackItem[]
			{
				new LootPackItem( typeof( Gold ), 1 )
			};

		public static readonly LootPackItem[] Instruments = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseInstrument ), 1 )
			};

		public static readonly LootPackItem[] LowScrollItems = new LootPackItem[]
			{
				new LootPackItem( typeof( ClumsyScroll ), 1 )
			};

		public static readonly LootPackItem[] MedScrollItems = new LootPackItem[]
			{
				new LootPackItem( typeof( ArchCureScroll ), 1 )
			};

		public static readonly LootPackItem[] HighScrollItems = new LootPackItem[]
			{
				new LootPackItem( typeof( SummonAirElementalScroll ), 1 )
			};

		public static readonly LootPackItem[] GemItems = new LootPackItem[]
			{
				new LootPackItem( typeof( Amber ), 1 )
			};

		public static readonly LootPackItem[] RareGemItems = new LootPackItem[]
			{
				new LootPackItem( typeof( BlueDiamond ), 1 )
			};

		public static readonly LootPackItem[] PotionItems = new LootPackItem[]
			{
				new LootPackItem( typeof( AgilityPotion ), 1 ),
				new LootPackItem( typeof( StrengthPotion ), 1 ),
				new LootPackItem( typeof( RefreshPotion ), 1 ),
				new LootPackItem( typeof( LesserCurePotion ), 1 ),
				new LootPackItem( typeof( LesserHealPotion ), 1 ),
				new LootPackItem( typeof( LesserPoisonPotion ), 1 )
			};

		public static readonly LootPackItem[] TalismanItems = new LootPackItem[]
			{
				new LootPackItem( typeof( RandomTalisman ), 1 )
			};

		public static readonly LootPackItem[] PeerlessIngredientsItems = new LootPackItem[]
			{
				new LootPackItem( typeof( Muculent ), 1 ),
				new LootPackItem( typeof( Blight ), 1 ),
				new LootPackItem( typeof( Corruption ), 1 ),
				new LootPackItem( typeof( Scourge ), 1 ),
				new LootPackItem( typeof( Taint ), 1 ),
				new LootPackItem( typeof( Putrefaction ), 1 )
			};

		public static readonly LootPackItem[] CavernIngredientsItems = new LootPackItem[]
			{
				new LootPackItem( typeof( AbyssalCloth ), 1 ),
				new LootPackItem( typeof( PowderedIron ), 1 ),
				new LootPackItem( typeof( CrushedGlass ), 1 ),
				new LootPackItem( typeof( SeedOfRenewal ), 1 ),
				new LootPackItem( typeof( CrystallineBlackrock ), 1 ),
				new LootPackItem( typeof( CrystalShards), 1 ),
				new LootPackItem( typeof( ElvenFletching), 1 ),
				new LootPackItem( typeof( ArcanicRuneStone ), 1 ),
				new LootPackItem( typeof( DelicateScales ), 1 )
			};

		#region AOS Magic Items
		public static readonly LootPackItem[] AosMagicItemsPoor = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 3 ),
				new LootPackItem( typeof( BaseRanged ), 1 ),
				new LootPackItem( typeof( BaseArmor ), 4 ),
				new LootPackItem( typeof( BaseShield ), 1 ),
				new LootPackItem( typeof( BaseJewel ), 2 )
			};

		public static readonly LootPackItem[] AosMagicItemsMeagerType1 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 56 ),
				new LootPackItem( typeof( BaseRanged ), 14 ),
				new LootPackItem( typeof( BaseArmor ), 81 ),
				new LootPackItem( typeof( BaseShield ), 11 ),
				new LootPackItem( typeof( BaseJewel ), 42 )
			};

		public static readonly LootPackItem[] AosMagicItemsMeagerType2 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 28 ),
				new LootPackItem( typeof( BaseRanged ), 7 ),
				new LootPackItem( typeof( BaseArmor ), 40 ),
				new LootPackItem( typeof( BaseShield ), 5 ),
				new LootPackItem( typeof( BaseJewel ), 21 )
			};

		public static readonly LootPackItem[] AosMagicItemsAverageType1 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 90 ),
				new LootPackItem( typeof( BaseRanged ), 23 ),
				new LootPackItem( typeof( BaseArmor ), 130 ),
				new LootPackItem( typeof( BaseShield ), 17 ),
				new LootPackItem( typeof( BaseJewel ), 68 )
			};

		public static readonly LootPackItem[] AosMagicItemsAverageType2 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 54 ),
				new LootPackItem( typeof( BaseRanged ), 13 ),
				new LootPackItem( typeof( BaseArmor ), 77 ),
				new LootPackItem( typeof( BaseShield ), 10 ),
				new LootPackItem( typeof( BaseJewel ), 40 )
			};

		public static readonly LootPackItem[] AosMagicItemsRichType1 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 211 ),
				new LootPackItem( typeof( BaseRanged ), 53 ),
				new LootPackItem( typeof( BaseArmor ), 303 ),
				new LootPackItem( typeof( BaseShield ), 39 ),
				new LootPackItem( typeof( BaseJewel ), 158 )
			};

		public static readonly LootPackItem[] AosMagicItemsRichType2 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 170 ),
				new LootPackItem( typeof( BaseRanged ), 43 ),
				new LootPackItem( typeof( BaseArmor ), 245 ),
				new LootPackItem( typeof( BaseShield ), 32 ),
				new LootPackItem( typeof( BaseJewel ), 128 )
			};

		public static readonly LootPackItem[] AosMagicItemsFilthyRichType1 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 219 ),
				new LootPackItem( typeof( BaseRanged ), 55 ),
				new LootPackItem( typeof( BaseArmor ), 315 ),
				new LootPackItem( typeof( BaseShield ), 41 ),
				new LootPackItem( typeof( BaseJewel ), 164 )
			};

		public static readonly LootPackItem[] AosMagicItemsFilthyRichType2 = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 239 ),
				new LootPackItem( typeof( BaseRanged ), 60 ),
				new LootPackItem( typeof( BaseArmor ), 343 ),
				new LootPackItem( typeof( BaseShield ), 90 ),
				new LootPackItem( typeof( BaseJewel ), 45 )
			};

		public static readonly LootPackItem[] AosMagicItemsUltraRich = new LootPackItem[]
			{
				new LootPackItem( typeof( BaseWeapon ), 276 ),
				new LootPackItem( typeof( BaseRanged ), 69 ),
				new LootPackItem( typeof( BaseArmor ), 397 ),
				new LootPackItem( typeof( BaseShield ), 52 ),
				new LootPackItem( typeof( BaseJewel ), 207 )
			};
		#endregion

		#region AOS definitions
		public static readonly LootPack AosPoor = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry(  true, Gold,						100.00, "2d10+20" ),
				new LootPackEntry( false, AosMagicItemsPoor,		  1.00, 1, 5, 0, 100 ),
				new LootPackEntry( false, Instruments,				  0.02, 1 )
			} );

		public static readonly LootPack AosMeager = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry(  true, Gold,						100.00, "4d10+40" ),
				new LootPackEntry( false, AosMagicItemsMeagerType1,	 20.40, 1, 2, 0, 50 ),
				new LootPackEntry( false, AosMagicItemsMeagerType2,	 10.20, 1, 5, 0, 100 ),
				new LootPackEntry( false, Instruments,				  0.10, 1 )
			} );

		public static readonly LootPack AosAverage = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry(  true, Gold,						100.00, "8d10+100" ),
				new LootPackEntry( false, AosMagicItemsAverageType1, 32.80, 1, 3, 0, 50 ),
				new LootPackEntry( false, AosMagicItemsAverageType1, 32.80, 1, 4, 0, 75 ),
				new LootPackEntry( false, AosMagicItemsAverageType2, 19.50, 1, 5, 0, 100 ),
				new LootPackEntry( false, Instruments,				  0.40, 1 )
			} );

		public static readonly LootPack AosRich = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry(  true, Gold,						100.00, "4d50+450" ),
				new LootPackEntry( false, AosMagicItemsRichType1,	100.00, 1, 3, 0, 75 ),
				new LootPackEntry( false, AosMagicItemsRichType1,	 80.00, 1, 3, 0, 75 ),
				new LootPackEntry( false, AosMagicItemsRichType1,	 60.00, 1, 5, 0, 100 ),
				new LootPackEntry( false, Instruments,				  1.00, 1 )
			} );

		public static readonly LootPack AosFilthyRich = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry(  true, Gold,						   100.00, "3d100+400" ),
				new LootPackEntry( false, AosMagicItemsFilthyRichType1,	79.50, 1, 5, 0, 100 ),
				new LootPackEntry( false, AosMagicItemsFilthyRichType1,	79.50, 1, 5, 0, 100 ),
				new LootPackEntry( false, AosMagicItemsFilthyRichType2,	77.60, 1, 5, 25, 100 ),
				new LootPackEntry( false, Instruments,					 2.00, 1 )
			} );

		public static readonly LootPack AosUltraRich = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry(  true, Gold,						100.00, "6d100+600" ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 33, 100 ),
				new LootPackEntry( false, Instruments,				  2.00, 1 )
			} );

		public static readonly LootPack AosSuperBoss = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry(  true, Gold,						100.00, "10d100+800" ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 25, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 33, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 33, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 33, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 33, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 50, 100 ),
				new LootPackEntry( false, AosMagicItemsUltraRich,	100.00, 1, 5, 50, 100 ),
				new LootPackEntry( false, Instruments,				  2.00, 1 )
			} );
		#endregion

		#region Generic accessors
		public static LootPack Poor { get { return AosPoor; } }
		public static LootPack Meager { get { return AosMeager; } }
		public static LootPack Average { get { return AosAverage; } }
		public static LootPack Rich { get { return AosRich; } }
		public static LootPack FilthyRich { get { return AosFilthyRich; } }
		public static LootPack UltraRich { get { return AosUltraRich; } }
		public static LootPack SuperBoss { get { return AosSuperBoss; } }
		#endregion

		public static readonly LootPack LowScrolls = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, LowScrollItems,	100.00, 1 )
			} );

		public static readonly LootPack MedScrolls = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, MedScrollItems,	100.00, 1 )
			} );

		public static readonly LootPack HighScrolls = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, HighScrollItems,	100.00, 1 )
			} );

		public static readonly LootPack Gems = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, GemItems,			100.00, 1 )
			} );

		public static readonly LootPack RareGems = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, RareGemItems,		100.00, 1 )
			} );

		public static readonly LootPack Potions = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, PotionItems,		100.00, 1 )
			} );

		public static readonly LootPack Talismans = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, TalismanItems,	100.00, 1 )
			} );

		public static readonly LootPack PeerlessIngredients = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, PeerlessIngredientsItems,		100.00, 1 )
			} );

		public static readonly LootPack CavernIngredients = new LootPack( new LootPackEntry[]
			{
				new LootPackEntry( false, CavernIngredientsItems,	100.00, 1 )
			} );
	}

	public class LootPackEntry
	{
		private int m_Chance;
		private LootPackDice m_Quantity;

		private int m_MaxProps, m_MinIntensity, m_MaxIntensity;

		private bool m_AtSpawnTime;

		private LootPackItem[] m_Items;

		public int Chance
		{
			get { return m_Chance; }
			set { m_Chance = value; }
		}

		public LootPackDice Quantity
		{
			get { return m_Quantity; }
			set { m_Quantity = value; }
		}

		public int MaxProps
		{
			get { return m_MaxProps; }
			set { m_MaxProps = value; }
		}

		public int MinIntensity
		{
			get { return m_MinIntensity; }
			set { m_MinIntensity = value; }
		}

		public int MaxIntensity
		{
			get { return m_MaxIntensity; }
			set { m_MaxIntensity = value; }
		}

		public LootPackItem[] Items
		{
			get { return m_Items; }
			set { m_Items = value; }
		}

		private static bool IsInTokuno( Mobile m )
		{
			// Fan Dancer's Dojo & Yomotsu Mines
			if ( m.Map == Map.Malas && m.X < 250 && m.Y > 720 )
				return true;

			return ( m.Map == Map.Tokuno );
		}

		private static bool IsInMLDungeon( Mobile m )
		{
			// Felucca and Trammel
			if ( ( m.Map == Map.Felucca || m.Map == Map.Trammel ) && m.X > 6144 )
				return true;

			// Twisted Weald (Ilshenar)
			if ( m.Map == Map.Ilshenar && m.X > 2110 && m.Y > 1150 && m.X < 2247 && m.Y < 1292 )
				return true;

			// Labyrinth (Malas)
			if ( m.Map == Map.Malas && m.X > 256 && m.Y > 1792 && m.X < 510 && m.Y < 2046 )
				return true;

			// Bedlam (Malas)
			if ( m.Map == Map.Malas && m.X > 77 && m.Y > 1594 && m.X < 212 && m.Y < 1778 )
				return true;

			// The Citadel (Tokuno)
			if ( m.Map == Map.Malas && m.X > 0 && m.Y > 1792 && m.X < 256 && m.Y < 2048 )
				return true;

			return false;
		}

		private static bool IsInSADungeon( Mobile m )
		{
			if ( m.Region.IsPartOf( "Abyssal Inferno" ) || m.Region.IsPartOf( "Primeval Lich" ) )
				return true;

			return m.Map == Map.TerMur;
		}

		public Item Construct( Mobile from, int luckChance, bool spawning )
		{
			if ( m_AtSpawnTime != spawning )
				return null;

			int totalChance = 0;

			for ( int i = 0; i < m_Items.Length; ++i )
				totalChance += m_Items[i].Chance;

			int rnd = Utility.Random( totalChance );

			for ( int i = 0; i < m_Items.Length; ++i )
			{
				LootPackItem item = m_Items[i];

				if ( rnd < item.Chance )
					return Mutate( from, luckChance, item.Construct( IsInTokuno( from ), IsInMLDungeon( from ), IsInSADungeon( from ) ) );

				rnd -= item.Chance;
			}

			return null;
		}

		public Item Mutate( Mobile from, int luckChance, Item item )
		{
			if ( item != null )
			{
				if ( item is BaseWeapon && 1 > Utility.Random( 100 ) )
				{
					item.Delete();
					item = new FireHorn();
					return item;
				}

				if ( item is BaseWeapon || item is BaseArmor || item is BaseJewel || item is BaseHat )
				{
					int bonusProps = GetBonusProperties();

					if ( bonusProps < m_MaxProps && LootPack.CheckLuck( luckChance ) )
						++bonusProps;

					int props = 1 + bonusProps;

					// Make sure we're not spawning items with 6 properties.
					if ( props > m_MaxProps )
						props = m_MaxProps;

					if ( item is BaseWeapon )
						BaseRunicTool.ApplyAttributesTo( (BaseWeapon) item, false, luckChance, props, m_MinIntensity, m_MaxIntensity );
					else if ( item is BaseArmor )
						BaseRunicTool.ApplyAttributesTo( (BaseArmor) item, false, luckChance, props, m_MinIntensity, m_MaxIntensity );
					else if ( item is BaseJewel )
						BaseRunicTool.ApplyAttributesTo( (BaseJewel) item, false, luckChance, props, m_MinIntensity, m_MaxIntensity );
					else if ( item is BaseHat )
						BaseRunicTool.ApplyAttributesTo( (BaseHat) item, false, luckChance, props, m_MinIntensity, m_MaxIntensity );
				}
				else if ( item is BaseInstrument )
				{
					SlayerName slayer = SlayerName.None;

					slayer = BaseRunicTool.GetRandomSlayer();

					if ( slayer == SlayerName.None )
					{
						item.Delete();
						return null;
					}

					BaseInstrument instr = (BaseInstrument) item;

					instr.Slayer = slayer;
				}

				if ( item.Stackable )
					item.Amount = m_Quantity.Roll();
			}

			return item;
		}

		public LootPackEntry( bool atSpawnTime, LootPackItem[] items, double chance, string quantity )
			: this( atSpawnTime, items, chance, new LootPackDice( quantity ), 0, 0, 0 )
		{
		}

		public LootPackEntry( bool atSpawnTime, LootPackItem[] items, double chance, int quantity )
			: this( atSpawnTime, items, chance, new LootPackDice( 0, 0, quantity ), 0, 0, 0 )
		{
		}

		public LootPackEntry( bool atSpawnTime, LootPackItem[] items, double chance, string quantity, int maxProps, int minIntensity, int maxIntensity )
			: this( atSpawnTime, items, chance, new LootPackDice( quantity ), maxProps, minIntensity, maxIntensity )
		{
		}

		public LootPackEntry( bool atSpawnTime, LootPackItem[] items, double chance, int quantity, int maxProps, int minIntensity, int maxIntensity )
			: this( atSpawnTime, items, chance, new LootPackDice( 0, 0, quantity ), maxProps, minIntensity, maxIntensity )
		{
		}

		public LootPackEntry( bool atSpawnTime, LootPackItem[] items, double chance, LootPackDice quantity, int maxProps, int minIntensity, int maxIntensity )
		{
			m_AtSpawnTime = atSpawnTime;
			m_Items = items;
			m_Chance = (int) ( 100 * chance );
			m_Quantity = quantity;
			m_MaxProps = maxProps;
			m_MinIntensity = minIntensity;
			m_MaxIntensity = maxIntensity;
		}

		public int GetBonusProperties()
		{
			int p0 = 0, p1 = 0, p2 = 0, p3 = 0, p4 = 0, p5 = 0;

			switch ( m_MaxProps )
			{
				case 1: p0 = 3; p1 = 1; break;
				case 2: p0 = 6; p1 = 3; p2 = 1; break;
				case 3: p0 = 10; p1 = 6; p2 = 3; p3 = 1; break;
				case 4: p0 = 16; p1 = 12; p2 = 6; p3 = 5; p4 = 1; break;
				case 5: p0 = 30; p1 = 25; p2 = 20; p3 = 15; p4 = 9; p5 = 1; break;
			}

			int pc = p0 + p1 + p2 + p3 + p4 + p5;

			int rnd = Utility.Random( pc );

			if ( rnd < p5 )
				return 5;
			else
				rnd -= p5;

			if ( rnd < p4 )
				return 4;
			else
				rnd -= p4;

			if ( rnd < p3 )
				return 3;
			else
				rnd -= p3;

			if ( rnd < p2 )
				return 2;
			else
				rnd -= p2;

			if ( rnd < p1 )
				return 1;

			return 0;
		}
	}

	public class LootPackItem
	{
		private Type m_Type;
		private int m_Chance;

		public Type Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		public int Chance
		{
			get { return m_Chance; }
			set { m_Chance = value; }
		}

		private static Type[] m_BlankTypes = new Type[] { typeof( BlankScroll ) };
		private static Type[][] m_NecroTypes = new Type[][]
			{
				new Type[] // low
				{
					typeof( AnimateDeadScroll ),		typeof( BloodOathScroll ),		typeof( CorpseSkinScroll ),	typeof( CurseWeaponScroll ),
					typeof( EvilOmenScroll ),			typeof( HorrificBeastScroll ),	typeof( MindRotScroll ),	typeof( PainSpikeScroll ),
					typeof( SummonFamiliarScroll ),		typeof( WraithFormScroll )
				},
				new Type[] // med
				{
					typeof( LichFormScroll ),			typeof( PoisonStrikeScroll ),	typeof( StrangleScroll ),	typeof( WitherScroll )
				},

				new Type[] // high
				{
					typeof( VengefulSpiritScroll ),		typeof( VampiricEmbraceScroll ),
					typeof( ExorcismScroll )
				}
			};

		public static Item RandomScroll( int index, int minCircle, int maxCircle )
		{
			--minCircle;
			--maxCircle;

			int scrollCount = ( ( maxCircle - minCircle ) + 1 ) * 8;

			if ( index == 0 )
				scrollCount += m_BlankTypes.Length;

			scrollCount += m_NecroTypes[index].Length;

			int rnd = Utility.Random( scrollCount );

			if ( index == 0 && rnd < m_BlankTypes.Length )
				return Loot.Construct( m_BlankTypes );
			else if ( index == 0 )
				rnd -= m_BlankTypes.Length;

			if ( rnd < m_NecroTypes.Length )
				return Loot.Construct( m_NecroTypes[index] );
			else
				rnd -= m_NecroTypes[index].Length;

			return Loot.RandomScroll( minCircle * 8, ( maxCircle * 8 ) + 7, SpellbookType.Regular );
		}

		public Item Construct( bool inTokuno, bool inMLDungeon, bool inSADungeon )
		{
			try
			{
				Item item;

				if ( m_Type == typeof( BaseRanged ) )
					item = Loot.RandomRangedWeapon( inTokuno, inMLDungeon, inSADungeon );
				else if ( m_Type == typeof( BaseWeapon ) )
					item = Loot.RandomWeapon( inTokuno, inMLDungeon, inSADungeon );
				else if ( m_Type == typeof( BaseArmor ) )
					item = Loot.RandomArmorOrHat( inTokuno, inMLDungeon, inSADungeon );
				else if ( m_Type == typeof( BaseShield ) )
					item = Loot.RandomShield( inSADungeon );
				else if ( m_Type == typeof( BaseJewel ) )
					item = Loot.RandomJewelry( inSADungeon );
				else if ( m_Type == typeof( BaseInstrument ) )
					item = Loot.RandomInstrument();
				else if ( m_Type == typeof( Amber ) ) // gem
					item = Loot.RandomGem();
				else if ( m_Type == typeof( BlueDiamond ) ) // rare gem
					item = Loot.RandomRareGem();
				else if ( m_Type == typeof( ClumsyScroll ) ) // low scroll
					item = RandomScroll( 0, 1, 3 );
				else if ( m_Type == typeof( ArchCureScroll ) ) // med scroll
					item = RandomScroll( 1, 4, 7 );
				else if ( m_Type == typeof( SummonAirElementalScroll ) ) // high scroll
					item = RandomScroll( 2, 8, 8 );
				else
					item = Activator.CreateInstance( m_Type ) as Item;

				return item;
			}
			catch
			{
			}

			return null;
		}

		public LootPackItem( Type type, int chance )
		{
			m_Type = type;
			m_Chance = chance;
		}
	}

	public class LootPackDice
	{
		private int m_Count, m_Sides, m_Bonus;

		public int Count
		{
			get { return m_Count; }
			set { m_Count = value; }
		}

		public int Sides
		{
			get { return m_Sides; }
			set { m_Sides = value; }
		}

		public int Bonus
		{
			get { return m_Bonus; }
			set { m_Bonus = value; }
		}

		public int Roll()
		{
			int v = m_Bonus;

			for ( int i = 0; i < m_Count; ++i )
				v += Utility.Random( 1, m_Sides );

			return v;
		}

		public LootPackDice( string str )
		{
			int start = 0;
			int index = str.IndexOf( 'd', start );

			if ( index < start )
				return;

			m_Count = Utility.ToInt32( str.Substring( start, index - start ) );

			bool negative;

			start = index + 1;
			index = str.IndexOf( '+', start );

			if ( negative = ( index < start ) )
				index = str.IndexOf( '-', start );

			if ( index < start )
				index = str.Length;

			m_Sides = Utility.ToInt32( str.Substring( start, index - start ) );

			if ( index == str.Length )
				return;

			start = index + 1;
			index = str.Length;

			m_Bonus = Utility.ToInt32( str.Substring( start, index - start ) );

			if ( negative )
				m_Bonus *= -1;
		}

		public LootPackDice( int count, int sides, int bonus )
		{
			m_Count = count;
			m_Sides = sides;
			m_Bonus = bonus;
		}
	}
}