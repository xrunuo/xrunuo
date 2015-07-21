using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public class SearchCriteriaCategory
	{
		private int m_Cliloc;
		private SearchCriterionEntry[] m_Criteria;

		public int Cliloc { get { return m_Cliloc; } }
		public SearchCriterionEntry[] Criteria { get { return m_Criteria; } }

		public SearchCriteriaCategory( int cliloc, SearchCriterionEntry[] criteria )
		{
			m_Cliloc = cliloc;
			m_Criteria = criteria;
		}

		private static SearchCriteriaCategory[] m_AllCategories = new SearchCriteriaCategory[]
			{
				new SearchCriteriaCategory( 1154647, new SearchCriterionEntry[] { // Miscellaneous
					new SearchCriterionEntry( 1154646, new ExcludeFeluccaSearchCriterion() ), // Exclude items on Felucca
					new SearchCriterionEntry( 1154648, new GargoylesOnlySearchCriterion() ), // Gargoyles Only
					new SearchCriterionEntry( 1154650, new ElvesOnlySearchCriterion() ), // Elves Only
					new SearchCriterionEntry( 1154661, new FactionItemSearchCriterion() ), // Faction Item
					new SearchCriterionEntry( 1154682, new PromotionalTokenSearchCriterion() ), // Promotional Token
					new SearchCriterionEntry( 1015168, new NightSightSearchCriterion() ), // Night Sight
					new SearchCriterionEntry( 1116639, new CursedSearchCriterion() ), // Cursed
					new SearchCriterionEntry( 1151826, new CannotBeRepairedSearchCriterion() ), // Cannot Be Repaired
					new SearchCriterionEntry( 1116209, new BrittleSearchCriterion() ), // Brittle
					new SearchCriterionEntry( 1075624, new EnhancePotionsSearchCriterion() ), // Enhance Potions
					new SearchCriterionEntry( 1079757, new LowerRequirementsSearchCriterion() ), // Lower Requirements
					new SearchCriterionEntry( 1061153, new LuckSearchCriterion() ), // Luck
					new SearchCriterionEntry( 1075626, new ReflectPhysicalDamageSearchCriterion() ), // Reflect Physical Damage
					new SearchCriterionEntry( 1079709, new SelfRepairSearchCriterion() ), // Self Repair
					new SearchCriterionEntry( 1154693, new ArtifactRaritySearchCriterion() ), // Artifact Rarity
				}),
				new SearchCriteriaCategory( 1154531, new SearchCriterionEntry[] { // Equipment
					new SearchCriterionEntry( 1154602, new ShoesSearchCriterion() ), // Footwear/Talons
					new SearchCriterionEntry( 1154603, new PantsSearchCriterion() ), // Pants/Leg Armor
					new SearchCriterionEntry( 1154604, new ShirtSearchCriterion() ), // Shirt
					new SearchCriterionEntry( 1154605, new HelmSearchCriterion() ), // Hat/Head Armor
					new SearchCriterionEntry( 1154606, new GlovesSearchCriterion() ), // Hand/Kilt Armor
					new SearchCriterionEntry( 1154607, new RingSearchCriterion() ), // Ring
					new SearchCriterionEntry( 1154608, new TalismanSearchCriterion() ), // Talisman
					new SearchCriterionEntry( 1154609, new NeckSearchCriterion() ), // Necklace/Neck Armor
					new SearchCriterionEntry( 1154611, new WaistSearchCriterion() ), // Apron/Belt
					new SearchCriterionEntry( 1154612, new InnerTorsoSearchCriterion() ), // Chest Armor
					new SearchCriterionEntry( 1154613, new BraceletSearchCriterion() ), // Bracelet
					new SearchCriterionEntry( 1154616, new MiddleTorsoSearchCriterion() ), // Surcoat/Tunic/Sash
					new SearchCriterionEntry( 1154617, new EarringsSearchCriterion() ), // Earrings/Gargish Glasses
					new SearchCriterionEntry( 1154618, new ArmsSearchCriterion() ), // Arm Armor
					new SearchCriterionEntry( 1154619, new CloakSearchCriterion() ), // Cloak/Quiver/Wing Armor
					new SearchCriterionEntry( 1154621, new OuterTorsoSearchCriterion() ), // Dress/Robe
					new SearchCriterionEntry( 1154622, new OuterLegsSearchCriterion() ) // Skirt
				}),
				new SearchCriteriaCategory( 1154541, new SearchCriterionEntry[] { // Combat
					new SearchCriterionEntry( 1079760, new DamageIncreaseSearchCriterion() ), // Damage Increase
					new SearchCriterionEntry( 1075620, new DefenseIncreaseChanceSearchCriterion() ), // Defense Chance Increase
					new SearchCriterionEntry( 1075616, new HitChanceIncreaseSearchCriterion() ), // Hit Chance Increase
					new SearchCriterionEntry( 1075629, new SwingSpeedIncreaseSearchCriterion() ), // Swing Speed Increase
					new SearchCriterionEntry( 1116536, new SoulChargeSearchCriterion() ), // Soul Charge
					new SearchCriterionEntry( 1079592, new UseBestWeaponSkillSearchCriterion() ), // Use Best Weapon Skill
					new SearchCriterionEntry( 1154660, new ReactiveParalyzeSearchCriterion() ), // Reactive Paralyze
					//new SearchCriterionEntry( 1152206, new AssassinHonedSearchCriterion() ), // Assassin Honed
					//new SearchCriterionEntry( 1151183, new SearingWeaponSearchCriterion() ), // Searing Weapon
					new SearchCriterionEntry( 1113591, new BloodDrinkerSearchCriterion() ), // Blood Drinker
					new SearchCriterionEntry( 1113710, new BattleLustSearchCriterion() ), // Battle Lust
					new SearchCriterionEntry( 1072792, new BalancedSearchCriterion() ), // Balanced
					//new SearchCriterionEntry( 1150018, new FocusSearchCriterion() ), // Focus
					new SearchCriterionEntry( 1154662, new FireEaterSearchCriterion() ), // Fire Eater
					new SearchCriterionEntry( 1154663, new ColdEaterSearchCriterion() ), // Cold Eater
					new SearchCriterionEntry( 1154664, new PoisonEaterSearchCriterion() ), // Poison Eater
					new SearchCriterionEntry( 1154665, new EnergyEaterSearchCriterion() ), // Energy Eater
					new SearchCriterionEntry( 1154666, new KineticEaterSearchCriterion() ), // Kinetic Eater
					new SearchCriterionEntry( 1154667, new DamageEaterSearchCriterion() ) // Damage Eater
				}),
				new SearchCriteriaCategory( 1154538, new SearchCriterionEntry[] { // Casting
					new SearchCriterionEntry( 1154655, new FireResonanceSearchCriterion() ), // Fire Resonance
					new SearchCriterionEntry( 1154656, new ColdResonanceSearchCriterion() ), // Cold Resonance
					new SearchCriterionEntry( 1154657, new PoisonResonanceSearchCriterion() ), // Poison Resonance
					new SearchCriterionEntry( 1154658, new EnergyResonanceSearchCriterion() ), // Energy Resonance
					new SearchCriterionEntry( 1154659, new KineticResonanceSearchCriterion() ), // Kinetic Resonance
					new SearchCriterionEntry( 1075628, new SpellDamageIncreaseSearchCriterion() ), // Spell Damage Increase
					new SearchCriterionEntry( 1116535, new CastingFocusSearchCriterion() ), // Casting Focus
					new SearchCriterionEntry( 1075618, new FasterCastRecoverySearchCriterion() ), // Faster Cast Recovery
					new SearchCriterionEntry( 1075617, new FasterCastingSearchCriterion() ), // Faster Casting
					new SearchCriterionEntry( 1075621, new LowerManaCostSearchCriterion() ), // Lower Mana Cost
					new SearchCriterionEntry( 1075625, new LowerReagentCostSearchCriterion() ), // Lower Reagent Cost
					new SearchCriterionEntry( 1079759, new MageWeaponSearchCriterion() ), // Mage Weapon
					new SearchCriterionEntry( 1079758, new MageArmorSearchCriterion() ), // Mage Armor
					new SearchCriterionEntry( 1079766, new SpellChannelingCostSearchCriterion() ) // Spell Channeling
				}),
				new SearchCriteriaCategory( 1154535, new SearchCriterionEntry[] { // Damage Type
					new SearchCriterionEntry( 1151800, new PhysicalDamageSearchCriterion() ), // Physical Damage
					new SearchCriterionEntry( 1151801, new FireDamageSearchCriterion() ), // Fire Damage
					new SearchCriterionEntry( 1151802, new ColdDamageSearchCriterion() ), // Cold Damage
					new SearchCriterionEntry( 1151803, new PoisonDamageSearchCriterion() ), // Poison Damage
					new SearchCriterionEntry( 1151804, new EnergyDamageSearchCriterion() ) // Energy Damage
				}),
				new SearchCriteriaCategory( 1154536, new SearchCriterionEntry[] { // Hit Spell
					new SearchCriterionEntry( 1079702, new HitDispelSearchCriterion() ), // Hit Dispel
					new SearchCriterionEntry( 1079703, new HitFireballSearchCriterion() ), // Hit Fireball
					new SearchCriterionEntry( 1079704, new HitHarmSearchCriterion() ), // Hit Harm
					new SearchCriterionEntry( 1154673, new HitCurseSearchCriterion() ), // Hit Curse
					new SearchCriterionEntry( 1079698, new HitLifeLeechSearchCriterion() ), // Hit Life Leech
					new SearchCriterionEntry( 1079705, new HitLightningSearchCriterion() ), // Hit Lightning
					new SearchCriterionEntry( 1080416, new VelocitySearchCriterion() ), // Velocity
					new SearchCriterionEntry( 1079699, new HitLowerAttackSearchCriterion() ), // Hit Lower Attack
					new SearchCriterionEntry( 1079700, new HitLowerDefenseSearchCriterion() ), // Hit Lower Defense
					new SearchCriterionEntry( 1079706, new HitMagicArrowSearchCriterion() ), // Hit Magic Arrow
					new SearchCriterionEntry( 1079701, new HitManaLeechSearchCriterion() ), // Hit Mana Leech
					new SearchCriterionEntry( 1079707, new HitStaminaLeechSearchCriterion() ), // Hit Stamina Leech
					new SearchCriterionEntry( 1154668, new HitFatigueSearchCriterion() ), // Hit Fatigue
					new SearchCriterionEntry( 1154669, new HitManaDrainSearchCriterion() ), // Hit Mana Drain
					new SearchCriterionEntry( 1154670, new SplinteringWeaponSearchCriterion() ), // Splintering Weapon
					//new SearchCriterionEntry( 1154671, new BaneSearchCriterion() ) // Bane
				}),
				new SearchCriteriaCategory( 1154537, new SearchCriterionEntry[] { // Hit Area
					new SearchCriterionEntry( 1079693, new HitColdAreaSearchCriterion() ), // Hit Cold Area
					new SearchCriterionEntry( 1079694, new HitEnergyAreaSearchCriterion() ), // Hit Energy Area
					new SearchCriterionEntry( 1079695, new HitFireAreaSearchCriterion() ), // Hit Fire Area
					new SearchCriterionEntry( 1079696, new HitPhysicalAreaSearchCriterion() ), // Hit Physical Area
					new SearchCriterionEntry( 1079697, new HitPoisonAreaSearchCriterion() ) // Hit Poison Area
				}),
				new SearchCriteriaCategory( 1154539, new SearchCriterionEntry[] { // Resists
					new SearchCriterionEntry( 1079761, new ColdResistSearchCriterion() ), // Cold Resist
					new SearchCriterionEntry( 1079762, new EnergyResistSearchCriterion() ), // Energy Resist
					new SearchCriterionEntry( 1079763, new FireResistSearchCriterion() ), // Fire Resist
					new SearchCriterionEntry( 1079764, new PhysicalResistSearchCriterion() ), // Physical Resist
					new SearchCriterionEntry( 1079765, new PoisonResistSearchCriterion() ) // Poison Resist
				}),
				new SearchCriteriaCategory( 1154540, new SearchCriterionEntry[] { // Stats
					new SearchCriterionEntry( 1079767, new StrengthBonusSearchCriterion() ), // Strength Bonus
					new SearchCriterionEntry( 1079732, new DexterityBonusSearchCriterion() ), // Dexterity Bonus
					new SearchCriterionEntry( 1079756, new IntelligenceBonusSearchCriterion() ), // Intelligence Bonus
					new SearchCriterionEntry( 1079404, new HitPointsIncreaseSearchCriterion() ), // Hit Points Increase
					new SearchCriterionEntry( 1079405, new StaminaIncreaseSearchCriterion() ), // Stamina Increase
					new SearchCriterionEntry( 1079406, new ManaIncreaseSearchCriterion() ), // Mana Increase
					new SearchCriterionEntry( 1075627, new HitPointRegenerationSearchCriterion() ), // Hit Point Regeneration
					new SearchCriterionEntry( 1079411, new StaminaRegerationSearchCriterion() ), // Stamina Regeneration
					new SearchCriterionEntry( 1079410, new ManaRegenerationSearchCriterion() ) // Mana Regeneration
				}),
				new SearchCriteriaCategory( 1154683, new SearchCriterionEntry[] { // Arachnid/Reptile Slayers
					new SearchCriterionEntry( 1079751, new ReptileSlayerSearchCriterion() ), // Reptile Slayer
					new SearchCriterionEntry( 1061284, new DragonSlayerSearchCriterion() ), // Dragon Slayer
					new SearchCriterionEntry( 1079738, new LizardmanSlayerSearchCriterion() ), // Lizardman Slayer
					new SearchCriterionEntry( 1079740, new OphidianSlayerSearchCriterion() ), // Ophidian Slayer
					new SearchCriterionEntry( 1079744, new SnakeSlayerSearchCriterion() ), // Snake Slayer
					new SearchCriterionEntry( 1079747, new ArachnidSlayerSearchCriterion() ), // Arachnid Slayer
					new SearchCriterionEntry( 1079743, new ScorpionSlayerSearchCriterion() ), // Scorpion Slayer
					new SearchCriterionEntry( 1079746, new SpiderSlayerSearchCriterion() ), // Spider Slayer
					new SearchCriterionEntry( 1079753, new TerathanSlayerSearchCriterion() ) // Terathan Slayer
				}),				
				new SearchCriteriaCategory( 1154684, new SearchCriterionEntry[] { // Repond/Undead Slayers
					new SearchCriterionEntry( 1079750, new RepondSlayerSearchCriterion() ), // Repond Slayer
					new SearchCriterionEntry( 1072506, new BatSlayerSearchCriterion() ), // Bat Slayer
					new SearchCriterionEntry( 1072504, new BearSlayerSearchCriterion() ), // Bear Slayer
					new SearchCriterionEntry( 1072508, new BeetleSlayerSearchCriterion() ), // Beetle Slayer
					new SearchCriterionEntry( 1072509, new BirdSlayerSearchCriterion() ), // Bird Slayer
					new SearchCriterionEntry( 1072512, new BovineSlayerSearchCriterion() ), // Bovine Slayer
					new SearchCriterionEntry( 1072511, new FlameSlayerSearchCriterion() ), // Flame Slayer
					new SearchCriterionEntry( 1095010, new GoblinSlayerSearchCriterion() ), // Goblin Slayer
					new SearchCriterionEntry( 1072510, new IceSlayerSearchCriterion() ), // Ice Slayer
					new SearchCriterionEntry( 1072507, new MageSlayerSearchCriterion() ), // Mage Slayer
					new SearchCriterionEntry( 1079739, new OgreSlayerSearchCriterion() ), // Ogre Slayer
					new SearchCriterionEntry( 1079741, new OrcSlayerSearchCriterion() ), // Orc Slayer
					new SearchCriterionEntry( 1079754, new TrollSlayerSearchCriterion() ), // Troll Slayer
					new SearchCriterionEntry( 1072505, new VerminSlayerSearchCriterion() ), // Vermin Slayer
					new SearchCriterionEntry( 1079752, new UndeadSlayerSearchCriterion() ), // Undead Slayer
					new SearchCriterionEntry( 1075462, new WolfSlayerSearchCriterion() ), // Wolf Slayer
				}),
				new SearchCriteriaCategory( 1154685, new SearchCriterionEntry[] { // Demon/Fey/Elemental Slayers
					new SearchCriterionEntry( 1079748, new DemonSlayerSearchCriterion() ), // Demon Slayer
					new SearchCriterionEntry( 1079737, new GargoyleSlayerSearchCriterion() ), // Gargoyle Slayer
					new SearchCriterionEntry( 1154652, new FeySlayerSearchCriterion() ), // Fey Slayer
					new SearchCriterionEntry( 1079749, new ElementalSlayerSearchCriterion() ), // Elemental Slayer
					new SearchCriterionEntry( 1079733, new AirElementalSlayerSearchCriterion() ), // Air Elemental Slayer
					new SearchCriterionEntry( 1079734, new BloodElementalSlayerSearchCriterion() ), // Blood Elemental Slayer
					new SearchCriterionEntry( 1079735, new EarthElementalSlayerSearchCriterion() ), // Earth Elemental Slayer
					new SearchCriterionEntry( 1079736, new FireElementalSlayerSearchCriterion() ), // Fire Elemental Slayer
					new SearchCriterionEntry( 1079742, new PoisonElementalSlayerSearchCriterion() ), // Poison Elemental Slayer
					new SearchCriterionEntry( 1079745, new SnowElementalSlayerSearchCriterion() ), // Snow Elemental Slayer
					new SearchCriterionEntry( 1079755, new WaterElementalSlayerSearchCriterion() ), // Water Elemental Slayer
				}),
				new SearchCriteriaCategory( 1154543, new SearchCriterionEntry[] { // Required Skill
					new SearchCriterionEntry( 1002151, new SwordsReqSkillSearchCriterion() ), // Swordsmanship
					new SearchCriterionEntry( 1044101, new MacingReqSkillSearchCriterion() ), // Mace Fighting
					new SearchCriterionEntry( 1044102, new FencingReqSkillSearchCriterion() ), // Fencing
					new SearchCriterionEntry( 1002029, new ArcheryReqSkillSearchCriterion() ), // Archery
					new SearchCriterionEntry( 1044117, new ThrowingReqSkillSearchCriterion() ), // Throwing
				}),
				new SearchCriteriaCategory( 1114255, new SearchCriterionEntry[] { // Skill Group 1
					new SearchCriterionEntry( 1002151, new SwordsBonusSearchCriteria() ), // Swordsmanship
					new SearchCriterionEntry( 1044102, new FencingBonusSearchCriteria() ), // Fencing
					new SearchCriterionEntry( 1044101, new MacingBonusSearchCriteria() ), // Mace Fighting
					new SearchCriterionEntry( 1002106, new MageryBonusSearchCriteria() ), // Magery
					new SearchCriterionEntry( 1002116, new MusicianshipBonusSearchCriteria() ), // Musicianship
				}),
				new SearchCriteriaCategory( 1114256, new SearchCriterionEntry[] { // Skill Group 2
					new SearchCriterionEntry( 1002169, new WrestlingBonusSearchCriteria() ), // Wrestling
					new SearchCriterionEntry( 1017321, new TacticsBonusSearchCriteria() ), // Tactics
					new SearchCriterionEntry( 1044095, new AnimalTamingBonusSearchCriteria() ), // Animal Taming
					new SearchCriterionEntry( 1002125, new ProvocationBonusSearchCriteria() ), // Provocation
					new SearchCriterionEntry( 1002140, new SpiritSpeakBonusSearchCriteria() ), // Spirit Speak
				}),
				new SearchCriteriaCategory( 1114257, new SearchCriterionEntry[] { // Skill Group 3
					new SearchCriterionEntry( 1044107, new StealthBonusSearchCriteria() ), // Stealth
					new SearchCriterionEntry( 1002118, new ParryBonusSearchCriteria() ), // Parrying
					new SearchCriterionEntry( 1044106, new MeditationBonusSearchCriteria() ), // Meditation
					new SearchCriterionEntry( 1002007, new AnimalLoreBonusSearchCriteria() ), // Animal Lore
					new SearchCriterionEntry( 1044075, new DiscordanceBonusSearchCriteria() ), // Discordance
					new SearchCriterionEntry( 1044110, new FocusBonusSearchCriteria() ), // Focus
				}),
				new SearchCriteriaCategory( 1114258, new SearchCriterionEntry[] { // Skill Group 4
					new SearchCriterionEntry( 1002142, new StealingBonusSearchCriteria() ), // Stealing
					new SearchCriterionEntry( 1002004, new AnatomyBonusSearchCriteria() ), // Anatomy
					new SearchCriterionEntry( 1044076, new EvalIntBonusSearchCriteria() ), // Eval Intelligence
					new SearchCriterionEntry( 1044099, new VeterinaryBonusSearchCriteria() ), // Veterinary
					new SearchCriterionEntry( 1044109, new NecromancyBonusSearchCriteria() ), // Necromancy
					new SearchCriterionEntry( 1044112, new BushidoBonusSearchCriteria() ), // Bushido
					new SearchCriterionEntry( 1044115, new MysticismBonusSearchCriteria() ), // Mysticism
				}),
				new SearchCriteriaCategory( 1114259, new SearchCriterionEntry[] { // Skill Group 5
					new SearchCriterionEntry( 1002082, new HealingBonusSearchCriteria() ), // Healing
					new SearchCriterionEntry( 1044086, new MagicResistBonusSearchCriteria() ), // Resisting Spells
					new SearchCriterionEntry( 1002120, new PeacemakingBonusSearchCriteria() ), // Peacemaking
					new SearchCriterionEntry( 1002029, new ArcheryBonusSearchCriteria() ), // Archery
					new SearchCriterionEntry( 1044111, new ChivalryBonusSearchCriteria() ), // Chivalry
					new SearchCriterionEntry( 1044113, new NinjitsuBonusSearchCriteria() ), // Ninjitsu
					new SearchCriterionEntry( 1044117, new ThrowingBonusSearchCriteria() ), // Throwing
				}),
				new SearchCriteriaCategory( 1114260, new SearchCriterionEntry[] { // Skill Group 6
					new SearchCriterionEntry( 1044104, new LumberjackingBonusSearchCriteria() ), // Lumberjacking
					new SearchCriterionEntry( 1044088, new SnoopingBonusSearchCriteria() ), // Snooping
					new SearchCriterionEntry( 1044105, new MiningBonusSearchCriteria() ), // Mining
				}),
			};

		public static SearchCriteriaCategory[] AllCategories { get { return m_AllCategories; } }
	}

	public class SearchCriterionEntry
	{
		private int m_Cliloc;
		private SearchCriterion m_Criterion;

		public int Cliloc { get { return m_Cliloc; } }
		public SearchCriterion Criterion { get { return m_Criterion; } }

		public SearchCriterionEntry( int cliloc, SearchCriterion criterion )
		{
			m_Cliloc = cliloc;
			m_Criterion = criterion;
		}
	}
}
