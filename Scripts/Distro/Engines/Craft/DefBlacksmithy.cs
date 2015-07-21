using System;
using Server.Items;

namespace Server.Engines.Craft
{
	#region Recipes
	public enum SmithRecipe
	{
		TrueSpellblade = 37,
		IcySpellblade = 5,
		FierySpellblade = 6,
		SpellbladeOfDefense = 7,
		TrueAssassinSpike = 8,
		ChargedAssassinSpike = 9,
		MagekillerAssassinSpike = 10,
		WoundingAssassinSpike = 11,
		TrueLeafblade = 12,
		Luckblade = 13,
		MagekillerLeafblade = 14,
		LeafbladeOfEase = 15,
		KnightsWarCleaver = 16,
		ButchersWarCleaver = 17,
		SerratedWarCleaver = 18,
		TrueWarCleaver = 19,
		AdventurersMachete = 20,
		OrcishMachete = 21,
		MacheteOfDefense = 22,
		DiseasedMachete = 23,
		Runesabre = 24,
		MagesRuneBlade = 25,
		RuneBladeOfKnowledge = 26,
		CorruptedRuneBlade = 27,
		TrueRadiantScimitar = 28,
		DarkglowScimitar = 29,
		IcyScimitar = 30,
		TwinklingScimitar = 31,
		GuardianAxe = 33,
		SingingAxe = 34,
		ThunderingAxe = 35,
		HeavyOrnateAxe = 36,
		RubyMace = 38,
		EmeraldMace = 39,
		SapphireMace = 40,
		SilverEtchedMace = 32,
		BoneMachete = 41
	}

	public enum SmithRecipeGreater
	{
		RuneCarvingKnife = 0,
		ColdForgedBlade = 1,
		OverseerSunderedBlade = 2,
		LuminousRuneBlade = 3,
		ShardTrasher = 4
	}
	#endregion

	public class DefBlacksmithy : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefBlacksmithy();
		}

		public override SkillName MainSkill { get { return SkillName.Blacksmith; } }

		public override int GumpTitleNumber
		{
			get { return 1044002; } // <CENTER>BLACKSMITHY MENU</CENTER>
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

		private DefBlacksmithy()
			: base( 1, 1, 1.25 )
		{
		}

		private static Type typeofAnvil = typeof( AnvilAttribute );
		private static Type typeofForge = typeof( ForgeAttribute );

		public static void CheckAnvilAndForge( Mobile from, int range, out bool anvil, out bool forge )
		{
			anvil = false;
			forge = false;

			Map map = from.Map;

			if ( map == null )
				return;

			var eable = map.GetItemsInRange( from.Location, range );

			foreach ( Item item in eable )
			{
				Type type = item.GetType();

				bool isAnvil = ( type.IsDefined( typeofAnvil, false ) || item.ItemID == 4015 || item.ItemID == 4016 || item.ItemID == 11734 || item.ItemID == 11733 );
				bool isForge = ( type.IsDefined( typeofForge, false ) || item.ItemID == 4017 || ( item.ItemID >= 6522 && item.ItemID <= 6569 ) || item.ItemID == 11736 );

				if ( isAnvil || isForge )
				{
					if ( ( from.Z + 16 ) < item.Z || ( item.Z + 16 ) < from.Z || !from.InLOS( item ) )
						continue;

					anvil = anvil || isAnvil;
					forge = forge || isForge;

					if ( anvil && forge )
						break;
				}
			}


			for ( int x = -range; ( !anvil || !forge ) && x <= range; ++x )
			{
				for ( int y = -range; ( !anvil || !forge ) && y <= range; ++y )
				{
					Tile[] tiles = map.Tiles.GetStaticTiles( from.X + x, from.Y + y, true );

					for ( int i = 0; ( !anvil || !forge ) && i < tiles.Length; ++i )
					{
						int id = tiles[i].ID & TileData.MaxItemValue;

						bool isAnvil = ( id == 4015 || id == 4016 || id == 11733 || id == 11734 );
						bool isForge = ( id == 4017 || ( id >= 6522 && id <= 6569 ) || id == 11736 );

						if ( isAnvil || isForge )
						{
							if ( ( from.Z + 16 ) < tiles[i].Z || ( tiles[i].Z + 16 ) < from.Z || !from.InLOS( new Point3D( from.X + x, from.Y + y, tiles[i].Z + ( tiles[i].Height / 2 ) + 1 ) ) )
								continue;

							anvil = anvil || isAnvil;
							forge = forge || isForge;
						}
					}
				}
			}
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			bool anvil, forge;
			CheckAnvilAndForge( from, 2, out anvil, out forge );

			if ( anvil && forge )
				return 0;

			return 1044267; // You must be near an anvil and a forge to smith items.
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x2A );
		}

		// Delay to synchronize the sound with the hit on the anvil
		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from )
				: base( TimeSpan.FromSeconds( 0.7 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound( 0x2A );
			}
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, bool exceptional, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

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

		public override void InitCraftList()
		{
			CraftItem craft = null;

			#region Metal Armor
			AddCraft( 1, typeof( RingmailGloves ), 1111704, 1025099, 12.0, 62.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 2, typeof( RingmailLegs ), 1111704, 1025104, 19.4, 69.4, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( 3, typeof( RingmailArms ), 1111704, 1025103, 16.9, 66.9, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 4, typeof( RingmailChest ), 1111704, 1025100, 21.9, 71.9, typeof( IronIngot ), 1044036, 18, 1044037 );

			AddCraft( 5, typeof( ChainCoif ), 1111704, 1025051, 14.5, 64.5, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 6, typeof( ChainLegs ), 1111704, 1025054, 36.7, 86.7, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 7, typeof( ChainChest ), 1111704, 1025055, 39.1, 89.1, typeof( IronIngot ), 1044036, 20, 1044037 );

			AddCraft( 8, typeof( PlateArms ), 1111704, 1025136, 66.3, 116.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 9, typeof( PlateGloves ), 1111704, 1025140, 58.9, 108.9, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 10, typeof( PlateGorget ), 1111704, 1025139, 56.4, 106.4, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 11, typeof( PlateLegs ), 1111704, 1025137, 68.8, 118.8, typeof( IronIngot ), 1044036, 20, 1044037 );
			AddCraft( 12, typeof( PlateChest ), 1111704, 1046431, 75.0, 125.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			AddCraft( 13, typeof( FemalePlateChest ), 1111704, 1046430, 44.1, 94.1, typeof( IronIngot ), 1044036, 20, 1044037 );

			AddCraft( 14, typeof( DragonBardingDeed ), 1111704, 1053012, 72.5, 122.5, typeof( IronIngot ), 1044036, 750, 1044037 );

			craft = AddCraft( 15, typeof( PlateMempo ), 1111704, 1030180, 80.0, 130.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 16, typeof( PlateDo ), 1111704, 1030184, 80.0, 130.0, typeof( IronIngot ), 1044036, 28, 1044037 ); //Double check skill
			craft.RequiresSE = true;
			craft = AddCraft( 17, typeof( PlateHiroSode ), 1111704, 1030187, 80.0, 130.0, typeof( IronIngot ), 1044036, 16, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 18, typeof( PlateSuneate ), 1111704, 1030195, 65.0, 115.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 19, typeof( PlateHaidate ), 1111704, 1030200, 65.0, 115.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSE = true;

			AddCraft( 216, typeof( FemaleGargishPlatemailArms ), 1111704, 1095335, 66.3, 116.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 217, typeof( FemaleGargishPlatemailChest ), 1111704, 1095337, 75.0, 125.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			AddCraft( 218, typeof( FemaleGargishPlatemailLeggings ), 1111704, 1095341, 68.8, 118.8, typeof( IronIngot ), 1044036, 20, 1044037 );
			AddCraft( 219, typeof( FemaleGargishPlatemailKilt ), 1111704, 1095339, 58.9, 108.9, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 220, typeof( GargishPlatemailArms ), 1111704, 1095336, 66.3, 116.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 221, typeof( GargishPlatemailChest ), 1111704, 1095338, 75.0, 125.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			AddCraft( 222, typeof( GargishPlatemailLeggings ), 1111704, 1095342, 68.8, 118.8, typeof( IronIngot ), 1044036, 20, 1044037 );
			AddCraft( 223, typeof( GargishPlatemailKilt ), 1111704, 1095340, 58.9, 108.9, typeof( IronIngot ), 1044036, 12, 1044037 );
			#endregion

			#region Helmets
			AddCraft( 20, typeof( Bascinet ), 1011079, 1025132, 8.3, 58.3, typeof( IronIngot ), 1044036, 15, 1044037 );
			AddCraft( 21, typeof( CloseHelm ), 1011079, 1025128, 37.9, 87.9, typeof( IronIngot ), 1044036, 15, 1044037 );
			AddCraft( 22, typeof( Helmet ), 1011079, 1025130, 37.9, 87.9, typeof( IronIngot ), 1044036, 15, 1044037 );
			AddCraft( 23, typeof( NorseHelm ), 1011079, 1025134, 37.9, 87.9, typeof( IronIngot ), 1044036, 15, 1044037 );
			AddCraft( 24, typeof( PlateHelm ), 1011079, 1025138, 62.6, 112.6, typeof( IronIngot ), 1044036, 15, 1044037 );

			craft = AddCraft( 25, typeof( ChainHatsuburi ), 1011079, 1030175, 30.0, 80.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 26, typeof( PlateHatsuburi ), 1011079, 1030176, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 27, typeof( HeavyPlateJingasa ), 1011079, 1030178, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 28, typeof( LightPlateJingasa ), 1011079, 1030188, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 29, typeof( SmallPlateJingasa ), 1011079, 1030191, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 30, typeof( DecorativePlateKabuto ), 1011079, 1030179, 90.0, 140.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 31, typeof( PlateBattleKabuto ), 1011079, 1030192, 90.0, 140.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 32, typeof( StandardPlateKabuto ), 1011079, 1030196, 90.0, 140.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			craft.RequiresSE = true;

			craft = AddCraft( 104, typeof( Circlet ), 1011079, 1031118, 62.1, 112.1, typeof( IronIngot ), 1044036, 6, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 105, typeof( RoyalCirclet ), 1011079, 1031119, 70.0, 120.0, typeof( IronIngot ), 1044036, 6, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 106, typeof( GemmedCirclet ), 1011079, 1031120, 75.0, 125.0, typeof( IronIngot ), 1044036, 6, 1044037 );
			craft.AddRes( typeof( Amethyst ), 1023862, 1, 1044253 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 1, 1044253 );
			craft.AddRes( typeof( Tourmaline ), 1023864, 1, 1044253 );
			craft.RequiresML = true;
			#endregion

			#region Shields
			AddCraft( 33, typeof( Buckler ), 1011080, 1027027, -25.0, 25.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 34, typeof( BronzeShield ), 1011080, 1027026, -15.2, 34.8, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 35, typeof( HeaterShield ), 1011080, 1027030, 24.3, 74.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 36, typeof( MetalShield ), 1011080, 1027035, -10.2, 39.8, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 37, typeof( MetalKiteShield ), 1011080, 1027028, 4.6, 54.6, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( 38, typeof( WoodenKiteShield ), 1011080, 1027032, -15.2, 34.8, typeof( IronIngot ), 1044036, 8, 1044037 );
			AddCraft( 39, typeof( ChaosShield ), 1011080, 1027107, 85.0, 135.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			AddCraft( 40, typeof( OrderShield ), 1011080, 1027108, 85.0, 135.0, typeof( IronIngot ), 1044036, 25, 1044037 );

			AddCraft( 360, typeof( SmallPlateShield ), 1011080, 1095770, -25.0, 25.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 361, typeof( GargishKiteShield ), 1011080, 1095769, 4.6, 54.6, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( 362, typeof( LargePlateShield ), 1011080, 1095772, 24.3, 74.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 363, typeof( MediumPlateShield ), 1011080, 1095771, -10.2, 39.8, typeof( IronIngot ), 1044036, 14, 1044037 );

			AddCraft( 374, typeof( GargishChaosShield ), 1011080, 1095809, 85.0, 135.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			AddCraft( 375, typeof( GargishOrderShield ), 1011080, 1095812, 85.0, 135.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			#endregion

			#region Bladed
			AddCraft( 41, typeof( BoneHarvester ), 1011081, 1029915, 33.0, 83.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 42, typeof( Broadsword ), 1011081, 1023934, 35.4, 85.4, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 43, typeof( CrescentBlade ), 1011081, 1029921, 45.0, 95.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 44, typeof( Cutlass ), 1011081, 1025185, 24.3, 74.3, typeof( IronIngot ), 1044036, 8, 1044037 );
			AddCraft( 45, typeof( Dagger ), 1011081, 1023921, -0.4, 49.6, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( 46, typeof( Katana ), 1011081, 1025119, 44.1, 94.1, typeof( IronIngot ), 1044036, 8, 1044037 );
			AddCraft( 47, typeof( Kryss ), 1011081, 1025121, 36.7, 86.7, typeof( IronIngot ), 1044036, 8, 1044037 );
			AddCraft( 48, typeof( Longsword ), 1011081, 1023937, 28.0, 78.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 49, typeof( Scimitar ), 1011081, 1025046, 31.7, 81.7, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 50, typeof( VikingSword ), 1011081, 1025049, 24.3, 74.3, typeof( IronIngot ), 1044036, 14, 1044037 );

			craft = AddCraft( 51, typeof( NoDachi ), 1011081, 1030221, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 52, typeof( Wakizashi ), 1011081, 1030223, 50.0, 100.0, typeof( IronIngot ), 1044036, 8, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 53, typeof( Lajatang ), 1011081, 1030226, 80.0, 130.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 54, typeof( Daisho ), 1011081, 1030228, 60.0, 110.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 55, typeof( Tekagi ), 1011081, 1030230, 55.0, 105.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 56, typeof( Shuriken ), 1011081, 1030231, 45.0, 95.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 57, typeof( Kama ), 1011081, 1030232, 40.0, 90.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.RequiresSE = true;
			craft = AddCraft( 58, typeof( Sai ), 1011081, 1030234, 50.0, 100.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.RequiresSE = true;

			craft = AddCraft( 90, typeof( RadiantScimitar ), 1011081, 1031559, 70.0, 120.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 91, typeof( WarCleaver ), 1011081, 1031555, 70.0, 120.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 1, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 92, typeof( ElvenSpellBlade ), 1011081, 1031552, 70.0, 120.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 93, typeof( AssassinSpike ), 1011081, 1031553, 70.0, 120.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 94, typeof( Leafblade ), 1011081, 1031554, 70.0, 120.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 96, typeof( RuneBlade ), 1011081, 1031558, 70.0, 120.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 97, typeof( OrnateAxe ), 1011082, 1031560, 70.0, 120.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 98, typeof( ElvenMachete ), 1011081, 1031561, 70.0, 120.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 99, typeof( RuneCarvingKnife ), 1011081, 1072915, 70.0, 120.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			craft.AddRes( typeof( DreadHornMane ), 1032682, 1, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.AddRes( typeof( Muculent ), 1032680, 10, 1044253 );
			craft.AddRecipe( (int) SmithRecipeGreater.RuneCarvingKnife, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 100, typeof( ColdForgedBlade ), 1011081, 1072916, 70.0, 120.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( GrizzledBones ), 1032684, 1, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRecipe( (int) SmithRecipeGreater.ColdForgedBlade, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 102, typeof( OverseerSunderedBlade ), 1011081, 1072920, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( GrizzledBones ), 1032684, 1, 1044253 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRes( typeof( Scourge ), 1032677, 10, 1044253 );
			craft.AddRecipe( (int) SmithRecipeGreater.OverseerSunderedBlade, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 103, typeof( LuminousRuneBlade ), 1011081, 1072922, 70.0, 120.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( GrizzledBones ), 1032684, 1, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.AddRecipe( (int) SmithRecipeGreater.LuminousRuneBlade, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 107, typeof( TrueSpellblade ), 1011081, 1073513, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.TrueSpellblade, this );
			craft.RequiresML = true;

			craft = AddCraft( 108, typeof( IcySpellblade ), 1011081, 1073514, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.AddRes( typeof( Turquoise ), 1026250, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.IcySpellblade, this );
			craft.RequiresML = true;

			craft = AddCraft( 109, typeof( FierySpellblade ), 1011081, 1073515, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.AddRes( typeof( FireRuby ), 1026254, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.FierySpellblade, this );
			craft.RequiresML = true;

			craft = AddCraft( 110, typeof( SpellbladeOfDefense ), 1011081, 1073516, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.SpellbladeOfDefense, this );
			craft.RequiresML = true;

			craft = AddCraft( 111, typeof( TrueAssassinSpike ), 1011081, 1073517, 75.0, 125.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			craft.AddRes( typeof( DarkSapphire ), 1026249, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.TrueAssassinSpike, this );
			craft.RequiresML = true;

			craft = AddCraft( 112, typeof( ChargedAssassinSpike ), 1011081, 1073518, 75.0, 125.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			craft.AddRes( typeof( EcruCitrine ), 1026252, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.ChargedAssassinSpike, this );
			craft.RequiresML = true;

			craft = AddCraft( 113, typeof( MagekillerAssassinSpike ), 1011081, 1073519, 75.0, 125.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			craft.AddRes( typeof( BrilliantAmber ), 1026256, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.MagekillerAssassinSpike, this );
			craft.RequiresML = true;

			craft = AddCraft( 114, typeof( WoundingAssassinSpike ), 1011081, 1073520, 75.0, 125.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.WoundingAssassinSpike, this );
			craft.RequiresML = true;

			craft = AddCraft( 115, typeof( TrueLeafblade ), 1011081, 1073521, 75.0, 125.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.TrueLeafblade, this );
			craft.RequiresML = true;

			craft = AddCraft( 116, typeof( Luckblade ), 1011081, 1073522, 75.0, 125.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.Luckblade, this );
			craft.RequiresML = true;

			craft = AddCraft( 117, typeof( MagekillerLeafblade ), 1011081, 1073523, 75.0, 125.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.AddRes( typeof( FireRuby ), 1026254, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.MagekillerLeafblade, this );
			craft.RequiresML = true;

			craft = AddCraft( 118, typeof( LeafbladeOfEase ), 1011081, 1073524, 75.0, 125.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.LeafbladeOfEase, this );
			craft.RequiresML = true;

			craft = AddCraft( 119, typeof( KnightsWarCleaver ), 1011081, 1073525, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.KnightsWarCleaver, this );
			craft.RequiresML = true;

			craft = AddCraft( 120, typeof( ButchersWarCleaver ), 1011081, 1073526, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( Turquoise ), 1026250, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.ButchersWarCleaver, this );
			craft.RequiresML = true;

			craft = AddCraft( 121, typeof( SerratedWarCleaver ), 1011081, 1073527, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( EcruCitrine ), 1026252, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.SerratedWarCleaver, this );
			craft.RequiresML = true;

			craft = AddCraft( 122, typeof( TrueWarCleaver ), 1011081, 1073528, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( BrilliantAmber ), 1026256, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.TrueWarCleaver, this );
			craft.RequiresML = true;

			craft = AddCraft( 127, typeof( AdventurersMachete ), 1011081, 1073533, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.AddRes( typeof( WhitePearl ), 1032694, 1, 1049063 );
			craft.AddRecipe( (int) SmithRecipe.AdventurersMachete, this );
			craft.RequiresML = true;

			craft = AddCraft( 128, typeof( OrcishMachete ), 1011081, 1073534, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.AddRes( typeof( Scourge ), 1032677, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.OrcishMachete, this );
			craft.RequiresML = true;

			craft = AddCraft( 129, typeof( MacheteOfDefense ), 1011081, 1073535, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.AddRes( typeof( BrilliantAmber ), 1026256, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.MacheteOfDefense, this );
			craft.RequiresML = true;

			craft = AddCraft( 130, typeof( DiseasedMachete ), 1011081, 1073536, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.AddRes( typeof( Blight ), 1032675, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.DiseasedMachete, this );
			craft.RequiresML = true;

			craft = AddCraft( 131, typeof( Runesabre ), 1011081, 1073537, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( Turquoise ), 1026250, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.Runesabre, this );
			craft.RequiresML = true;

			craft = AddCraft( 132, typeof( MagesRuneBlade ), 1011081, 1073538, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.MagesRuneBlade, this );
			craft.RequiresML = true;

			craft = AddCraft( 133, typeof( RuneBladeOfKnowledge ), 1011081, 1073539, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( EcruCitrine ), 1026252, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.RuneBladeOfKnowledge, this );
			craft.RequiresML = true;

			craft = AddCraft( 134, typeof( CorruptedRuneBlade ), 1011081, 1073540, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( Corruption ), 1032676, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.CorruptedRuneBlade, this );
			craft.RequiresML = true;

			craft = AddCraft( 135, typeof( TrueRadiantScimitar ), 1011081, 1073541, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( BrilliantAmber ), 1026256, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.TrueRadiantScimitar, this );
			craft.RequiresML = true;

			craft = AddCraft( 136, typeof( DarkglowScimitar ), 1011081, 1073542, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( DarkSapphire ), 1026249, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.DarkglowScimitar, this );
			craft.RequiresML = true;

			craft = AddCraft( 137, typeof( IcyScimitar ), 1011081, 1073543, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( DarkSapphire ), 1026249, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.IcyScimitar, this );
			craft.RequiresML = true;

			craft = AddCraft( 138, typeof( TwinklingScimitar ), 1011081, 1073544, 75.0, 125.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.AddRes( typeof( DarkSapphire ), 1026249, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.TwinklingScimitar, this );
			craft.RequiresML = true;

			craft = AddCraft( 143, typeof( BoneMachete ), 1011081, 1020526, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.AddRes( typeof( Bone ), 1049064, 6, 1049063 );
			craft.AddRecipe( (int) SmithRecipe.BoneMachete, this );
			craft.RequiresML = true;

			craft = AddCraft( 147, typeof( GargishKatana ), 1011081, 1097490, 44.1, 94.1, typeof( IronIngot ), 1044036, 8, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 148, typeof( GargishKryss ), 1011081, 1097492, 36.7, 86.7, typeof( IronIngot ), 1044036, 8, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 153, typeof( GargishBoneHarvester ), 1011081, 1097502, 33.0, 83.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 157, typeof( GargishTekagi ), 1011081, 1097510, 55.0, 105.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 158, typeof( GargishDaisho ), 1011081, 1097512, 60.0, 110.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.RequiresSA = true;

			AddCraft( 201, typeof( DreadSword ), 1011081, 1095372, 75.0, 125.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 202, typeof( GargishTalwar ), 1011081, 1095373, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 204, typeof( GargishDagger ), 1011081, 1095362, -0.4, 49.6, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( 205, typeof( Bloodblade ), 1011081, 1095370, 44.1, 94.1, typeof( IronIngot ), 1044036, 8, 1044037 );
			AddCraft( 206, typeof( Shortblade ), 1011081, 1095374, 28.0, 78.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			#endregion

			#region Axes
			AddCraft( 59, typeof( Axe ), 1011082, 1023913, 34.2, 84.2, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 60, typeof( BattleAxe ), 1011082, 1023911, 30.5, 80.5, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 61, typeof( DoubleAxe ), 1011082, 1023915, 29.3, 79.3, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 62, typeof( ExecutionersAxe ), 1011082, 1023909, 34.2, 84.2, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 63, typeof( LargeBattleAxe ), 1011082, 1025115, 28.0, 78.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 64, typeof( TwoHandedAxe ), 1011082, 1025187, 33.0, 83.0, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( 65, typeof( WarAxe ), 1011082, 1025040, 39.1, 89.1, typeof( IronIngot ), 1044036, 16, 1044037 );

			craft = AddCraft( 139, typeof( GuardianAxe ), 1011082, 1073545, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.GuardianAxe, this );
			craft.RequiresML = true;

			craft = AddCraft( 140, typeof( SingingAxe ), 1011082, 1073546, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( BrilliantAmber ), 1026256, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.SingingAxe, this );
			craft.RequiresML = true;

			craft = AddCraft( 141, typeof( ThunderingAxe ), 1011082, 1073547, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( EcruCitrine ), 1026252, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.ThunderingAxe, this );
			craft.RequiresML = true;

			craft = AddCraft( 142, typeof( HeavyOrnateAxe ), 1011082, 1073548, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.AddRes( typeof( Turquoise ), 1026250, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.HeavyOrnateAxe, this );
			craft.RequiresML = true;

			craft = AddCraft( 144, typeof( GargishBattleAxe ), 1011082, 1097480, 30.5, 80.5, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 145, typeof( GargishAxe ), 1011082, 1097482, 34.2, 84.2, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.RequiresSA = true;

			AddCraft( 200, typeof( DualShortAxes ), 1011082, 1095360, 75.0, 125.0, typeof( IronIngot ), 1044036, 24, 1044037 );
			#endregion

			#region Pole Arms
			AddCraft( 66, typeof( Bardiche ), 1011083, 1023917, 31.7, 81.7, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( 67, typeof( BladedStaff ), 1011083, 1029917, 40.0, 90.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 68, typeof( DoubleBladedStaff ), 1011083, 1029919, 45.0, 95.0, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( 69, typeof( Halberd ), 1011083, 1025183, 39.1, 89.1, typeof( IronIngot ), 1044036, 20, 1044037 );
			AddCraft( 70, typeof( Lance ), 1011083, 1029920, 48.0, 98.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			AddCraft( 71, typeof( Pike ), 1011083, 1029918, 47.0, 97.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 72, typeof( ShortSpear ), 1011083, 1025123, 45.3, 95.3, typeof( IronIngot ), 1044036, 6, 1044037 );
			AddCraft( 73, typeof( Scythe ), 1011083, 1029914, 39.0, 89.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 74, typeof( Spear ), 1011083, 1023938, 49.0, 99.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( 75, typeof( WarFork ), 1011083, 1025125, 42.9, 92.9, typeof( IronIngot ), 1044036, 12, 1044037 );

			craft = AddCraft( 146, typeof( GargishBardiche ), 1011083, 1097484, 31.7, 81.7, typeof( IronIngot ), 1044036, 18, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 149, typeof( GargishWarFork ), 1011083, 1097494, 42.9, 92.9, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 152, typeof( GargishScythe ), 1011083, 1097500, 39.0, 89.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 154, typeof( GargishPike ), 1011083, 1097504, 47.0, 97.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			craft.RequiresSA = true;
			craft = AddCraft( 155, typeof( GargishLance ), 1011083, 1097506, 48.0, 98.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresSA = true;

			AddCraft( 203, typeof( DualPointedSpear ), 1011083, 1095365, 47.0, 97.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			#endregion

			#region Bashing
			AddCraft( 76, typeof( HammerPick ), 1011084, 1025181, 34.2, 84.2, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( 77, typeof( Mace ), 1011084, 1023932, 14.5, 64.5, typeof( IronIngot ), 1044036, 6, 1044037 );
			AddCraft( 78, typeof( Maul ), 1011084, 1025179, 19.4, 69.4, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 79, typeof( Scepter ), 1011084, 1029916, 21.4, 71.4, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( 80, typeof( WarMace ), 1011084, 1025127, 28.0, 78.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( 81, typeof( WarHammer ), 1011084, 1025177, 34.2, 84.2, typeof( IronIngot ), 1044036, 16, 1044037 );

			craft = AddCraft( 82, typeof( Tessen ), 1011084, 1030222, 85.0, 135.0, typeof( IronIngot ), 1044036, 16, 1044037 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );
			craft.RequiresSE = true;

			craft = AddCraft( 95, typeof( DiamondMace ), 1011084, 1031556, 70.0, 120.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.RequiresML = true;

			craft = AddCraft( 101, typeof( ShardThrasher ), 1011084, 1072918, 70.0, 120.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.AddRes( typeof( EyeOfTheTravesty ), 1032685, 1, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRes( typeof( Muculent ), 1032680, 10, 1044253 );
			craft.AddRecipe( (int) SmithRecipeGreater.ShardTrasher, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 123, typeof( RubyMace ), 1011084, 1073529, 75.0, 125.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.AddRes( typeof( FireRuby ), 1026254, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.RubyMace, this );
			craft.RequiresML = true;

			craft = AddCraft( 124, typeof( EmeraldMace ), 1011084, 1073530, 75.0, 125.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.EmeraldMace, this );
			craft.RequiresML = true;

			craft = AddCraft( 125, typeof( SapphireMace ), 1011084, 1073531, 75.0, 125.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.AddRes( typeof( DarkSapphire ), 1026249, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.SapphireMace, this );
			craft.RequiresML = true;

			craft = AddCraft( 126, typeof( SilverEtchedMace ), 1011084, 1073532, 75.0, 125.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 1, 1044253 );
			craft.AddRecipe( (int) SmithRecipe.SilverEtchedMace, this );
			craft.RequiresML = true;

			craft = AddCraft( 150, typeof( GargishWarHammer ), 1011084, 1097496, 34.2, 84.2, typeof( IronIngot ), 1044036, 16, 1044037 );
			craft.RequiresSA = true;

			craft = AddCraft( 151, typeof( GargishMaul ), 1011084, 1097498, 19.4, 69.4, typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.RequiresSA = true;

			craft = AddCraft( 156, typeof( GargishTessen ), 1011084, 1097508, 85.0, 135.0, typeof( IronIngot ), 1044036, 16, 1044037 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );
			craft.RequiresSA = true;

			AddCraft( 207, typeof( DiscMace ), 1011084, 1095366, 70.0, 120.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			#endregion

			#region Cannons
			if ( Expansion.HS )
			{
				// TODO (HS): 1116354 Cannons
				// 159 - 1116266 light cannonball
				// 160 - 1116267 heavy cannonball
				// 162 - 1116030 light grapeshot
				// 163 - 1116166 heavy grapeshot
				// 168 - 1095790 light ship cannon
				// 169 - 1095794 heavy ship cannon
			}
			#endregion

			#region Throwing
			AddCraft( 208, typeof( Boomerang ), 1044117, 1095359, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			AddCraft( 209, typeof( Cyclone ), 1044117, 1095364, 75.0, 125.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			AddCraft( 210, typeof( SoulGlaive ), 1044117, 1095363, 75.0, 125.0, typeof( IronIngot ), 1044036, 9, 1044037 );
			#endregion

			#region Miscellaneous
			craft = AddCraft( 83, typeof( DragonGloves ), 1011173, 1029795, 68.9, 118.9, typeof( RedScales ), 1060883, 16, 1060884 );
			craft.UseSubRes2 = true;

			craft = AddCraft( 84, typeof( DragonHelm ), 1011173, 1029797, 72.6, 122.6, typeof( RedScales ), 1060883, 20, 1060884 );
			craft.UseSubRes2 = true;

			craft = AddCraft( 85, typeof( DragonLegs ), 1011173, 1029799, 78.8, 128.8, typeof( RedScales ), 1060883, 28, 1060884 );
			craft.UseSubRes2 = true;

			craft = AddCraft( 86, typeof( DragonArms ), 1011173, 1029815, 76.3, 126.3, typeof( RedScales ), 1060883, 24, 1060884 );
			craft.UseSubRes2 = true;

			craft = AddCraft( 87, typeof( DragonChest ), 1011173, 1029793, 85.0, 135.0, typeof( RedScales ), 1060883, 36, 1060884 );
			craft.UseSubRes2 = true;

			craft = AddCraft( 376, typeof( CrushedGlass ), 1011173, 1113351, 110.0, 135.0, typeof( BlueDiamond ), 1032696, 1, 1044253 );
			craft.AddRes( typeof( GlassSword ), 1095371, 5, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 377, typeof( PowderedIron ), 1011173, 1113353, 110.0, 135.0, typeof( WhitePearl ), 1026253, 1, 1044253 );
			craft.AddRes( typeof( IronIngot ), 1044036, 20, 1044037 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;
			#endregion

			// Set the overidable material
			SetSubRes( typeof( IronIngot ), 1044022 );
			SetSubRes2( typeof( RedScales ), 1060875 );

			// Add every material you want the player to be able to chose from
			// This will overide the overidable material
			AddSubRes( typeof( IronIngot ), 1044022, 00.0, 1044036, 1044267 );
			AddSubRes( typeof( DullCopperIngot ), 1044023, 65.0, 1044036, 1044268 );
			AddSubRes( typeof( ShadowIronIngot ), 1044024, 70.0, 1044036, 1044268 );
			AddSubRes( typeof( CopperIngot ), 1044025, 75.0, 1044036, 1044268 );
			AddSubRes( typeof( BronzeIngot ), 1044026, 80.0, 1044036, 1044268 );
			AddSubRes( typeof( GoldIngot ), 1044027, 85.0, 1044036, 1044268 );
			AddSubRes( typeof( AgapiteIngot ), 1044028, 90.0, 1044036, 1044268 );
			AddSubRes( typeof( VeriteIngot ), 1044029, 95.0, 1044036, 1044268 );
			AddSubRes( typeof( ValoriteIngot ), 1044030, 99.0, 1044036, 1044268 );

			AddSubRes2( typeof( RedScales ), 1060875, 0.0, 1053137, 1044268 );
			AddSubRes2( typeof( YellowScales ), 1060876, 0.0, 1053137, 1044268 );
			AddSubRes2( typeof( BlackScales ), 1060877, 0.0, 1053137, 1044268 );
			AddSubRes2( typeof( GreenScales ), 1060878, 0.0, 1053137, 1044268 );
			AddSubRes2( typeof( WhiteScales ), 1060879, 0.0, 1053137, 1044268 );
			AddSubRes2( typeof( BlueScales ), 1060880, 0.0, 1053137, 1044268 );

			Resmelt = true;
			Repair = true;
			MarkOption = true;
			CanEnhance = true;
			Alter = true;
		}
	}

	public class ForgeAttribute : Attribute
	{
		public ForgeAttribute()
		{
		}
	}

	public class AnvilAttribute : Attribute
	{
		public AnvilAttribute()
		{
		}
	}
}