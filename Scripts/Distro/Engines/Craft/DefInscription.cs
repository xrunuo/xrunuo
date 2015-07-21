using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.Craft
{
	public class DefInscription : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefInscription();
		}

		public override SkillName MainSkill { get { return SkillName.Inscribe; } }

		public override int GumpTitleNumber
		{
			get { return 1044009; } // <CENTER>INSCRIPTION MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get { return m_CraftSystem; }
		}

		public override CraftECA ECA { get { return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double DefaultChanceAtMin
		{
			get { return 0.0; }
		}

		private DefInscription()
			: base( 1, 1, 1.25 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type typeItem )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			if ( typeItem != null )
			{
				object o = Activator.CreateInstance( typeItem );

				if ( o is SpellScroll )
				{
					SpellScroll scroll = (SpellScroll) o;
					Spellbook book = Spellbook.Find( from, scroll.SpellID );

					bool hasSpell = ( book != null && book.HasSpell( scroll.SpellID ) );

					scroll.Delete();

					return ( hasSpell ? 0 : 1044253 ); // null : You don't have the components needed to make that.
				}
				else if ( o is Item )
				{
					( (Item) o ).Delete();
				}
			}

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x249 );
		}

		private static Type typeofSpellScroll = typeof( SpellScroll );

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, bool exceptional, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( !typeofSpellScroll.IsAssignableFrom( item.ItemType ) ) //  not a scroll
			{
				if ( failed )
				{
					if ( lostMaterial )
						return 1044043; // You failed to create the item, and some of your materials are lost.
					else
						return 1044157; // You failed to create the item, but no materials were lost.
				}
				else
				{
					if ( makersMark && exceptional )
						return 1044156; // You create an exceptional quality item and affix your maker's mark.
					else if ( exceptional )
						return 1044155; // You create an exceptional quality item.
					else
						return 1044154; // You create the item.
				}
			}
			else
			{
				if ( failed )
					return 501630; // You fail to inscribe the scroll, and the scroll is ruined.
				else
					return 501629; // You inscribe the spell and put the scroll in your backpack.
			}
		}

		private int m_Circle, m_Mana;

		private enum Reg
		{
			BlackPearl,
			Bloodmoss,
			Garlic,
			Ginseng,
			MandrakeRoot,
			Nightshade,
			SulfurousAsh,
			SpidersSilk
		}

		private Type[] m_RegTypes = new Type[]
			{
				typeof( BlackPearl ),	typeof( Bloodmoss ),
				typeof( Garlic ),		typeof( Ginseng ),
				typeof( MandrakeRoot ),	typeof( Nightshade ),
				typeof( SulfurousAsh ), typeof( SpidersSilk )
			};

		private void AddSpell( int index, Type type, params Reg[] regs )
		{
			double minSkill, maxSkill;
			int groupCliloc;

			switch ( m_Circle )
			{
				default:
				case 0:
					minSkill = -25.0;
					maxSkill = 25.0;
					groupCliloc = 1111691;
					break;
				case 1:
					minSkill = -10.8;
					maxSkill = 39.2;
					groupCliloc = 1111691;
					break;
				case 2:
					minSkill = 03.5;
					maxSkill = 53.5;
					groupCliloc = 1111692;
					break;
				case 3:
					minSkill = 17.8;
					maxSkill = 67.8;
					groupCliloc = 1111692;
					break;
				case 4:
					minSkill = 32.1;
					maxSkill = 82.1;
					groupCliloc = 1111693;
					break;
				case 5:
					minSkill = 46.4;
					maxSkill = 96.4;
					groupCliloc = 1111693;
					break;
				case 6:
					minSkill = 60.7;
					maxSkill = 110.7;
					groupCliloc = 1111694;
					break;
				case 7:
					minSkill = 75.0;
					maxSkill = 125.0;
					groupCliloc = 1111694;
					break;
			}

			CraftItem craft = AddCraft( index, type, groupCliloc, 1044380 + index, minSkill, maxSkill, m_RegTypes[(int) regs[0]], 1044353 + (int) regs[0], 1, 1044361 + (int) regs[0] );

			for ( int i = 1; i < regs.Length; ++i )
				craft.AddRes( m_RegTypes[(int) regs[i]], 1044353 + (int) regs[i], 1, 1044361 + (int) regs[i] );

			craft.AddRes( typeof( BlankScroll ), 1044377, 1, 1044378 );
			craft.Mana = m_Mana;
		}

		private void AddNecroSpell( int index, int spell, int mana, double minSkill, Type type, params Type[] regs )
		{
			CraftItem craft = AddCraft( index, type, 1061677, 1060509 + spell, minSkill, minSkill + 1.0, regs[0], CraftItem.LabelNumber( regs[0] ), 1, 501627 );

			for ( int i = 1; i < regs.Length; ++i )
				craft.AddRes( regs[i], CraftItem.LabelNumber( regs[i] ), 1, 501627 );

			craft.AddRes( typeof( BlankScroll ), 1044377, 1, 1044378 );
			craft.Mana = mana;
		}

		private void AddMysticismSpell( int index, int spell, int mana, double minSkill, double maxSkill, Type type, params Type[] regs )
		{
			CraftItem craft = AddCraft( index, type, 1111671, 1031678 + spell, minSkill, maxSkill, regs[0], CraftItem.LabelNumber( regs[0] ), 1, 501627 );

			for ( int i = 1; i < regs.Length; ++i )
				craft.AddRes( regs[i], CraftItem.LabelNumber( regs[i] ), 1, 501627 );

			craft.AddRes( typeof( BlankScroll ), 1044377, 1, 1044378 );
			craft.Mana = mana;
		}

		public override void InitCraftList()
		{
			CraftItem craft = null;

			#region First - Second Circle
			m_Circle = 0;
			m_Mana = 4;

			AddSpell( 1, typeof( ReactiveArmorScroll ), Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 2, typeof( ClumsyScroll ), Reg.Bloodmoss, Reg.Nightshade );
			AddSpell( 3, typeof( CreateFoodScroll ), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot );
			AddSpell( 4, typeof( FeeblemindScroll ), Reg.Nightshade, Reg.Ginseng );
			AddSpell( 5, typeof( HealScroll ), Reg.Garlic, Reg.Ginseng, Reg.SpidersSilk );
			AddSpell( 6, typeof( MagicArrowScroll ), Reg.SulfurousAsh );
			AddSpell( 7, typeof( NightSightScroll ), Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 8, typeof( WeakenScroll ), Reg.Garlic, Reg.Nightshade );

			m_Circle = 1;
			m_Mana = 6;

			AddSpell( 9, typeof( AgilityScroll ), Reg.Bloodmoss, Reg.MandrakeRoot );
			AddSpell( 10, typeof( CunningScroll ), Reg.Nightshade, Reg.MandrakeRoot );
			AddSpell( 11, typeof( CureScroll ), Reg.Garlic, Reg.Ginseng );
			AddSpell( 12, typeof( HarmScroll ), Reg.Nightshade, Reg.SpidersSilk );
			AddSpell( 13, typeof( MagicTrapScroll ), Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 14, typeof( MagicUnTrapScroll ), Reg.Bloodmoss, Reg.SulfurousAsh );
			AddSpell( 15, typeof( ProtectionScroll ), Reg.Garlic, Reg.Ginseng, Reg.SulfurousAsh );
			AddSpell( 16, typeof( StrengthScroll ), Reg.Nightshade, Reg.MandrakeRoot );
			#endregion

			#region Third - Fourth Circle
			m_Circle = 2;
			m_Mana = 9;

			AddSpell( 17, typeof( BlessScroll ), Reg.Garlic, Reg.MandrakeRoot );
			AddSpell( 18, typeof( FireballScroll ), Reg.BlackPearl );
			AddSpell( 19, typeof( MagicLockScroll ), Reg.Bloodmoss, Reg.Garlic, Reg.SulfurousAsh );
			AddSpell( 20, typeof( PoisonScroll ), Reg.Nightshade );
			AddSpell( 21, typeof( TelekinisisScroll ), Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 22, typeof( TeleportScroll ), Reg.Bloodmoss, Reg.MandrakeRoot );
			AddSpell( 23, typeof( UnlockScroll ), Reg.Bloodmoss, Reg.SulfurousAsh );
			AddSpell( 24, typeof( WallOfStoneScroll ), Reg.Bloodmoss, Reg.Garlic );

			m_Circle = 3;
			m_Mana = 11;

			AddSpell( 25, typeof( ArchCureScroll ), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot );
			AddSpell( 26, typeof( ArchProtectionScroll ), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot, Reg.SulfurousAsh );
			AddSpell( 27, typeof( CurseScroll ), Reg.Garlic, Reg.Nightshade, Reg.SulfurousAsh );
			AddSpell( 28, typeof( FireFieldScroll ), Reg.BlackPearl, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 29, typeof( GreaterHealScroll ), Reg.Garlic, Reg.SpidersSilk, Reg.MandrakeRoot, Reg.Ginseng );
			AddSpell( 30, typeof( LightningScroll ), Reg.MandrakeRoot, Reg.SulfurousAsh );
			AddSpell( 31, typeof( ManaDrainScroll ), Reg.BlackPearl, Reg.SpidersSilk, Reg.MandrakeRoot );
			AddSpell( 32, typeof( RecallScroll ), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot );
			#endregion

			#region Fifth - Sixth Circle
			m_Circle = 4;
			m_Mana = 14;

			AddSpell( 33, typeof( BladeSpiritsScroll ), Reg.BlackPearl, Reg.Nightshade, Reg.MandrakeRoot );
			AddSpell( 34, typeof( DispelFieldScroll ), Reg.BlackPearl, Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 35, typeof( IncognitoScroll ), Reg.Bloodmoss, Reg.Garlic, Reg.Nightshade );
			AddSpell( 36, typeof( MagicReflectScroll ), Reg.Garlic, Reg.MandrakeRoot, Reg.SpidersSilk );
			AddSpell( 37, typeof( MindBlastScroll ), Reg.BlackPearl, Reg.MandrakeRoot, Reg.Nightshade, Reg.SulfurousAsh );
			AddSpell( 38, typeof( ParalyzeScroll ), Reg.Garlic, Reg.MandrakeRoot, Reg.SpidersSilk );
			AddSpell( 39, typeof( PoisonFieldScroll ), Reg.BlackPearl, Reg.Nightshade, Reg.SpidersSilk );
			AddSpell( 40, typeof( SummonCreatureScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk );

			m_Circle = 5;
			m_Mana = 20;

			AddSpell( 41, typeof( DispelScroll ), Reg.Garlic, Reg.MandrakeRoot, Reg.SulfurousAsh );
			AddSpell( 42, typeof( EnergyBoltScroll ), Reg.BlackPearl, Reg.Nightshade );
			AddSpell( 43, typeof( ExplosionScroll ), Reg.Bloodmoss, Reg.MandrakeRoot );
			AddSpell( 44, typeof( InvisibilityScroll ), Reg.Bloodmoss, Reg.Nightshade );
			AddSpell( 45, typeof( MarkScroll ), Reg.Bloodmoss, Reg.BlackPearl, Reg.MandrakeRoot );
			AddSpell( 46, typeof( MassCurseScroll ), Reg.Garlic, Reg.MandrakeRoot, Reg.Nightshade, Reg.SulfurousAsh );
			AddSpell( 47, typeof( ParalyzeFieldScroll ), Reg.BlackPearl, Reg.Ginseng, Reg.SpidersSilk );
			AddSpell( 48, typeof( RevealScroll ), Reg.Bloodmoss, Reg.SulfurousAsh );
			#endregion

			#region Seventh - Eighth Circle
			m_Circle = 6;
			m_Mana = 40;

			AddSpell( 49, typeof( ChainLightningScroll ), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SulfurousAsh );
			AddSpell( 50, typeof( EnergyFieldScroll ), Reg.BlackPearl, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 51, typeof( FlamestrikeScroll ), Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 52, typeof( GateTravelScroll ), Reg.BlackPearl, Reg.MandrakeRoot, Reg.SulfurousAsh );
			AddSpell( 53, typeof( ManaVampireScroll ), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk );
			AddSpell( 54, typeof( MassDispelScroll ), Reg.BlackPearl, Reg.Garlic, Reg.MandrakeRoot, Reg.SulfurousAsh );
			AddSpell( 55, typeof( MeteorSwarmScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SulfurousAsh, Reg.SpidersSilk );
			AddSpell( 56, typeof( PolymorphScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk );

			m_Circle = 7;
			m_Mana = 50;

			AddSpell( 57, typeof( EarthquakeScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.Ginseng, Reg.SulfurousAsh );
			AddSpell( 58, typeof( EnergyVortexScroll ), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.Nightshade );
			AddSpell( 59, typeof( ResurrectionScroll ), Reg.Bloodmoss, Reg.Garlic, Reg.Ginseng );
			AddSpell( 60, typeof( SummonAirElementalScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk );
			AddSpell( 61, typeof( SummonDaemonScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 62, typeof( SummonEarthElementalScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk );
			AddSpell( 63, typeof( SummonFireElementalScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh );
			AddSpell( 64, typeof( SummonWaterElementalScroll ), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk );
			#endregion

			#region Spells Of Necromancy
			AddNecroSpell( 101, 0, 23, 39.6, typeof( AnimateDeadScroll ), Reagent.GraveDust, Reagent.DaemonBlood );
			AddNecroSpell( 102, 1, 13, 19.6, typeof( BloodOathScroll ), Reagent.DaemonBlood );
			AddNecroSpell( 103, 2, 11, 19.6, typeof( CorpseSkinScroll ), Reagent.BatWing, Reagent.GraveDust );
			AddNecroSpell( 104, 3, 7, 19.6, typeof( CurseWeaponScroll ), Reagent.PigIron );
			AddNecroSpell( 105, 4, 11, 19.6, typeof( EvilOmenScroll ), Reagent.BatWing, Reagent.NoxCrystal );
			AddNecroSpell( 106, 5, 11, 39.6, typeof( HorrificBeastScroll ), Reagent.BatWing, Reagent.DaemonBlood );
			AddNecroSpell( 107, 6, 23, 69.6, typeof( LichFormScroll ), Reagent.GraveDust, Reagent.DaemonBlood, Reagent.NoxCrystal );
			AddNecroSpell( 108, 7, 17, 29.6, typeof( MindRotScroll ), Reagent.BatWing, Reagent.DaemonBlood, Reagent.PigIron );
			AddNecroSpell( 109, 8, 5, 19.6, typeof( PainSpikeScroll ), Reagent.GraveDust, Reagent.PigIron );
			AddNecroSpell( 110, 9, 17, 49.6, typeof( PoisonStrikeScroll ), Reagent.NoxCrystal );
			AddNecroSpell( 111, 10, 29, 64.6, typeof( StrangleScroll ), Reagent.DaemonBlood, Reagent.NoxCrystal );
			AddNecroSpell( 112, 11, 17, 29.6, typeof( SummonFamiliarScroll ), Reagent.BatWing, Reagent.GraveDust, Reagent.DaemonBlood );
			AddNecroSpell( 113, 12, 23, 98.6, typeof( VampiricEmbraceScroll ), Reagent.BatWing, Reagent.NoxCrystal, Reagent.PigIron );
			AddNecroSpell( 114, 13, 41, 79.6, typeof( VengefulSpiritScroll ), Reagent.BatWing, Reagent.GraveDust, Reagent.PigIron );
			AddNecroSpell( 115, 14, 23, 59.6, typeof( WitherScroll ), Reagent.GraveDust, Reagent.NoxCrystal, Reagent.PigIron );
			AddNecroSpell( 116, 15, 17, 79.6, typeof( WraithFormScroll ), Reagent.NoxCrystal, Reagent.PigIron );
			AddNecroSpell( 117, 16, 40, 79.6, typeof( ExorcismScroll ), Reagent.NoxCrystal, Reagent.GraveDust );
			#endregion

			#region Other
			craft = AddCraft( 118, typeof( EnchantedSwitch ), 1044294, 1072893, 45.0, 95.0, typeof( BlankScroll ), 1044377, 1, 1044378 );
			craft.AddRes( typeof( SpidersSilk ), 1015007, 1, 1044253 );
			craft.AddRes( typeof( BlackPearl ), 1015001, 1, 1044253 );
			craft.AddRes( typeof( SwitchItem ), 1024239, 1, 1044253 );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 119, typeof( RunedPrism ), 1044294, 1073465, 45.0, 95.0, typeof( BlankScroll ), 1044377, 1, 1044378 );
			craft.AddRes( typeof( SpidersSilk ), 1015007, 1, 1044253 );
			craft.AddRes( typeof( BlackPearl ), 1015001, 1, 1044253 );
			craft.AddRes( typeof( HollowPrism ), 1032125, 1, 1044253 );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 200, typeof( Runebook ), 1044294, 1041267, 45.0, 95.0, typeof( BlankScroll ), 1044377, 8, 1044378 );
			craft.AddRes( typeof( RecallScroll ), 1044445, 1, 1044253 );
			craft.AddRes( typeof( GateTravelScroll ), 1044446, 1, 1044253 );

			AddCraft( 201, typeof( Engines.BulkOrders.BulkOrderBook ), 1044294, 1028793, 65.0, 115.0, typeof( BlankScroll ), 1044377, 10, 1044378 );

			AddCraft( 202, typeof( Spellbook ), 1044294, 1023834, 50.0, 150.0, typeof( BlankScroll ), 1044377, 10, 1044378 );

			craft = AddCraft( 204, typeof( ScrappersCompendium ), 1044294, 1072940, 85.0, 135.0, typeof( BlankScroll ), 1044377, 100, 1044378 );
			craft.AddRes( typeof( DreadHornMane ), 1032682, 1, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRecipe( (int) TinkerRecipeGreater.ScrappersCompendium, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 205, typeof( SpellBookEngravingTool ), 1044294, 1072151, 75.0, 125.0, typeof( Feather ), 1044562, 1, 1044253 );
			craft.AddRes( typeof( BlackPearl ), 1015001, 7, 1044253 );
			craft.RequiresML = true;

			AddCraft( 206, typeof( MysticismSpellbook ), 1044294, 1031677, 50.0, 150.0, typeof( BlankScroll ), 1044377, 10, 1044378 );
			AddCraft( 207, typeof( NecromancerSpellbook ), 1044294, 1028787, 50.0, 150.0, typeof( BlankScroll ), 1044377, 10, 1044378 );

			craft = AddCraft( 300, typeof( BlankScroll ), 1044294, 1044377, 50.0, 75.0, typeof( WoodPulp ), 1113136, 1, 1113289 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 301, typeof( ScrollBinder ), 1044294, 1113135, 75.0, 100.0, typeof( WoodPulp ), 1113136, 1, 1113289 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 302, typeof( GreenBook ), 1044294, 1113290, 50.0, 75.0, typeof( BlankScroll ), 1044377, 40, 1044378 );
			craft.AddRes( typeof( Beeswax ), 1025154, 2, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 303, typeof( GargishBook ), 1044294, 1113291, 50.0, 75.0, typeof( BlankScroll ), 1044377, 80, 1044378 );
			craft.AddRes( typeof( Beeswax ), 1025154, 4, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;
			#endregion

			#region Spells Of Mysticism
			AddMysticismSpell( 678, 0, 4, -25.0, 25.0, typeof( NetherBoltScroll ), Reagent.BlackPearl, Reagent.SulfurousAsh );
			AddMysticismSpell( 679, 1, 4, -25.0, 25.0, typeof( HealingStoneScroll ), Reagent.Bone, Reagent.Garlic, Reagent.Ginseng, Reagent.SpidersSilk );
			AddMysticismSpell( 680, 2, 6, -10.8, 39.2, typeof( PurgeMagicScroll ), Reagent.FertileDirt, Reagent.Garlic, Reagent.MandrakeRoot, Reagent.SulfurousAsh );
			AddMysticismSpell( 681, 3, 6, -10.8, 39.2, typeof( EnchantScroll ), Reagent.SpidersSilk, Reagent.MandrakeRoot, Reagent.SulfurousAsh );
			AddMysticismSpell( 682, 4, 9, 3.5, 53.5, typeof( SleepScroll ), Reagent.Nightshade, Reagent.SpidersSilk, Reagent.BlackPearl );
			AddMysticismSpell( 683, 5, 9, 3.5, 53.5, typeof( EagleStrikeScroll ), Reagent.Bloodmoss, Reagent.Bone, Reagent.SpidersSilk, Reagent.MandrakeRoot );
			AddMysticismSpell( 684, 6, 11, 17.8, 67.8, typeof( AnimatedWeaponScroll ), Reagent.Bone, Reagent.BlackPearl, Reagent.MandrakeRoot, Reagent.Nightshade );
			AddMysticismSpell( 685, 7, 11, 17.8, 67.8, typeof( StoneFormScroll ), Reagent.Bloodmoss, Reagent.FertileDirt, Reagent.Garlic );
			AddMysticismSpell( 686, 8, 14, 32.1, 82.1, typeof( SpellTriggerScroll ), Reagent.DragonsBlood, Reagent.Garlic, Reagent.MandrakeRoot, Reagent.SpidersSilk );
			AddMysticismSpell( 687, 9, 14, 32.1, 82.1, typeof( MassSleepScroll ), Reagent.Ginseng, Reagent.Nightshade, Reagent.SpidersSilk );
			AddMysticismSpell( 688, 10, 20, 46.4, 96.4, typeof( CleansingWindsScroll ), Reagent.DragonsBlood, Reagent.Garlic, Reagent.Ginseng, Reagent.MandrakeRoot );
			AddMysticismSpell( 689, 11, 20, 46.4, 96.4, typeof( BombardScroll ), Reagent.Bloodmoss, Reagent.DragonsBlood, Reagent.Garlic, Reagent.SulfurousAsh );
			AddMysticismSpell( 690, 12, 40, 60.7, 110.7, typeof( SpellPlagueScroll ), Reagent.DaemonBone, Reagent.DragonsBlood, Reagent.Nightshade, Reagent.SulfurousAsh );
			AddMysticismSpell( 691, 13, 40, 60.7, 110.7, typeof( HailStormScroll ), Reagent.DragonsBlood, Reagent.Bloodmoss, Reagent.BlackPearl, Reagent.MandrakeRoot );
			AddMysticismSpell( 692, 14, 50, 75.0, 125.0, typeof( NetherCycloneScroll ), Reagent.MandrakeRoot, Reagent.Nightshade, Reagent.SulfurousAsh, Reagent.Bloodmoss );
			AddMysticismSpell( 693, 15, 50, 75.0, 125.0, typeof( RisingColossusScroll ), Reagent.DaemonBone, Reagent.DragonsBlood, Reagent.FertileDirt, Reagent.Nightshade );
			#endregion

			MarkOption = true;
		}
	}
}