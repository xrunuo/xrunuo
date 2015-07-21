using System;
using Server;
using Server.Items;

namespace Server.Engines.Harvest
{
	public class Lumberjacking : HarvestSystem
	{
		private static Lumberjacking m_System;

		public static Lumberjacking System
		{
			get
			{
				if ( m_System == null )
					m_System = new Lumberjacking();

				return m_System;
			}
		}

		private HarvestDefinition m_Definition;

		public HarvestDefinition Definition
		{
			get { return m_Definition; }
		}

		private Lumberjacking()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region Lumberjacking
			HarvestDefinition lumber = new HarvestDefinition();

			// Resource banks are every 4x3 tiles
			lumber.BankWidth = 4;
			lumber.BankHeight = 3;

			// Every bank holds from 8 to 19 uses
			lumber.MinTotal = 8;
			lumber.MaxTotal = 19;

			// A resource bank will respawn its content every 10 to 12 minutes
			lumber.MinRespawn = TimeSpan.FromMinutes( 10.0 );
			lumber.MaxRespawn = TimeSpan.FromMinutes( 12.0 );

			// Skill checking is done on the Lumberjacking skill
			lumber.Skill = SkillName.Lumberjacking;

			// Set the list of harvestable tiles
			lumber.Tiles = m_TreeTiles;

			// Players must be within 2 tiles to harvest
			lumber.MaxRange = 2;

			// Ten logs per harvest action
			lumber.ConsumedPerHarvest = 10;
			lumber.ConsumedPerFeluccaHarvest = 20;

			// The chopping effect
			lumber.EffectActions = new int[] { 13 };
			lumber.EffectSounds = new int[] { 0x13E };
			lumber.EffectCounts = new int[] { 1 };
			lumber.EffectDelay = TimeSpan.FromSeconds( 1.6 );
			lumber.EffectSoundDelay = TimeSpan.FromSeconds( 0.9 );

			lumber.NoResourcesMessage = 500493; // There's not enough wood here to harvest.
			lumber.FailMessage = 500495; // You hack at the tree for a while, but fail to produce any useable wood.
			lumber.OutOfRangeMessage = 500446; // That is too far away.
			lumber.PackFullMessage = 500497; // You can't place any wood into your backpack!
			lumber.ToolBrokeMessage = 500499; // You broke your axe.

			res = new HarvestResource[]
				{
					new HarvestResource(  00.0, 00.0, 100.0, 1072540, typeof( Log ) ),
					new HarvestResource(  65.0, 25.0, 105.0, 1072541, typeof( OakLog ) ),
					new HarvestResource(  80.0, 40.0, 120.0, 1072542, typeof( AshLog ) ),
					new HarvestResource(  95.0, 55.0, 135.0, 1072543, typeof( YewLog ) ),
					new HarvestResource( 100.0, 60.0, 140.0, 1072544, typeof( HeartwoodLog ) ),
					new HarvestResource( 100.0, 60.0, 140.0, 1072545, typeof( BloodwoodLog ) ),
					new HarvestResource( 100.0, 60.0, 140.0, 1072546, typeof( FrostwoodLog ) )
				};

			veins = new HarvestVein[]
				{
					new HarvestVein( 49.0, res[0] ), // Ordinary Logs
					new HarvestVein( 30.0, res[1] ), // Oak
					new HarvestVein( 10.0, res[2] ), // Ash
					new HarvestVein( 05.0, res[3] ), // Yew
					new HarvestVein( 03.0, res[4] ), // Heartwood
					new HarvestVein( 02.0, res[5] ), // Bloodwood
					new HarvestVein( 01.0, res[6] ), // Frostwood
				};

			lumber.BonusResources = new BonusHarvestResource[]
				{
					new BonusHarvestResource( 0, 83.8, null, null ), // Nothing
					new BonusHarvestResource( 100, 10.0, 1072548, typeof( BarkFragment ) ),
					new BonusHarvestResource( 100, 03.0, 1072550, typeof( LuminescentFungi ) ),
					new BonusHarvestResource( 100, 02.0, 1072547, typeof( SwitchItem ) ),
					new BonusHarvestResource( 100, 01.0, 1072549, typeof( ParasiticPlant ) ),
					new BonusHarvestResource( 100, 00.1, 1072551, typeof( BrilliantAmber ) ),
					new BonusHarvestResource( 100, 00.1, 1113756, typeof( CrystalShards ), Map.TerMur ),
				};

			lumber.Resources = res;
			lumber.Veins = veins;

			lumber.RaceBonus = true;
			lumber.RandomizeVeins = true;

			m_Definition = lumber;
			Definitions.Add( lumber );
			#endregion
		}

		public override bool CheckHarvest( Mobile from, Item tool )
		{
			if ( !base.CheckHarvest( from, tool ) )
				return false;

			if ( tool.Parent != from )
			{
				from.SendLocalizedMessage( 500487 ); // The axe must be equipped for any serious wood chopping.
				return false;
			}

			return true;
		}

		public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
				return false;

			if ( tool.Parent != from )
			{
				from.SendLocalizedMessage( 500487 ); // The axe must be equipped for any serious wood chopping.
				return false;
			}

			return true;
		}

		public override void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
		{
			from.SendLocalizedMessage( 500489 ); // You can't use an axe on that.
		}

		public override void OnHarvestStarted( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			base.OnHarvestStarted( from, tool, def, toHarvest );

			from.RevealingAction();
		}

		public static void Initialize()
		{
			Array.Sort( m_TreeTiles );
		}

		#region Tile lists
		private static int[] m_TreeTiles = new int[]
			{
				0x8CCA, 0x8CCB, 0x8CCC, 0x8CCD, 0x8CD0, 0x8CD3, 0x8CD6, 0x8CD8,
				0x8CDA, 0x8CDD, 0x8CE0, 0x8CE3, 0x8CE6, 0x8CF8, 0x8CFB, 0x8CFE,
				0x8D01, 0x8D41, 0x8D42, 0x8D43, 0x8D44, 0x8D57, 0x8D58, 0x8D59,
				0x8D5A, 0x8D5B, 0x8D6E, 0x8D6F, 0x8D70, 0x8D71, 0x8D72, 0x8D84,
				0x8D85, 0x8D86, 0x92B5, 0x92B6, 0x92B7, 0x92B8, 0x92B9, 0x92BA,
				0x92BB, 0x92BC, 0x92BD,

				0x8CCE, 0x8CCF, 0x8CD1, 0x8CD2, 0x8CD4, 0x8CD5, 0x8CD7, 0x8CD9,
				0x8CDB, 0x8CDC, 0x8CDE, 0x8CDF, 0x8CE1, 0x8CE2, 0x8CE4, 0x8CE5,
				0x8CE7, 0x8CE8, 0x8CF9, 0x8CFA, 0x8CFC, 0x8CFD, 0x8CFF, 0x8D00,
				0x8D02, 0x8D03, 0x8D45, 0x8D46, 0x8D47, 0x8D48, 0x8D49, 0x8D4A,
				0x8D4B, 0x8D4C, 0x8D4D, 0x8D4E, 0x8D4F, 0x8D50, 0x8D51, 0x8D52,
				0x8D53, 0x8D5C, 0x8D5D, 0x8D5E, 0x8D5F, 0x8D60, 0x8D61, 0x8D62,
				0x8D63, 0x8D64, 0x8D65, 0x8D66, 0x8D67, 0x8D68, 0x8D69, 0x8D73,
				0x8D74, 0x8D75, 0x8D76, 0x8D77, 0x8D78, 0x8D79, 0x8D7A, 0x8D7B,
				0x8D7C, 0x8D7D, 0x8D7E, 0x8D7F, 0x8D87, 0x8D88, 0x8D89, 0x8D8A,
				0x8D8B, 0x8D8C, 0x8D8D, 0x8D8E, 0x8D8F, 0x8D90, 0x8D95, 0x8D96,
				0x8D97, 0x8D99, 0x8D9A, 0x8D9B, 0x8D9D, 0x8D9E, 0x8D9F, 0x8DA1,
				0x8DA2, 0x8DA3, 0x8DA5, 0x8DA6, 0x8DA7, 0x8DA9, 0x8DAA, 0x8DAB,
				0x92BE, 0x92BF, 0x92C0, 0x92C1, 0x92C2, 0x92C3, 0x92C4, 0x92C5,
				0x92C6, 0x92C7
			};
		#endregion
	}
}