using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.Housing.Regions;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;
using Server.Regions;

namespace Server.Items
{
	public class TreasureMap : MapItem
	{
		private int m_Level;
		private bool m_Completed;
		private Mobile m_CompletedBy;
		private Mobile m_Decoder;
		private Map m_Map;
		private Point2D m_Location;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Level
		{
			get { return m_Level; }
			set
			{
				m_Level = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Completed
		{
			get { return m_Completed; }
			set
			{
				m_Completed = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile CompletedBy
		{
			get { return m_CompletedBy; }
			set
			{
				m_CompletedBy = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Decoder
		{
			get { return m_Decoder; }
			set
			{
				m_Decoder = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map ChestMap
		{
			get { return m_Map; }
			set
			{
				m_Map = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point2D ChestLocation { get { return m_Location; } set { m_Location = value; } }

		public override Map DisplayMap { get { return ChestMap; } }

		private static Type[][] m_DefaultSpawns = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( Mongbat ), typeof( Ratman ), typeof( HeadlessOne ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( OrcishMage ), typeof( Gargoyle ), typeof( Gazer ), typeof( HellHound ), typeof( EarthElemental ) },
			new Type[]{ typeof( Lich ), typeof( OgreLord ), typeof( DreadSpider ), typeof( AirElemental ), typeof( FireElemental ) },
			new Type[]{ typeof( DreadSpider ), typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( OgreLord ) },
			new Type[]{ typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( PoisonElemental ), typeof( BloodElemental ) },
			new Type[]{ typeof( AncientWyrm ), typeof( Balron ), typeof( BloodElemental ), typeof( PoisonElemental ), typeof( Titan ), typeof( ColdDrake ), typeof( GreaterDragon ) },
		};

		private static Type[][] m_IlshenarSpawns = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( Mongbat ), typeof( Ratman ), typeof( HeadlessOne ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( EarthElemental ), typeof( Gazer ), typeof( Gargoyle ), typeof( HellHound ), typeof( OrcishMage ) },
			new Type[]{ typeof( AirElemental ), typeof( DreadSpider ), typeof( FireElemental ), typeof( Lich ), typeof( OgreLord ) },
			new Type[]{ typeof( ElderGazer ), typeof( Daemon ), typeof( DreadSpider ), typeof( LichLord ), typeof( OgreLord ) },
			new Type[]{ typeof( DarkGuardian ), typeof( ExodusOverseer ), typeof( GargoyleDestroyer ), typeof( GargoyleEnforcer ), typeof( PoisonElemental ) },
			new Type[]{ typeof( Changeling ), typeof( ExodusMinion ), typeof( GargoyleDestroyer ), typeof( GargoyleEnforcer ), typeof( Titan ) },
		};

		private static Type[][] m_MalasSpawns = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( HeadlessOne ), typeof( Mongbat ), typeof( Ratman ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( EarthElemental ), typeof( Gazer ), typeof( Gargoyle ), typeof( HellHound ), typeof( OrcishMage ) },
			new Type[]{ typeof( AirElemental ), typeof( DreadSpider ), typeof( FireElemental ), typeof( Lich ), typeof( OgreLord ) },
			new Type[]{ typeof( ElderGazer ), typeof( Daemon ), typeof( DreadSpider ), typeof( LichLord ), typeof( OgreLord ) },
			new Type[]{ typeof( LichLord ), typeof( Ravager ), typeof( WandererOfTheVoid ), typeof( Minotaur ) },
            new Type[]{ typeof( Devourer ), typeof( MinotaurScout ), typeof( MinotaurCaptain ), typeof( RottingCorpse ), typeof( WandererOfTheVoid ) },
        };

		private static Type[][] m_TokunoSpawns = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( HeadlessOne ), typeof( Mongbat ), typeof( Ratman ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( EarthElemental ), typeof( Gazer ), typeof( Gargoyle ), typeof( HellHound ), typeof( OrcishMage ) },
			new Type[]{ typeof( AirElemental ), typeof( DreadSpider ), typeof( FireElemental ), typeof( Lich ), typeof( OgreLord ) },
			new Type[]{ typeof( ElderGazer ), typeof( Daemon ), typeof( DreadSpider ), typeof( LichLord ), typeof( OgreLord ) },
			new Type[]{ typeof( FanDancer ), typeof( RevenantLion ), typeof( Ronin ), typeof( RuneBeetle ) },
			new Type[]{ typeof( Hiryu ), typeof( LadyOfTheSnow ), typeof( Oni ), typeof( RuneBeetle ), typeof( YomotsuWarrior ), typeof( YomotsuPriest ) },
        };

		private static Type[][] m_TerMurSpawns = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( ClockworkScorpion ), typeof( CorrosiveSlime ), typeof( StrongMongbat ) },
			new Type[]{ typeof( AcidSlug ), typeof( Slith ), typeof( WaterElemental ) },
			new Type[]{ typeof( LeatherWolf ), typeof( StoneSlith ), typeof( ToxicSlith ) },
			new Type[]{ typeof( Bloodworm ), typeof( Kepetch ), typeof( StoneSlith ), typeof( ToxicSlith ) },
			new Type[]{ typeof( FireAnt ), typeof( LavaElemental ), typeof( MaddeningHorror ) },
			new Type[]{ typeof( EnragedEarthElemental ), typeof( FireDaemon ), typeof( GreaterPoisonElemental ), typeof( LavaElemental ) },
		};

		public const double LootChance = 0.01; // 1% chance to appear as loot

		public static Point2D GetRandomLocation( Map map )
		{
			for ( int i = 0; i < 30; ++i )
			{
				int tx = 0, ty = 0;

				if ( map == Map.Trammel || map == Map.Felucca )
				{
					tx = Utility.RandomMinMax( 0, 4080 );
					ty = Utility.RandomMinMax( 0, 5100 );
				}
				else if ( map == Map.Ilshenar )
				{
					tx = Utility.RandomMinMax( 220, 1770 );
					ty = Utility.RandomMinMax( 200, 1415 );
				}
				else if ( map == Map.Malas )
				{
					tx = Utility.RandomMinMax( 600, 2150 );
					ty = Utility.RandomMinMax( 70, 1910 );
				}
				else if ( map == Map.Tokuno )
				{
					tx = Utility.RandomMinMax( 90, 1410 );
					ty = Utility.RandomMinMax( 20, 1400 );
				}
				else if ( map == Map.TerMur )
				{
					tx = Utility.RandomMinMax( 300, 1220 );
					ty = Utility.RandomMinMax( 2800, 4050 );
				}

				// First, check for land tile to be valid, for most efficiency
				if ( !IsValidLandTile( map, tx, ty ) )
					continue;

				var loc = new Point3D( tx, ty, map.GetAverageZ( tx, ty ) );

				var region = Region.Find( loc, map );

				if ( region.IsPartOf<GuardedRegion>() || region.IsPartOf<DungeonRegion>() || region.IsPartOf<HouseRegion>() )
					continue;

				if ( map.CanSpawnMobile( loc ) )
					return new Point2D( tx, ty );
			}

			return Point2D.Zero;
		}

		private static bool IsValidLandTile( Map map, int tx, int ty )
		{
			Tile tile = map.Tiles.GetLandTile( tx, ty );

			if ( tile.ID == 1 )
				return false;
			if ( tile.ID >= 26 && tile.ID <= 50 )
				return false;
			if ( tile.ID >= 68 && tile.ID <= 111 )
				return false;
			if ( tile.ID >= 141 && tile.ID <= 171 )
				return false;
			if ( tile.ID >= 220 && tile.ID <= 231 )
				return false;
			if ( tile.ID >= 236 && tile.ID <= 239 )
				return false;
			if ( tile.ID >= 244 && tile.ID <= 247 )
				return false;
			if ( tile.ID >= 252 && tile.ID <= 255 )
				return false;
			if ( tile.ID >= 260 && tile.ID <= 263 )
				return false;
			if ( tile.ID >= 268 && tile.ID <= 279 )
				return false;
			if ( tile.ID >= 286 && tile.ID <= 297 )
				return false;
			if ( tile.ID >= 302 && tile.ID <= 324 )
				return false;
			if ( tile.ID >= 380 && tile.ID <= 394 )
				return false;
			if ( tile.ID >= 441 && tile.ID <= 475 )
				return false;
			if ( tile.ID >= 500 && tile.ID <= 580 )
				return false;
			if ( tile.ID >= 586 && tile.ID <= 621 )
				return false;
			if ( tile.ID >= 662 && tile.ID <= 799 )
				return false;
			if ( tile.ID >= 1001 && tile.ID <= 1308 )
				return false;
			if ( tile.ID >= 1325 && tile.ID <= 1340 )
				return false;
			if ( tile.ID >= 1367 && tile.ID <= 1378 )
				return false;
			if ( tile.ID >= 1661 && tile.ID <= 1696 )
				return false;
			if ( tile.ID >= 1741 && tile.ID <= 1757 )
				return false;
			if ( tile.ID >= 1771 && tile.ID <= 2495 )
				return false;
			if ( tile.ID >= 2540 )
				return false;

			return true;
		}

		private static BaseCreature Spawn( int level, Map map, Point3D p )
		{
			var spawnTypes = GetSpawnTypes( map );

			if ( level >= 0 && level < spawnTypes.Length )
			{
				BaseCreature bc;

				try
				{
					bc = (BaseCreature) Activator.CreateInstance( spawnTypes[level][Utility.Random( spawnTypes[level].Length )] );
				}
				catch
				{
					return null;
				}

				bc.Home = p;
				bc.RangeHome = 5;
				bc.Title = "(Guardian)";
				bc.Tamable = false;

				return bc;
			}

			return null;
		}

		private static Type[][] GetSpawnTypes( Map map )
		{
			if ( map == Map.Ilshenar )
				return m_IlshenarSpawns;
			if ( map == Map.Malas )
				return m_MalasSpawns;
			if ( map == Map.Tokuno )
				return m_TokunoSpawns;
			if ( map == Map.TerMur )
				return m_TerMurSpawns;

			return m_DefaultSpawns;
		}

		public static BaseCreature Spawn( int level, Point3D p, Map map, Mobile target )
		{
			if ( map == null )
				return null;

			BaseCreature bc = Spawn( level, map, p );

			if ( bc != null )
			{
				bool spawned = false;

				Point3D loc = GetRandomSpawnLocation( p, map );

				if ( loc != Point3D.Zero )
				{
					bc.MoveToWorld( p, map );
					spawned = true;
				}

				if ( !spawned )
				{
					bc.Delete();
					return null;
				}

				if ( target != null )
				{
					bc.Combatant = target;
				}

				return bc;
			}

			return null;
		}

		public static Point3D GetRandomSpawnLocation( Point3D p, Map map )
		{
			for ( int i = 0; i < 10; ++i )
			{
				int x = p.X - 3 + Utility.Random( 7 );
				int y = p.Y - 3 + Utility.Random( 7 );

				if ( map.CanSpawnMobile( x, y, p.Z ) )
				{
					return new Point3D( x, y, p.Z );
				}
				else
				{
					int z = map.GetAverageZ( x, y );

					if ( map.CanSpawnMobile( x, y, z ) )
						return new Point3D( x, y, z );
				}
			}

			return Point3D.Zero;
		}

		private static Map GetRandomMap()
		{
			return Utility.RandomList( Map.Felucca, Map.Trammel, Map.Ilshenar, Map.Malas, Map.Tokuno, Map.TerMur );
		}

		[Constructable]
		public TreasureMap( int level )
			: this( level, GetRandomMap() )
		{
		}

		[Constructable]
		public TreasureMap( int level, Map map )
			: this( level, map, GetRandomLocation( map ) )
		{
		}

		[Constructable]
		public TreasureMap( int level, Map map, Point2D location )
		{
			m_Level = level;
			m_Map = map;
			m_Location = location;

			Width = 300;
			Height = 300;

			RecalculateDisplayMap();
		}

		private void RecalculateDisplayMap()
		{
			const int width = 600;
			const int height = 600;

			int x1 = m_Location.X - Utility.RandomMinMax( width / 4, ( width / 4 ) * 3 );
			int y1 = m_Location.Y - Utility.RandomMinMax( height / 4, ( height / 4 ) * 3 );

			int x2 = x1 + width;
			int y2 = y1 + height;

			EnsureInBounds( m_Map, ref x1, ref x2, ref y1, ref y2 );

			if ( x2 > x1 + width )
				x1 = x2 - width;

			if ( y2 > y1 + height )
				y1 = y2 - height;

			Bounds = new Rectangle2D( x1, y1, x2 - x1, y2 - y1 );
			Protected = true;

			AddWorldPin( m_Location.X, m_Location.Y );
		}

		public TreasureMap( Serial serial )
			: base( serial )
		{
		}

		private static bool HasDiggingTool( Mobile m )
		{
			if ( m.Backpack == null )
				return false;

			List<BaseHarvestTool> items = m.Backpack.FindItemsByType<BaseHarvestTool>();

			foreach ( BaseHarvestTool tool in items )
			{
				if ( tool.HarvestSystem == Engines.Harvest.Mining.System )
					return true;
			}

			return false;
		}

		public void OnBeginDig( Mobile from )
		{
			if ( m_Completed )
			{
				from.SendLocalizedMessage( 503028 ); // The treasure for this map has already been found.
			}
			/*
			else if ( from != m_Decoder )
			{
				from.SendLocalizedMessage( 503016 ); // Only the person who decoded this map may actually dig up the treasure.
			}
			*/
			else if ( m_Decoder != from && !HasRequiredSkill( from ) )
			{
				from.SendLocalizedMessage( 503031 ); // You did not decode this map and have no clue where to look for the treasure.
			}
			else if ( !from.CanBeginAction( typeof( TreasureMap ) ) )
			{
				from.SendLocalizedMessage( 503020 ); // You are already digging treasure.
			}
			else if ( from.Map != this.m_Map )
			{
				from.SendLocalizedMessage( 1010479 ); // You seem to be in the right place, but may be on the wrong facet!
			}
			else
			{
				from.SendLocalizedMessage( 503033 ); // Where do you wish to dig?
				from.Target = new DigTarget( this );
			}
		}

		private class DigTarget : Target
		{
			private TreasureMap m_Map;

			public DigTarget( TreasureMap map )
				: base( 6, true, TargetFlags.None )
			{
				m_Map = map;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Map.Deleted )
					return;

				Map map = m_Map.Map;

				if ( m_Map.m_Completed )
				{
					from.SendLocalizedMessage( 503028 ); // The treasure for this map has already been found.
				}
				else if ( from != m_Map.m_Decoder )
				{
					from.SendLocalizedMessage( 503016 ); // Only the person who decoded this map may actually dig up the treasure.
				}
				else if ( m_Map.m_Decoder != from && !m_Map.HasRequiredSkill( from ) )
				{
					from.SendLocalizedMessage( 503031 ); // You did not decode this map and have no clue where to look for the treasure.
					return;
				}
				else if ( !from.CanBeginAction( typeof( TreasureMap ) ) )
				{
					from.SendLocalizedMessage( 503020 ); // You are already digging treasure.
				}
				else if ( !HasDiggingTool( from ) )
				{
					from.SendMessage( "You must have a digging tool to dig for treasure." );
				}
				else if ( from.Map != map )
				{
					from.SendLocalizedMessage( 1010479 ); // You seem to be in the right place, but may be on the wrong facet!
				}
				else
				{
					IPoint3D p = targeted as IPoint3D;

					Point3D targ3D;
					if ( p is Item )
						targ3D = ( (Item) p ).GetWorldLocation();
					else
						targ3D = new Point3D( p );

					int maxRange;
					double skillValue = from.Skills[SkillName.Mining].Value;

					if ( skillValue >= 100.0 )
						maxRange = 4;
					else if ( skillValue >= 81.0 )
						maxRange = 3;
					else if ( skillValue >= 51.0 )
						maxRange = 2;
					else
						maxRange = 1;

					Point2D loc = m_Map.m_Location;
					int x = loc.X, y = loc.Y;

					Point3D chest3D0 = new Point3D( loc, 0 );

					if ( targ3D.InRange( chest3D0, maxRange ) )
					{
						if ( from.Location.X == x && from.Location.Y == y )
						{
							from.SendLocalizedMessage( 503030 ); // The chest can't be dug up because you are standing on top of it.
						}
						else if ( map != null )
						{
							int z = map.GetAverageZ( x, y );

							if ( !map.CanFit( x, y, z, 16, true, true ) )
							{
								from.SendLocalizedMessage( 503021 ); // You have found the treasure chest but something is keeping it from being dug up.
							}
							else if ( from.BeginAction( typeof( TreasureMap ) ) )
							{
								new DigTimer( from, m_Map, new Point3D( x, y, z ), map ).Start();
							}
							else
							{
								from.SendLocalizedMessage( 503020 ); // You are already digging treasure.
							}
						}
					}
					else if ( m_Map.Level > 0 )
					{
						if ( targ3D.InRange( chest3D0, 8 ) ) // We're close, but not quite
						{
							from.SendLocalizedMessage( 503032 ); // You dig and dig but no treasure seems to be here.
						}
						else
						{
							from.SendLocalizedMessage( 503035 ); // You dig and dig but fail to find any treasure.
						}
					}
					else
					{
						if ( targ3D.InRange( chest3D0, 8 ) ) // We're close, but not quite
						{
							from.SendAsciiMessage( 0x44, "The treasure chest is very close!" );
						}
						else
						{
							Direction dir = Utility.GetDirection( targ3D, chest3D0 );

							string sDir;
							switch ( dir )
							{
								case Direction.North:
									sDir = "north";
									break;
								case Direction.Right:
									sDir = "northeast";
									break;
								case Direction.East:
									sDir = "east";
									break;
								case Direction.Down:
									sDir = "southeast";
									break;
								case Direction.South:
									sDir = "south";
									break;
								case Direction.Left:
									sDir = "southwest";
									break;
								case Direction.West:
									sDir = "west";
									break;
								default:
									sDir = "northwest";
									break;
							}

							from.SendAsciiMessage( 0x44, "Try looking for the treasure chest more to the {0}.", sDir );
						}
					}
				}
			}
		}

		private class DigTimer : Timer
		{
			private Mobile m_From;
			private TreasureMap m_TreasureMap;

			private Point3D m_Location;
			private Map m_Map;

			private TreasureChestDirt m_Dirt1;
			private TreasureChestDirt m_Dirt2;
			private TreasureMapChest m_Chest;

			private int m_Count;

			private DateTime m_NextSkillTime;
			private DateTime m_NextSpellTime;
			private DateTime m_NextActionTime;
			private DateTime m_LastMoveTime;

			public DigTimer( Mobile from, TreasureMap treasureMap, Point3D location, Map map )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				m_From = from;
				m_TreasureMap = treasureMap;

				m_Location = location;
				m_Map = map;

				m_NextSkillTime = from.NextSkillTime;
				m_NextSpellTime = from.NextSpellTime;
				m_NextActionTime = from.NextActionTime;
				m_LastMoveTime = from.LastMoveTime;

			}

			private void Terminate()
			{
				Stop();
				m_From.EndAction( typeof( TreasureMap ) );

				if ( m_Chest != null )
					m_Chest.Delete();

				if ( m_Dirt1 != null )
				{
					m_Dirt1.Delete();
					m_Dirt2.Delete();
				}
			}

			protected override void OnTick()
			{
				if ( m_NextSkillTime != m_From.NextSkillTime || m_NextSpellTime != m_From.NextSpellTime || m_NextActionTime != m_From.NextActionTime )
				{
					Terminate();
					return;
				}

				if ( m_LastMoveTime != m_From.LastMoveTime )
				{
					m_From.SendLocalizedMessage( 503023 ); // You cannot move around while digging up treasure. You will need to start digging anew.
					Terminate();
					return;
				}

				int z = ( m_Chest != null ) ? m_Chest.Z + m_Chest.ItemData.Height : int.MinValue;
				int height = 16;

				if ( z > m_Location.Z )
					height -= ( z - m_Location.Z );
				else
					z = m_Location.Z;

				if ( !m_Map.CanFit( m_Location.X, m_Location.Y, z, height, true, true, false ) )
				{
					m_From.SendLocalizedMessage( 503024 ); // You stop digging because something is directly on top of the treasure chest.
					Terminate();
					return;
				}

				m_Count++;

				m_From.RevealingAction();
				m_From.Direction = m_From.GetDirectionTo( m_Location );

				if ( m_Count > 1 && m_Dirt1 == null )
				{
					m_Dirt1 = new TreasureChestDirt();
					m_Dirt1.MoveToWorld( m_Location, m_Map );

					m_Dirt2 = new TreasureChestDirt();
					m_Dirt2.MoveToWorld( new Point3D( m_Location.X, m_Location.Y - 1, m_Location.Z ), m_Map );
				}

				if ( m_Count == 5 )
				{
					m_Dirt1.Turn1();
				}
				else if ( m_Count == 10 )
				{
					m_Dirt1.Turn2();
					m_Dirt2.Turn2();
				}
				else if ( m_Count > 10 )
				{
					if ( m_Chest == null )
					{
						m_Chest = new TreasureMapChest( m_From, m_TreasureMap.Level, true, m_Map );
						m_Chest.MoveToWorld( new Point3D( m_Location.X, m_Location.Y, m_Location.Z - 15 ), m_Map );
					}
					else
					{
						m_Chest.Z++;
					}

					Effects.PlaySound( m_Chest, m_Map, 0x33B );
				}

				if ( m_Chest != null && m_Chest.Location.Z >= m_Location.Z )
				{
					Stop();
					m_From.EndAction( typeof( TreasureMap ) );

					m_TreasureMap.Completed = true;
					m_TreasureMap.CompletedBy = m_From;

					m_Chest.Temporary = false;

					SpawnGuardians();
				}
				else
				{
					if ( m_From.Body.IsHuman && !m_From.Mounted )
						m_From.Animate( 11, 5, 1, true, false, 0 );

					new SoundTimer( m_From, 0x125 + ( m_Count % 2 ) ).Start();
				}
			}

			public void SpawnGuardians()
			{
				int spawnCount = GetSpawnCount();

				for ( int i = 0; i < spawnCount; ++i )
				{
					BaseCreature bc = Spawn( m_TreasureMap.Level, m_Chest.Location, m_Chest.Map, null );

					if ( bc != null )
						m_Chest.Guardians.Add( bc );
				}

				new ReturnToHomeTimer( m_Chest ).Start();
			}

			private int GetSpawnCount()
			{
				switch ( m_TreasureMap.Level )
				{
					case 0:
						return 3;
					default:
						return Utility.RandomMinMax( 4, 8 );
				}
			}

			private class ReturnToHomeTimer : Timer
			{
				private TreasureMapChest m_Chest;

				public ReturnToHomeTimer( TreasureMapChest chest )
					: base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
				{
					m_Chest = chest;
				}

				protected override void OnTick()
				{
					if ( m_Chest.Deleted || m_Chest.Guardians.Count == 0 )
					{
						Stop();
					}
					else
					{
						foreach ( var bc in m_Chest.Guardians )
						{
							if ( !bc.InRange( m_Chest, 25 ) )
								ReturnToHome( bc );
						}
					}
				}

				private void ReturnToHome( Mobile m )
				{
					var loc = GetRandomSpawnLocation( m_Chest.Location, m_Chest.Map );

					if ( loc != Point3D.Zero )
						m.MoveToWorld( loc, m_Chest.Map );
				}
			}

			private class SoundTimer : Timer
			{
				private Mobile m_From;
				private int m_SoundID;

				public SoundTimer( Mobile from, int soundId )
					: base( TimeSpan.FromSeconds( 0.9 ) )
				{
					m_From = from;
					m_SoundID = soundId;

				}

				protected override void OnTick()
				{
					m_From.PlaySound( m_SoundID );
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( GetWorldLocation(), 2 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			if ( !m_Completed && m_Decoder == null )
				Decode( from );
			else
				DisplayTo( from );
		}

		private double GetRequiredSkillLevel()
		{
			switch ( m_Level )
			{
				case 1:
					return 27.0;
				case 2:
					return 71.0;
				case 3:
					return 81.0;
				case 4:
					return 91.0;
				case 5:
					return 100.0;
				case 6:
					return 100.0;
				case 7:
					return 100.0;

				default:
					return 0.0;
			}
		}

		private bool HasRequiredSkill( Mobile from )
		{
			return ( from.Skills[SkillName.Cartography].Value >= GetRequiredSkillLevel() );
		}

		private void Decode( Mobile from )
		{
			if ( m_Completed || m_Decoder != null )
				return;

			double reqSkill = GetRequiredSkillLevel();

			if ( from.Skills[SkillName.Cartography].Value < reqSkill )
			{
				from.SendLocalizedMessage( 503013 ); // The map is too difficult to attempt to decode.
				return;
			}

			double minSkill = reqSkill - 30.0;
			double maxSkill = reqSkill + 30.0;

			if ( !from.CheckSkill( SkillName.Cartography, minSkill, maxSkill ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503018 ); // You fail to make anything of the map.
				return;
			}

			from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503019 ); // You successfully decode a treasure map!
			Decoder = from;

			DisplayTo( from );
		}

		public override void DisplayTo( Mobile from )
		{
			if ( m_Completed )
			{
				SendLocalizedMessageTo( from, 503014 ); // This treasure hunt has already been completed.
			}
			else if ( m_Decoder != from && !HasRequiredSkill( from ) )
			{
				from.SendLocalizedMessage( 503031 ); // You did not decode this map and have no clue where to look for the treasure.
				return;
			}
			else
			{
				SendLocalizedMessageTo( from, 503017 ); // The treasure is marked by the red pin. Grab a shovel and go dig it up!
			}

			from.PlaySound( 0x249 );
			base.DisplayTo( from );
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( !m_Completed )
			{
				if ( m_Decoder == null )
				{
					list.Add( new DecodeMapEntry( this ) );
				}
				else
				{
					bool digTool = HasDiggingTool( from );

					list.Add( new OpenMapEntry( this ) );
					list.Add( new DigEntry( this, digTool ) );
				}
			}
		}

		private class DecodeMapEntry : ContextMenuEntry
		{
			private TreasureMap m_Map;

			public DecodeMapEntry( TreasureMap map )
				: base( 6147, 2 )
			{
				m_Map = map;
			}

			public override void OnClick()
			{
				if ( !m_Map.Deleted )
					m_Map.Decode( Owner.From );
			}
		}

		private class OpenMapEntry : ContextMenuEntry
		{
			private TreasureMap m_Map;

			public OpenMapEntry( TreasureMap map )
				: base( 6150, 2 )
			{
				m_Map = map;
			}

			public override void OnClick()
			{
				if ( !m_Map.Deleted )
					m_Map.DisplayTo( Owner.From );
			}
		}

		private class DigEntry : ContextMenuEntry
		{
			private TreasureMap m_Map;

			public DigEntry( TreasureMap map, bool enabled )
				: base( 6148, 2 )
			{
				m_Map = map;

				if ( !enabled )
					this.Flags |= CMEFlags.Disabled;
			}

			public override void OnClick()
			{
				if ( m_Map.Deleted )
					return;

				Mobile from = Owner.From;

				if ( HasDiggingTool( from ) )
					m_Map.OnBeginDig( from );
				else
					from.SendMessage( "You must have a digging tool to dig for treasure." );
			}
		}

		public override int LabelNumber
		{
			get
			{
				if ( m_Decoder != null )
				{
					if ( m_Level == 7 )
						return 1116773; // a diabolically drawn treasure map
					else if ( m_Level == 6 )
						return 1063453; // an ingeniously drawn treasure map
					else
						return 1041516 + m_Level; // a [...] drawn treasure map
				}
				else
				{
					if ( m_Level == 7 )
						return 1116790; // a tattered, diabolically drawn treasure map
					else if ( m_Level == 6 )
						return 1063452; // a tattered, ingeniously drawn treasure map
					else
						return 1041510 + m_Level; // a tattered, [...] drawn treasure map
				}
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Map == Map.Felucca )
				list.Add( 1041502 ); // for somewhere in Felucca
			else if ( m_Map == Map.Trammel )
				list.Add( 1041503 ); // for somewhere in Trammel
			else if ( m_Map == Map.Ilshenar )
				list.Add( 1060850 ); // for somewhere in Ilshenar
			else if ( m_Map == Map.Malas )
				list.Add( 1060851 ); // for somewhere in Malas
			else if ( m_Map == Map.Tokuno )
				list.Add( 1115645 ); // for somewhere in Tokuno Islands
			else if ( m_Map == Map.TerMur )
				list.Add( 1115646 ); // for somewhere in Ter Mur

			if ( m_Completed )
				list.Add( 1041507, m_CompletedBy == null ? "someone" : m_CompletedBy.Name ); // completed by ~1_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 );

			writer.Write( (Mobile) m_CompletedBy );

			writer.Write( m_Level );
			writer.Write( m_Completed );
			writer.Write( m_Decoder );
			writer.Write( m_Map );
			writer.Write( m_Location );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				case 2:
				case 1:
					{
						m_CompletedBy = reader.ReadMobile();

						goto case 0;
					}
				case 0:
					{
						m_Level = (int) reader.ReadInt();
						m_Completed = reader.ReadBool();
						m_Decoder = reader.ReadMobile();
						m_Map = reader.ReadMap();
						m_Location = reader.ReadPoint2D();

						if ( version == 0 && m_Completed )
							m_CompletedBy = m_Decoder;

						break;
					}
			}

			if ( version < 2 )
			{
				if ( m_Level < 1 )
					m_Level = 1;

				m_Map = GetRandomMap();
				m_Location = GetRandomLocation( m_Map );
			}

			if ( version < 3 )
			{
				ClearPins();
				RecalculateDisplayMap();
			}
		}
	}

	public class TreasureChestDirt : Item
	{
		public TreasureChestDirt()
			: base( 0x912 )
		{
			Movable = false;

			Timer.DelayCall( TimeSpan.FromMinutes( 2.0 ), new TimerCallback( Delete ) );
		}

		public TreasureChestDirt( Serial serial )
			: base( serial )
		{
		}

		public void Turn1()
		{
			this.ItemID = 0x913;
		}

		public void Turn2()
		{
			this.ItemID = 0x914;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			Delete();
		}
	}
}
