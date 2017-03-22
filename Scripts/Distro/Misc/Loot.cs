using System;
using Server;
using Server.Items;

namespace Server
{
	public class Loot
	{
		#region List Definitions

		private static Type[] m_SAWeaponTypes = new Type[]
			{
				typeof( Bloodblade ),			typeof( DiscMace ),				typeof( DreadSword ),
				typeof( DualPointedSpear ),		typeof( DualShortAxes ),		typeof( GargishAxe ),
				typeof( GargishBardiche ),		typeof( GargishBattleAxe ),		typeof( GargishBoneHarvester ),
				typeof( GargishButcherKnife ),	typeof( GargishCleaver ),		typeof( GargishDaisho ),
				typeof( GargishGnarledStaff ),	typeof( GargishKatana ),		typeof( GargishKryss ),
				typeof( GargishLance ),			typeof( GargishMaul ),			typeof( GargishPike ),
				typeof( GargishScythe ),		typeof( GargishTalwar ),		typeof( GargishTekagi ),
				typeof( GargishTessen ),		typeof( GargishWarFork ),		typeof( GargishWarHammer ),
				typeof( GlassStaff ),			typeof( GlassSword ),			typeof( SerpentstoneStaff ),
				typeof( Shortblade )
			};

		public static Type[] SAWeaponTypes { get { return m_SAWeaponTypes; } }

		private static Type[] m_MLWeaponTypes = new Type[]
			{
				typeof( AssassinSpike ),		typeof( DiamondMace ),			typeof( ElvenMachete ),
				typeof( ElvenSpellBlade ),		typeof( Leafblade ),			typeof( OrnateAxe ),
				typeof( RadiantScimitar ),		typeof( RuneBlade ),			typeof( WarCleaver ),
				typeof( WildStaff )
			};

		public static Type[] MLWeaponTypes { get { return m_MLWeaponTypes; } }

		private static Type[] m_SEWeaponTypes = new Type[]
			{
				typeof( Bokuto ),				typeof( Daisho ),				typeof( Kama ),
				typeof( Lajatang ),				typeof( NoDachi ),				typeof( Nunchaku ),
				typeof( Sai ),					typeof( Tekagi ),				typeof( Tessen ),
				typeof( Tetsubo ),				typeof( Wakizashi )
			};

		public static Type[] SEWeaponTypes { get { return m_SEWeaponTypes; } }

		private static Type[] m_WeaponTypes = new Type[]
			{
				typeof( Axe ),					typeof( BattleAxe ),			typeof( DoubleAxe ),
				typeof( ExecutionersAxe ),		typeof( Hatchet ),				typeof( LargeBattleAxe ),
				typeof( TwoHandedAxe ),			typeof( WarAxe ),				typeof( Club ),
				typeof( Mace ),					typeof( Maul ),					typeof( WarHammer ),
				typeof( WarMace ),				typeof( Bardiche ),				typeof( Halberd ),
				typeof( Spear ),				typeof( ShortSpear ),			typeof( Pitchfork ),
				typeof( WarFork ),				typeof( BlackStaff ),			typeof( GnarledStaff ),
				typeof( QuarterStaff ),			typeof( Broadsword ),			typeof( Cutlass ),
				typeof( Katana ),				typeof( Kryss ),				typeof( Longsword ),
				typeof( Scimitar ),				typeof( VikingSword ),			typeof( Pickaxe ),
				typeof( HammerPick ),			typeof( ButcherKnife ),			typeof( Cleaver ),
				typeof( Dagger ),				typeof( SkinningKnife ),		typeof( ShepherdsCrook ),
				typeof( Scythe ),				typeof( BoneHarvester ),		typeof( Scepter ),
				typeof( BladedStaff ),			typeof( Pike ),					typeof( DoubleBladedStaff ),
				typeof( Lance ),				typeof( CrescentBlade )
			};

		public static Type[] WeaponTypes { get { return m_WeaponTypes; } }

		private static Type[] m_SARangedWeaponTypes = new Type[]
			{
				typeof( Boomerang ),			typeof( Cyclone ),				typeof( SoulGlaive )
			};

		public static Type[] SARangedWeaponTypes { get { return m_SARangedWeaponTypes; } }

		private static Type[] m_MLRangedWeaponTypes = new Type[]
			{
				typeof( ElvenCompositeLongBow ),	typeof( MagicalShortbow )
			};

		public static Type[] MLRangedWeaponTypes { get { return m_MLRangedWeaponTypes; } }

		private static Type[] m_SERangedWeaponTypes = new Type[]
			{
				typeof( Yumi )
			};

		public static Type[] SERangedWeaponTypes { get { return m_SERangedWeaponTypes; } }

		private static Type[] m_RangedWeaponTypes = new Type[]
			{
				typeof( Bow ),					typeof( Crossbow ),				typeof( HeavyCrossbow ),
				typeof( CompositeBow ),			typeof( RepeatingCrossbow )
			};

		public static Type[] RangedWeaponTypes { get { return m_RangedWeaponTypes; } }

		private static Type[] m_SAArmorTypes = new Type[]
			{
				typeof( FemaleGargishLeatherArms ),		typeof( FemaleGargishLeatherChest ),
				typeof( FemaleGargishLeatherKilt ),		typeof( FemaleGargishLeatherLeggings ),
				typeof( GargishLeatherArms ),			typeof( GargishLeatherChest ),
				typeof( GargishLeatherKilt ),			typeof( GargishLeatherLeggings ),
				typeof( FemaleGargishPlatemailArms ),	typeof( FemaleGargishPlatemailChest ),
				typeof( FemaleGargishPlatemailKilt ),	typeof( FemaleGargishPlatemailLeggings ),
				typeof( GargishPlatemailArms ),			typeof( GargishPlatemailChest ),
				typeof( GargishPlatemailKilt ),			typeof( GargishPlatemailLeggings )
			};

		public static Type[] SAArmorTypes { get { return m_SAArmorTypes; } }

		private static Type[] m_MLArmorTypes = new Type[]
			{
				typeof( HideGloves ),			typeof( HideGorget ),			typeof( HidePants ),
				typeof( HidePauldrons ),		typeof( HideTunic ),			typeof( LeafArms ),
				typeof( LeafGloves ),			typeof( LeafGorget ),			typeof( LeafLeggings ),
				typeof( LeafTonlet ),			typeof( LeafTunic ),			typeof( WoodlandArms ),
				typeof( WoodlandChest ),		typeof( WoodlandGauntlets ),	typeof( WoodlandGorget ),
				typeof( WoodlandLeggings )
			};

		public static Type[] MLArmorTypes { get { return m_MLArmorTypes; } }

		private static Type[] m_SEArmorTypes = new Type[]
			{
				typeof( ChainHatsuburi ),		typeof( LeatherDo ),			typeof( LeatherHaidate ),
				typeof( LeatherHiroSode ),		typeof( LeatherJingasa ),		typeof( LeatherMempo ),
				typeof( LeatherNinjaHood ),		typeof( LeatherNinjaJacket ),	typeof( LeatherNinjaMitts ),
				typeof( LeatherNinjaPants ),	typeof( LeatherSuneate ),		typeof( DecorativePlateKabuto ),
				typeof( HeavyPlateJingasa ),	typeof( LightPlateJingasa ),	typeof( PlateBattleKabuto ),
				typeof( PlateDo ),				typeof( PlateHaidate ),			typeof( PlateHatsuburi ),
				typeof( PlateHiroSode ),		typeof( PlateMempo ),			typeof( PlateSuneate ),
				typeof( SmallPlateJingasa ),	typeof( StandardPlateKabuto ),	typeof( StuddedDo ),
				typeof( StuddedHaidate ),		typeof( StuddedHiroSode ),		typeof( StuddedMempo ),
				typeof( StuddedSuneate )
			};

		public static Type[] SEArmorTypes { get { return m_SEArmorTypes; } }

		private static Type[] m_ArmorTypes = new Type[]
			{
				typeof( BoneArms ),				typeof( BoneChest ),			typeof( BoneGloves ),
				typeof( BoneLegs ),				typeof( BoneHelm ),				typeof( ChainChest ),
				typeof( ChainLegs ),			typeof( ChainCoif ),			typeof( Bascinet ),
				typeof( CloseHelm ),			typeof( Helmet ),				typeof( NorseHelm ),
				typeof( OrcHelm ),				typeof( FemaleLeatherChest ),	typeof( LeatherArms ),
				typeof( LeatherBustierArms ),	typeof( LeatherChest ),			typeof( LeatherGloves ),
				typeof( LeatherGorget ),		typeof( LeatherLegs ),			typeof( LeatherShorts ),
				typeof( LeatherSkirt ),			typeof( LeatherCap ),			typeof( FemalePlateChest ),
				typeof( PlateArms ),			typeof( PlateChest ),			typeof( PlateGloves ),
				typeof( PlateGorget ),			typeof( PlateHelm ),			typeof( PlateLegs ),
				typeof( RingmailArms ),			typeof( RingmailChest ),		typeof( RingmailGloves ),
				typeof( RingmailLegs ),			typeof( FemaleStuddedChest ),	typeof( StuddedArms ),
				typeof( StuddedBustierArms ),	typeof( StuddedChest ),			typeof( StuddedGloves ),
				typeof( StuddedGorget ),		typeof( StuddedLegs )
			};

		public static Type[] ArmorTypes { get { return m_ArmorTypes; } }

		private static Type[] m_SAShieldTypes = new Type[]
			{
				typeof( GargishChaosShield ),	typeof( GargishKiteShield ),	typeof( GargishOrderShield ),
				typeof( GargishWoodenShield ),	typeof( LargePlateShield ),		typeof( MediumPlateShield ),
				typeof( SmallPlateShield ),
			};

		public static Type[] SAShieldTypes { get { return m_SAShieldTypes; } }

		private static Type[] m_ShieldTypes = new Type[]
			{
				typeof( BronzeShield ),			typeof( Buckler ),				typeof( HeaterShield ),
				typeof( MetalShield ),			typeof( MetalKiteShield ),		typeof( WoodenKiteShield ),
				typeof( WoodenShield ),			typeof( ChaosShield ),			typeof( OrderShield )
			};

		public static Type[] ShieldTypes { get { return m_ShieldTypes; } }

		private static Type[] m_GemTypes = new Type[]
			{
				typeof( Amber ),				typeof( Amethyst ),				typeof( Citrine ),
				typeof( Diamond ),				typeof( Emerald ),				typeof( Ruby ),
				typeof( Sapphire ),				typeof( StarSapphire ),			typeof( Tourmaline )
			};

		public static Type[] GemTypes { get { return m_GemTypes; } }

		private static Type[] m_RareGemTypes = new Type[]
			{
				typeof( BlueDiamond ),			typeof( BrilliantAmber ),		typeof( DarkSapphire ),
				typeof( EcruCitrine ),			typeof( FireRuby ),				typeof( PerfectEmerald ),
				typeof( Turquoise ),			typeof( WhitePearl )
			};

		public static Type[] RareGemTypes { get { return m_RareGemTypes; } }

		private static Type[] m_SAJewelryTypes = new Type[]
			{
				typeof( GargishRing ),			typeof( GargishNecklace ),
				typeof( GargishEarrings ),		typeof( GargishBracelet )
			};

		public static Type[] SAJewelryTypes { get { return m_SAJewelryTypes; } }

		private static Type[] m_JewelryTypes = new Type[]
			{
				typeof( GoldRing ),				typeof( GoldBracelet ),
				typeof( SilverRing ),			typeof( SilverBracelet )
			};

		public static Type[] JewelryTypes { get { return m_JewelryTypes; } }

		private static Type[] m_RegTypes = new Type[]
			{
				typeof( BlackPearl ),			typeof( Bloodmoss ),			typeof( Garlic ),
				typeof( Ginseng ),				typeof( MandrakeRoot ),			typeof( Nightshade ),
				typeof( SulfurousAsh ),			typeof( SpidersSilk )
			};

		public static Type[] RegTypes { get { return m_RegTypes; } }

		private static Type[] m_NecroRegTypes = new Type[]
			{
				typeof( BatWing ),				typeof( GraveDust ),			typeof( DaemonBlood ),
				typeof( NoxCrystal ),			typeof( PigIron )
			};

		public static Type[] NecroRegTypes { get { return m_NecroRegTypes; } }

		private static Type[] m_MysticRegTypes = new Type[]
			{
				typeof( BlackPearl ),			typeof( Bloodmoss ),			typeof( Garlic ),
				typeof( Ginseng ),				typeof( MandrakeRoot ),			typeof( Nightshade ),
				typeof( SulfurousAsh ),			typeof( SpidersSilk ),			typeof( Bone ),
				typeof( FertileDirt ),			typeof( DragonsBlood ),			typeof( DaemonBone )
			};

		public static Type[] MysticRegTypes { get { return m_MysticRegTypes; } }

		private static Type[] m_PotionTypes = new Type[]
			{
				typeof( AgilityPotion ),		typeof( StrengthPotion ),		typeof( RefreshPotion ),
				typeof( LesserCurePotion ),		typeof( LesserHealPotion ),		typeof( LesserPoisonPotion )
			};

		public static Type[] PotionTypes { get { return m_PotionTypes; } }

		private static Type[] m_SEInstrumentTypes = new Type[]
			{
				typeof( BambooFlute )
			};

		public static Type[] SEInstrumentTypes { get { return m_SEInstrumentTypes; } }

		private static Type[] m_InstrumentTypes = new Type[]
			{
				typeof( Drums ),				typeof( Harp ),					typeof( LapHarp ),
				typeof( Lute ),					typeof( Tambourine ),			typeof( TambourineTassel )
			};

		public static Type[] InstrumentTypes { get { return m_InstrumentTypes; } }

		private static Type[] m_StatueTypes = new Type[]
			{
				typeof( StatueSouth ),			typeof( StatueSouth2 ),			typeof( StatueNorth ),
				typeof( StatueWest ),			typeof( StatueEast ),			typeof( StatueEast2 ),
				typeof( StatueSouthEast ),		typeof( BustSouth ),			typeof( BustEast )
			};

		public static Type[] StatueTypes { get { return m_StatueTypes; } }

		#region Spell Scrolls
		private static Type[] m_RegularScrollTypes = new Type[]
			{
				typeof( ClumsyScroll ),			typeof( CreateFoodScroll ),		typeof( FeeblemindScroll ),		typeof( HealScroll ),
				typeof( MagicArrowScroll ),		typeof( NightSightScroll ),		typeof( ReactiveArmorScroll ),	typeof( WeakenScroll ),
				typeof( AgilityScroll ),		typeof( CunningScroll ),		typeof( CureScroll ),			typeof( HarmScroll ),
				typeof( MagicTrapScroll ),		typeof( MagicUnTrapScroll ),	typeof( ProtectionScroll ),		typeof( StrengthScroll ),
				typeof( BlessScroll ),			typeof( FireballScroll ),		typeof( MagicLockScroll ),		typeof( PoisonScroll ),
				typeof( TelekinisisScroll ),	typeof( TeleportScroll ),		typeof( UnlockScroll ),			typeof( WallOfStoneScroll ),
				typeof( ArchCureScroll ),		typeof( ArchProtectionScroll ),	typeof( CurseScroll ),			typeof( FireFieldScroll ),
				typeof( GreaterHealScroll ),	typeof( LightningScroll ),		typeof( ManaDrainScroll ),		typeof( RecallScroll ),
				typeof( BladeSpiritsScroll ),	typeof( DispelFieldScroll ),	typeof( IncognitoScroll ),		typeof( MagicReflectScroll ),
				typeof( MindBlastScroll ),		typeof( ParalyzeScroll ),		typeof( PoisonFieldScroll ),	typeof( SummonCreatureScroll ),
				typeof( DispelScroll ),			typeof( EnergyBoltScroll ),		typeof( ExplosionScroll ),		typeof( InvisibilityScroll ),
				typeof( MarkScroll ),			typeof( MassCurseScroll ),		typeof( ParalyzeFieldScroll ),	typeof( RevealScroll ),
				typeof( ChainLightningScroll ), typeof( EnergyFieldScroll ),	typeof( FlamestrikeScroll ),	typeof( GateTravelScroll ),
				typeof( ManaVampireScroll ),	typeof( MassDispelScroll ),		typeof( MeteorSwarmScroll ),	typeof( PolymorphScroll ),
				typeof( EarthquakeScroll ),		typeof( EnergyVortexScroll ),	typeof( ResurrectionScroll ),	typeof( SummonAirElementalScroll ),
				typeof( SummonDaemonScroll ),	typeof( SummonEarthElementalScroll ),	typeof( SummonFireElementalScroll ),	typeof( SummonWaterElementalScroll )
			};

		private static Type[] m_NecromancyScrollTypes = new Type[]
			{
				typeof( AnimateDeadScroll ),		typeof( BloodOathScroll ),		typeof( CorpseSkinScroll ),	typeof( CurseWeaponScroll ),
				typeof( EvilOmenScroll ),			typeof( HorrificBeastScroll ),	typeof( LichFormScroll ),	typeof( MindRotScroll ),
				typeof( PainSpikeScroll ),			typeof( PoisonStrikeScroll ),	typeof( StrangleScroll ),	typeof( SummonFamiliarScroll ),
				typeof( VampiricEmbraceScroll ),	typeof( VengefulSpiritScroll ),	typeof( WitherScroll ),		typeof( WraithFormScroll ),
				typeof( ExorcismScroll )
			};

		private static Type[] m_PaladinScrollTypes = new Type[0];

		private static Type[] m_ArcanistScrollTypes = new Type[]
			{
				typeof( ArcaneEmpowermentScroll ),	typeof( AttunementScroll ),
				typeof( DryadAllureScroll ),		typeof( EssenceOfWindScroll ),
				typeof( EtherealVoyageScroll ),		typeof( GiftOfLifeScroll ),
				typeof( GiftOfRenewalScroll ),		typeof( ImmolatingWeaponScroll ),
				typeof( NaturesFuryScroll ),		typeof( ReaperFormScroll ),
				typeof( ThunderstormScroll ),		typeof( WildfireScroll ),
				typeof( WordOfDeathScroll )
			};

		private static Type[] m_MysticScrollTypes = new Type[]
			{
				typeof( NetherBoltScroll ),			typeof( HealingStoneScroll ),
				typeof( PurgeMagicScroll ),			typeof( EnchantScroll ),
				typeof( SleepScroll ),				typeof( EagleStrikeScroll ),
				typeof( AnimatedWeaponScroll ),		typeof( StoneFormScroll ),
				typeof( SpellTriggerScroll ),		typeof( MassSleepScroll ),
				typeof( CleansingWindsScroll ),		typeof( BombardScroll ),
				typeof( SpellPlagueScroll ),		typeof( HailStormScroll ),
				typeof( NetherCycloneScroll ),		typeof( RisingColossusScroll )
			};

		public static Type[] RegularScrollTypes { get { return m_RegularScrollTypes; } }
		public static Type[] NecromancyScrollTypes { get { return m_NecromancyScrollTypes; } }
		public static Type[] PaladinScrollTypes { get { return m_PaladinScrollTypes; } }
		public static Type[] ArcanistScrollTypes { get { return m_ArcanistScrollTypes; } }
		public static Type[] MysticScrollTypes { get { return m_MysticScrollTypes; } }
		#endregion

		private static Type[] m_GrimmochJournalTypes = new Type[]
		{
			typeof( GrimmochJournal1 ),		typeof( GrimmochJournal2 ),		typeof( GrimmochJournal3 ),
			typeof( GrimmochJournal6 ),		typeof( GrimmochJournal7 ),		typeof( GrimmochJournal11 ),
			typeof( GrimmochJournal14 ),	typeof( GrimmochJournal17 ),	typeof( GrimmochJournal23 )
		};

		public static Type[] GrimmochJournalTypes { get { return m_GrimmochJournalTypes; } }

		private static Type[] m_LysanderNotebookTypes = new Type[]
		{
			typeof( LysanderNotebook1 ),		typeof( LysanderNotebook2 ),		typeof( LysanderNotebook3 ),
			typeof( LysanderNotebook7 ),		typeof( LysanderNotebook8 ),		typeof( LysanderNotebook11 )
		};

		public static Type[] LysanderNotebookTypes { get { return m_LysanderNotebookTypes; } }

		private static Type[] m_TavarasJournalTypes = new Type[]
		{
			typeof( TavarasJournal1 ),		typeof( TavarasJournal2 ),		typeof( TavarasJournal3 ),
			typeof( TavarasJournal6 ),		typeof( TavarasJournal7 ),		typeof( TavarasJournal8 ),
			typeof( TavarasJournal9 ),		typeof( TavarasJournal11 ),		typeof( TavarasJournal14 ),
			typeof( TavarasJournal16 ),		typeof( TavarasJournal16b ),	typeof( TavarasJournal17 ),
			typeof( TavarasJournal19 )
		};

		public static Type[] TavarasJournalTypes { get { return m_TavarasJournalTypes; } }

		private static Type[] m_WandTypes = new Type[]
			{
				typeof( ClumsyWand ),               typeof( FeebleWand ),           typeof( FireballWand ),
				typeof( GreaterHealWand ),          typeof( HarmWand ),             typeof( HealWand ),
				typeof( IDWand ),                   typeof( LightningWand ),        typeof( MagicArrowWand ),
				typeof( ManaDrainWand ),            typeof( WeaknessWand )

			};
		public static Type[] WandTypes { get { return m_WandTypes; } }

		private static Type[] m_MLClothingTypes = new Type[]
			{
				typeof( ElvenBoots ),			typeof( ElvenPants ),			typeof( ElvenRobe ),
				typeof( ElvenShirt ),			typeof( ElvenShirtDark ),		typeof( FemaleElvenRobe ),
				typeof( WoodlandBelt )
			};

		public static Type[] MLClothingTypes { get { return m_MLClothingTypes; } }

		private static Type[] m_SEClothingTypes = new Type[]
			{
				typeof( ClothNinjaJacket ),		typeof( FemaleKimono ),			typeof( Hakama ),
				typeof( HakamaShita ),			typeof( JinBaori ),				typeof( Kamishimo ),
				typeof( MaleKimono ),			typeof( NinjaTabi ),			typeof( Obi ),
				typeof( SamuraiTabi ),			typeof( TattsukeHakama ),		typeof( Waraji )
			};

		public static Type[] SEClothingTypes { get { return m_SEClothingTypes; } }

		private static Type[] m_ClothingTypes = new Type[]
			{
				typeof( Cloak ),
				typeof( Bonnet ),               typeof( Cap ),		            typeof( FeatheredHat ),
				typeof( FloppyHat ),            typeof( JesterHat ),			typeof( Surcoat ),
				typeof( SkullCap ),             typeof( StrawHat ),	            typeof( TallStrawHat ),
				typeof( TricorneHat ),			typeof( WideBrimHat ),          typeof( WizardsHat ),
				typeof( BodySash ),             typeof( Doublet ),              typeof( Boots ),
				typeof( FullApron ),            typeof( JesterSuit ),           typeof( Sandals ),
				typeof( Tunic ),				typeof( Shoes ),				typeof( Shirt ),
				typeof( Kilt ),                 typeof( Skirt ),				typeof( FancyShirt ),
				typeof( FancyDress ),			typeof( ThighBoots ),			typeof( LongPants ),
				typeof( PlainDress ),           typeof( Robe ),					typeof( ShortPants ),
				typeof( HalfApron ),			typeof( FurSarong ),			typeof( FurCape ),
				typeof( FlowerGarland ),		typeof( GildedDress ),			typeof( FurBoots ),
				typeof( FormalShirt ),
			};

		public static Type[] ClothingTypes { get { return m_ClothingTypes; } }

		private static Type[] m_MLHatTypes = new Type[]
			{
				typeof( Circlet ),				typeof( GemmedCirclet ),		typeof( RoyalCirclet ),
				typeof( RavenHelm ),			typeof( VultureHelm ),			typeof( WingedHelm )
			};

		public static Type[] MLHatTypes { get { return m_MLHatTypes; } }

		private static Type[] m_SEHatTypes = new Type[]
			{
				typeof( ClothNinjaHood ),		typeof( Kasa )
			};

		public static Type[] SEHatTypes { get { return m_SEHatTypes; } }

		private static Type[] m_HatTypes = new Type[]
			{
				typeof( SkullCap ),			typeof( Bandana ),		typeof( FloppyHat ),
				typeof( Cap ),				typeof( WideBrimHat ),	typeof( StrawHat ),
				typeof( TallStrawHat ),		typeof( WizardsHat ),	typeof( Bonnet ),
				typeof( FeatheredHat ),		typeof( TricorneHat ),	typeof( JesterHat ),
				typeof( FlowerGarland ),	typeof( BearMask ),		typeof( DeerMask )
			};

		public static Type[] HatTypes { get { return m_HatTypes; } }

		private static Type[] m_LibraryBookTypes = new Type[]
			{
				typeof( GrammarOfOrcish ),		typeof( CallToAnarchy ),				typeof( ArmsAndWeaponsPrimer ),
				typeof( SongOfSamlethe ),		typeof( TaleOfThreeTribes ),			typeof( GuideToGuilds ),
				typeof( BirdsOfBritannia ),		typeof( BritannianFlora ),				typeof( ChildrenTalesVol2 ),
				typeof( TalesOfVesperVol1 ),	typeof( DeceitDungeonOfHorror ),		typeof( DimensionalTravel ),
				typeof( EthicalHedonism ),		typeof( MyStory ),						typeof( DiversityOfOurLand ),
				typeof( QuestOfVirtues ),		typeof( RegardingLlamas ),				typeof( TalkingToWisps ),
				typeof( TamingDragons ),		typeof( BoldStranger ),					typeof( BurningOfTrinsic ),
				typeof( TheFight ),				typeof( LifeOfATravellingMinstrel ),	typeof( MajorTradeAssociation ),
				typeof( RankingsOfTrades ),		typeof( WildGirlOfTheForest ),			typeof( TreatiseOnAlchemy ),
				typeof( VirtueBook )
			};

		public static Type[] LibraryBookTypes { get { return m_LibraryBookTypes; } }

		private static Type[] m_EssenceTypes = new Type[]
			{
				typeof( EssenceOfBalance ),		typeof( EssenceOfAchievement ),		typeof( EssenceOfPassion ),
				typeof( EssenceOfPrecision ),	typeof( EssenceOfDirection ),		typeof( EssenceOfDiligence ),
				typeof( EssenceOfFeeling ),		typeof( EssenceOfControl ),			typeof( EssenceOfSingularity ),
				typeof( EssenceOfOrder ),		typeof( EssenceOfPersistence ),
			};

		public static Type[] EssenceTypes { get { return m_EssenceTypes; } }

		#endregion

		#region Accessors

		public static BaseWand RandomWand()
		{
			return Construct( m_WandTypes ) as BaseWand;
		}

		public static BaseClothing RandomClothing()
		{
			return RandomClothing( false );
		}

		public static BaseClothing RandomClothing( bool inTokuno )
		{
			return RandomClothing( inTokuno, false );
		}

		public static BaseClothing RandomClothing( bool inTokuno, bool inMLDungeon )
		{
			if ( inMLDungeon )
			{
				if ( inTokuno )
					return Construct( m_MLClothingTypes, m_SEClothingTypes, m_ClothingTypes ) as BaseClothing;
				else
					return Construct( m_MLClothingTypes, m_ClothingTypes ) as BaseClothing;
			}

			if ( inTokuno )
				return Construct( m_SEClothingTypes, m_ClothingTypes ) as BaseClothing;

			return Construct( m_ClothingTypes ) as BaseClothing;
		}

		public static BaseWeapon RandomRangedWeapon()
		{
			return RandomRangedWeapon( false );
		}

		public static BaseWeapon RandomRangedWeapon( bool inTokuno )
		{
			return RandomRangedWeapon( inTokuno, false );
		}

		public static BaseWeapon RandomRangedWeapon( bool inTokuno, bool inMLDungeon )
		{
			return RandomRangedWeapon( inTokuno, inMLDungeon, false );
		}

		public static BaseWeapon RandomRangedWeapon( bool inTokuno, bool inMLDungeon, bool inSADungeon )
		{
			if ( inSADungeon )
				return Construct( m_SARangedWeaponTypes, m_RangedWeaponTypes ) as BaseWeapon;

			if ( inMLDungeon )
			{
				if ( inTokuno )
					return Construct( m_MLRangedWeaponTypes, m_SERangedWeaponTypes, m_RangedWeaponTypes ) as BaseWeapon;
				else
					return Construct( m_MLRangedWeaponTypes, m_RangedWeaponTypes ) as BaseWeapon;
			}

			if ( inTokuno )
				return Construct( m_SERangedWeaponTypes, m_RangedWeaponTypes ) as BaseWeapon;

			return Construct( m_RangedWeaponTypes ) as BaseWeapon;
		}

		public static BaseWeapon RandomWeapon()
		{
			return RandomWeapon( false );
		}

		public static BaseWeapon RandomWeapon( bool inTokuno )
		{
			return RandomWeapon( inTokuno, false );
		}

		public static BaseWeapon RandomWeapon( bool inTokuno, bool inMLDungeon )
		{
			return RandomWeapon( inTokuno, inMLDungeon, false );
		}

		public static BaseWeapon RandomWeapon( bool inTokuno, bool inMLDungeon, bool inSADungeon )
		{
			if ( inSADungeon )
				return Construct( m_SAWeaponTypes, m_WeaponTypes ) as BaseWeapon;

			if ( inMLDungeon )
			{
				if ( inTokuno )
					return Construct( m_MLWeaponTypes, m_SEWeaponTypes, m_WeaponTypes ) as BaseWeapon;
				else
					return Construct( m_MLWeaponTypes, m_WeaponTypes ) as BaseWeapon;
			}

			if ( inTokuno )
				return Construct( m_SEWeaponTypes, m_WeaponTypes ) as BaseWeapon;

			return Construct( m_WeaponTypes ) as BaseWeapon;
		}

		public static Item RandomWeaponOrJewelry()
		{
			return RandomWeaponOrJewelry( false );
		}

		public static Item RandomWeaponOrJewelry( bool inTokuno )
		{
			if ( inTokuno )
				return Construct( m_SEWeaponTypes, m_WeaponTypes, m_JewelryTypes );

			return Construct( m_WeaponTypes, m_JewelryTypes );
		}

		public static BaseJewel RandomJewelry()
		{
			return RandomJewelry( false );
		}

		public static BaseJewel RandomJewelry( bool inSADungeon )
		{
			if ( inSADungeon )
				return Construct( m_SAJewelryTypes, m_JewelryTypes ) as BaseJewel;

			return Construct( m_JewelryTypes ) as BaseJewel;
		}

		public static BaseArmor RandomArmor()
		{
			return RandomArmor( false );
		}

		public static BaseArmor RandomArmor( bool inTokuno )
		{
			return RandomArmor( inTokuno, false );
		}

		public static BaseArmor RandomArmor( bool inTokuno, bool inMLDungeon )
		{
			return RandomArmor( inTokuno, inMLDungeon, false );
		}

		public static BaseArmor RandomArmor( bool inTokuno, bool inMLDungeon, bool inSADungeon )
		{
			if ( inSADungeon )
				return Construct( m_SAArmorTypes, m_ArmorTypes ) as BaseArmor;

			if ( inMLDungeon )
				if ( inTokuno )
					return Construct( m_MLArmorTypes, m_SEArmorTypes, m_ArmorTypes ) as BaseArmor;
				else
					return Construct( m_MLArmorTypes, m_ArmorTypes ) as BaseArmor;

			if ( inTokuno )
				return Construct( m_SEArmorTypes, m_ArmorTypes ) as BaseArmor;

			return Construct( m_ArmorTypes ) as BaseArmor;
		}

		public static BaseHat RandomHat()
		{
			return RandomHat( false );
		}

		public static BaseHat RandomHat( bool inTokuno )
		{
			if ( inTokuno )
				return Construct( m_SEHatTypes, m_HatTypes ) as BaseHat;

			return Construct( m_HatTypes ) as BaseHat;
		}

		public static Item RandomArmorOrHat( bool inTokuno, bool inMLDungeon, bool inSADungeon )
		{
			if ( inSADungeon )
				return Construct( m_SAArmorTypes, m_ArmorTypes, m_HatTypes );

			if ( inMLDungeon )
			{
				if ( inTokuno )
					return Construct( m_MLArmorTypes, m_ArmorTypes, m_SEHatTypes, m_HatTypes );
				else
					return Construct( m_MLArmorTypes, m_ArmorTypes, m_HatTypes );
			}

			if ( inTokuno )
				return Construct( m_SEArmorTypes, m_ArmorTypes, m_SEHatTypes, m_HatTypes );

			return Construct( m_ArmorTypes, m_HatTypes );
		}

		public static BaseShield RandomShield()
		{
			return RandomShield( false );
		}

		public static BaseShield RandomShield( bool inSADungeon )
		{
			if ( inSADungeon )
				return Construct( m_SAShieldTypes, m_ShieldTypes ) as BaseShield;

			return Construct( m_ShieldTypes ) as BaseShield;
		}

		public static BaseArmor RandomArmorOrShield()
		{
			return RandomArmorOrShield( false, false );
		}

		public static BaseArmor RandomArmorOrShield( bool inTokuno, bool inSADungeon )
		{
			if ( inSADungeon )
				return Construct( m_SAArmorTypes, m_SAShieldTypes, m_ArmorTypes, m_ShieldTypes ) as BaseArmor;

			if ( inTokuno )
				return Construct( m_SEArmorTypes, m_ArmorTypes, m_ShieldTypes ) as BaseArmor;

			return Construct( m_ArmorTypes, m_ShieldTypes ) as BaseArmor;
		}

		public static Item RandomArmorOrShieldOrJewelry()
		{
			return RandomArmorOrShieldOrJewelry( false );
		}

		public static Item RandomArmorOrShieldOrJewelry( bool inTokuno )
		{
			if ( inTokuno )
				return Construct( m_SEArmorTypes, m_ArmorTypes, m_SEHatTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );

			return Construct( m_ArmorTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );
		}

		public static Item RandomArmorOrShieldOrWeapon()
		{
			return RandomArmorOrShieldOrWeapon( false );
		}

		public static Item RandomArmorOrShieldOrWeapon( bool inTokuno )
		{
			if ( inTokuno )
				return Construct( m_SEWeaponTypes, m_WeaponTypes, m_SERangedWeaponTypes, m_RangedWeaponTypes, m_SEArmorTypes, m_ArmorTypes, m_SEHatTypes, m_HatTypes, m_ShieldTypes );

			return Construct( m_WeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_HatTypes, m_ShieldTypes );
		}

		public static Item RandomArmorOrShieldOrWeaponOrJewelry()
		{
			return RandomArmorOrShieldOrWeaponOrJewelry( false );
		}

		public static Item RandomArmorOrShieldOrWeaponOrJewelry( bool inTokuno )
		{
			return RandomArmorOrShieldOrWeaponOrJewelry( inTokuno, false );
		}

		public static Item RandomArmorOrShieldOrWeaponOrJewelry( bool inTokuno, bool inMLDungeon )
		{
			if ( inMLDungeon )
			{
				if ( inTokuno )
					return Construct( m_MLWeaponTypes, m_SEWeaponTypes, m_WeaponTypes, m_MLRangedWeaponTypes, m_SERangedWeaponTypes, m_RangedWeaponTypes, m_MLArmorTypes, m_ArmorTypes, m_MLHatTypes, m_SEHatTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );
				else
					return Construct( m_MLWeaponTypes, m_WeaponTypes, m_MLRangedWeaponTypes, m_RangedWeaponTypes, m_MLArmorTypes, m_ArmorTypes, m_MLHatTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );
			}

			if ( inTokuno )
				return Construct( m_SEWeaponTypes, m_WeaponTypes, m_SERangedWeaponTypes, m_RangedWeaponTypes, m_SEArmorTypes, m_ArmorTypes, m_SEHatTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );

			return Construct( m_WeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );
		}

		public static Item RandomEssence()
		{
			return Construct( m_EssenceTypes );
		}

		public static Item RandomGem()
		{
			return Construct( m_GemTypes );
		}

		public static Item RandomRareGem()
		{
			return Construct( m_RareGemTypes );
		}

		public static Item RandomReagent()
		{
			return Construct( m_RegTypes );
		}

		public static Item RandomNecromancyReagent()
		{
			return Construct( m_NecroRegTypes );
		}

		public static Item RandomMysticismReagent()
		{
			return Construct( m_MysticRegTypes );
		}

		public static Item RandomPossibleReagent()
		{
			return Construct( m_RegTypes, m_NecroRegTypes, m_MysticRegTypes );
		}

		public static Item RandomPotion()
		{
			return Construct( m_PotionTypes );
		}

		public static BaseInstrument RandomInstrument()
		{
			return Construct( m_InstrumentTypes, m_SEInstrumentTypes ) as BaseInstrument;
		}

		public static Item RandomStatue()
		{
			return Construct( m_StatueTypes );
		}

		public static SpellScroll RandomScroll( int minIndex, int maxIndex, SpellbookType type )
		{
			Type[] types;

			switch ( type )
			{
				default:
				case SpellbookType.Regular: types = m_RegularScrollTypes; break;
				case SpellbookType.Necromancer: types = m_NecromancyScrollTypes; break;
				case SpellbookType.Paladin: types = m_PaladinScrollTypes; break;
			}

			return Construct( types, Utility.RandomMinMax( minIndex, maxIndex ) ) as SpellScroll;
		}

		public static BaseBook RandomGrimmochJournal()
		{
			return Construct( m_GrimmochJournalTypes ) as BaseBook;
		}

		public static BaseBook RandomLysanderNotebook()
		{
			return Construct( m_LysanderNotebookTypes ) as BaseBook;
		}

		public static BaseBook RandomTavarasJournal()
		{
			return Construct( m_TavarasJournalTypes ) as BaseBook;
		}

		public static BaseBook RandomLibraryBook()
		{
			return Construct( m_LibraryBookTypes ) as BaseBook;
		}
		#endregion

		#region Construction methods
		public static Item Construct( Type type )
		{
			try
			{
				return Activator.CreateInstance( type ) as Item;
			}
			catch
			{
				return null;
			}
		}

		public static Item Construct( Type[] types )
		{
			if ( types.Length > 0 )
				return Construct( types, Utility.Random( types.Length ) );

			return null;
		}

		public static Item Construct( Type[] types, int index )
		{
			if ( index >= 0 && index < types.Length )
				return Construct( types[index] );

			return null;
		}

		public static Item Construct( params Type[][] types )
		{
			int totalLength = 0;

			for ( int i = 0; i < types.Length; ++i )
				totalLength += types[i].Length;

			if ( totalLength > 0 )
			{
				int index = Utility.Random( totalLength );

				for ( int i = 0; i < types.Length; ++i )
				{
					if ( index >= 0 && index < types[i].Length )
						return Construct( types[i][index] );

					index -= types[i].Length;
				}
			}

			return null;
		}
		#endregion
	}
}
