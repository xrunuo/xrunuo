using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Events;

namespace Server.Engines.CannedEvil
{
	public class ChampionSpawn : Item
	{
		public static void Initialize()
		{
			foreach ( Item item in World.Items )
			{
				if ( item is ChampionSpawn )
				{
					ChampionSpawn spawn = (ChampionSpawn) item;

					EventSink.Login += new LoginEventHandler( spawn.OnLogin );
					EventSink.Logout += new LogoutEventHandler( spawn.OnLogout );
				}
			}
		}

		private bool m_Active;
		private bool m_RandomizeType;
		private ChampionSpawnType m_Type;
		private List<Mobile> m_Creatures;
		private List<Item> m_RedSkulls;
		private List<Item> m_WhiteSkulls;
		private ChampionPlatform m_Platform;
		private ChampionAltar m_Altar;
		private int m_Kills;
		private Mobile m_Champion;

		//private int m_SpawnRange;
		private Rectangle2D m_SpawnArea;
		private ChampionSpawnRegion m_Region;

		private TimeSpan m_ExpireDelay;
		private DateTime m_ExpireTime;

		private TimeSpan m_RestartDelay;
		private DateTime m_RestartTime;

		private TimeSpan m_KickDelay;
		private DateTime m_KickTime;

		private Timer m_Timer, m_RestartTimer;

		private IdolOfTheChampion m_Idol;

		private bool m_HasBeenAdvanced;

		private Dictionary<Mobile, int> m_DamageEntries;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasBeenAdvanced
		{
			get { return m_HasBeenAdvanced; }
			set { m_HasBeenAdvanced = value; }
		}

		[Constructable]
		public ChampionSpawn()
			: base( 0xBD2 )
		{
			Movable = false;
			Visible = false;

			m_Creatures = new List<Mobile>();
			m_RedSkulls = new List<Item>();
			m_WhiteSkulls = new List<Item>();

			m_Platform = new ChampionPlatform( this );
			m_Altar = new ChampionAltar( this );
			m_Idol = new IdolOfTheChampion( this );

			m_ExpireDelay = TimeSpan.FromMinutes( 10.0 );
			m_RestartDelay = TimeSpan.FromMinutes( 5.0 );
			m_KickDelay = TimeSpan.FromMinutes( 5.0 );

			m_DamageEntries = new Dictionary<Mobile, int>();

			// Set initial spawn area (50 by default)
			SetSpawnArea( 50 );
		}

		public void SetSpawnArea( int range )
		{
			SpawnArea = new Rectangle2D( new Point2D( X - range, Y - range ), new Point2D( X + range, Y + range ) );
		}

		public void UpdateRegion()
		{
			if ( m_Region != null )
				m_Region.Unregister();

			if ( !Deleted && this.Map != Map.Internal )
			{
				m_Region = new ChampionSpawnRegion( this );
				m_Region.Register();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RandomizeType
		{
			get { return m_RandomizeType; }
			set { m_RandomizeType = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Kills
		{
			get
			{
				return m_Kills;
			}
			set
			{
				m_Kills = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Rectangle2D SpawnArea
		{
			get
			{
				return m_SpawnArea;
			}
			set
			{
				m_SpawnArea = value;
				InvalidateProperties();
				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan RestartDelay
		{
			get
			{
				return m_RestartDelay;
			}
			set
			{
				m_RestartDelay = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime RestartTime
		{
			get
			{
				return m_RestartTime;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan ExpireDelay
		{
			get
			{
				return m_ExpireDelay;
			}
			set
			{
				m_ExpireDelay = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime ExpireTime
		{
			get
			{
				return m_ExpireTime;
			}
			set
			{
				m_ExpireTime = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan KickDelay
		{
			get
			{
				return m_KickDelay;
			}
			set
			{
				m_KickDelay = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime KickTime
		{
			get
			{
				return m_KickTime;
			}
			set
			{
				m_KickTime = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ChampionSpawnType Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get
			{
				return m_Active;
			}
			set
			{
				if ( value )
					Start();
				else
					Stop();

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Champion
		{
			get
			{
				return m_Champion;
			}
			set
			{
				m_Champion = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Level
		{
			get
			{
				return m_RedSkulls.Count;
			}
			set
			{
				for ( int i = m_RedSkulls.Count - 1; i >= value; --i )
				{
					m_RedSkulls[i].Delete();
					m_RedSkulls.RemoveAt( i );
				}

				for ( int i = m_RedSkulls.Count; i < value; ++i )
				{
					Item skull = new Item( 0x1854 );

					skull.Hue = 0x26;
					skull.Movable = false;
					skull.Light = LightType.Circle150;

					skull.MoveToWorld( GetRedSkullLocation( i ), Map );

					m_RedSkulls.Add( skull );
				}

				InvalidateProperties();
			}
		}

		public int MaxKills
		{
			get
			{
				return 250 - ( Level * 12 );
			}
		}

		public bool IsChampionSpawn( Mobile m )
		{
			return m_Creatures.Contains( m );
		}

		public void SetWhiteSkullCount( int val )
		{
			for ( int i = m_WhiteSkulls.Count - 1; i >= val; --i )
			{
				m_WhiteSkulls[i].Delete();
				m_WhiteSkulls.RemoveAt( i );
			}

			for ( int i = m_WhiteSkulls.Count; i < val; ++i )
			{
				Item skull = new Item( 0x1854 );

				skull.Movable = false;
				skull.Light = LightType.Circle150;

				skull.MoveToWorld( GetWhiteSkullLocation( i ), Map );

				m_WhiteSkulls.Add( skull );

				Effects.PlaySound( skull.Location, skull.Map, 0x29 );
				Effects.SendLocationEffect( new Point3D( skull.X + 1, skull.Y + 1, skull.Z ), skull.Map, 0x3728, 10 );
			}
		}

		public void Start()
		{
			Start( true );
		}

		public void Start( bool invokeEvent )
		{
			if ( m_Active || Deleted )
				return;

			m_Active = true;

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = new SliceTimer( this );
			m_Timer.Start();

			if ( m_RestartTimer != null )
				m_RestartTimer.Stop();

			m_RestartTimer = null;

			if ( m_Altar != null )
				m_Altar.Hue = 0;

			if ( invokeEvent )
				OnSpawnActivated();
		}

		public void Stop()
		{
			if ( !m_Active || Deleted )
				return;

			m_Active = false;

			// We must despawn all the creatures.
			if ( m_Creatures != null )
			{
				for ( int i = 0; i < m_Creatures.Count; ++i )
					m_Creatures[i].Delete();

				m_Creatures.Clear();
			}

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;

			if ( m_RestartTimer != null )
				m_RestartTimer.Stop();

			m_RestartTimer = null;

			if ( m_Altar != null )
				m_Altar.Hue = 0x455;

			OnSpawnDeactivated();
		}

		public void BeginRestart( TimeSpan ts )
		{
			if ( m_RestartTimer != null )
				m_RestartTimer.Stop();

			m_RestartTime = DateTime.UtcNow + ts;

			m_RestartTimer = new RestartTimer( this, ts );
			m_RestartTimer.Start();
		}

		public void EndRestart()
		{
			if ( RandomizeType )
			{
				switch ( Utility.Random( 5 ) )
				{
					case 0:
						Type = ChampionSpawnType.VerminHorde;
						break;
					case 1:
						Type = ChampionSpawnType.UnholyTerror;
						break;
					case 2:
						Type = ChampionSpawnType.ColdBlood;
						break;
					case 3:
						Type = ChampionSpawnType.Abyss;
						break;
					case 4:
						Type = ChampionSpawnType.Arachnid;
						break;
				}
			}

			m_HasBeenAdvanced = false;

			Start();
		}

		public void OnSlice()
		{
			if ( !m_Active || Deleted )
				return;

			if ( m_Champion != null )
			{
				if ( m_Champion.Deleted )
				{
					RegisterDamageTo( m_Champion );

					if ( m_Champion is BaseChampion )
						AwardArtifact( ( (BaseChampion) m_Champion ).GetArtifact() );

					m_DamageEntries.Clear();

					if ( m_Altar != null )
					{
						m_Altar.Hue = 0x455;

						if ( m_Altar.Map == Map.Felucca )
							new StarRoomGate( true, m_Altar.Location, m_Altar.Map );
					}

					m_Champion = null;
					Stop();

					m_HasBeenAdvanced = false;

					BeginRestart( m_RestartDelay );
				}
				else
				{
					if ( m_Champion.Region != m_Region )
						m_Champion.MoveToWorld( new Point3D( X, Y, Z - 15 ), Map );
				}
			}
			else
			{
				int kills = m_Kills;

				for ( int i = 0; i < m_Creatures.Count; ++i )
				{
					Mobile creature = m_Creatures[i];

					if ( creature.Deleted )
					{
						if ( creature.Corpse != null && !creature.Corpse.Deleted )
							( (Corpse) creature.Corpse ).BeginDecay( TimeSpan.FromMinutes( 1.0 ) );

						m_Creatures.RemoveAt( i );
						--i;
						++m_Kills;

						RegisterDamageTo( creature );

						Mobile killer = creature.FindMostRecentDamager( false );

						if ( killer is BaseCreature )
							killer = ( (BaseCreature) killer ).GetMaster();

						if ( killer is PlayerMobile && killer.Alive && killer.Region == m_Region )
						{
							if ( creature.Fame > Utility.Random( 1000000 ) )
							{
								if ( Map == Map.Felucca && Utility.RandomBool() )
								{
									PowerScroll ps = PowerScroll.CreateRandomNoCraft( 5, 5 );

									ps.LootType = LootType.Blessed;

									killer.SendLocalizedMessage( 1049524 ); // You have received a scroll of power!

									killer.AddToBackpack( ps );
								}
								else
								{
									ScrollOfTranscendence sot;

									if ( this.Map.Rules == MapRules.FeluccaRules )
										sot = ScrollOfTranscendence.CreateRandom( 6, 10 );
									else
										sot = ScrollOfTranscendence.CreateRandom( 1, 5 );

									killer.SendLocalizedMessage( 1094936 ); // You have received a Scroll of Transcendence!

									killer.AddToBackpack( sot );
								}
							}

							int mobSubLevel = GetSubLevelFor( creature ) + 1;

							if ( mobSubLevel >= 0 )
							{
								bool gainedPath = false;

								int pointsToGain = mobSubLevel * 40;

								if ( VirtueHelper.Award( killer, VirtueName.Valor, pointsToGain, ref gainedPath ) )
								{
									if ( gainedPath )
										killer.SendLocalizedMessage( 1054032 ); // You have gained a path in Valor!
									else
										killer.SendLocalizedMessage( 1054030 ); // You have gained in Valor!

									// No delay on Valor gains
								}
							}

							#region Champion Monster Titles
							int type = -1;

							switch ( m_Type )
							{
								case ChampionSpawnType.ColdBlood:
									type = 0;
									break;
								case ChampionSpawnType.ForestLord:
									type = 1;
									break;
								case ChampionSpawnType.Arachnid:
									type = 2;
									break;
								case ChampionSpawnType.Abyss:
									type = 3;
									break;
								case ChampionSpawnType.VerminHorde:
									type = 4;
									break;
								case ChampionSpawnType.UnholyTerror:
									type = 5;
									break;
								case ChampionSpawnType.SleepingDragon:
									type = 6;
									break;
								case ChampionSpawnType.Corrupt:
									type = 7;
									break;
								case ChampionSpawnType.Glade:
									type = 8;
									break;
								case ChampionSpawnType.Unliving:
									type = 9;
									break;
								case ChampionSpawnType.Pit:
									type = 10;
									break;
							}

							BaseCreature bc = creature as BaseCreature;

							if ( bc != null && type != -1 )
								bc.CalculateTitlesScore( (PlayerMobile) killer, bc.SpawnLevel, type );
							#endregion
						}
					}
				}

				// Only really needed once.
				if ( m_Kills > kills )
					InvalidateProperties();

				double n = m_Kills / (double) MaxKills;
				int p = (int) ( n * 100 );

				if ( p >= 90 )
					AdvanceLevel();
				else if ( p > 0 )
					SetWhiteSkullCount( p / 20 );

				if ( DateTime.UtcNow >= m_ExpireTime )
					Expire();

				if ( DateTime.UtcNow >= m_KickTime )
					KickGhosts();

				Respawn();
			}
		}

		public void KickGhosts()
		{
			// Ghosts without corpses will be automatically exorcised periodically

			foreach ( Mobile m in m_Region.GetMobiles() )
			{
				if ( IsGhost( m ) )
					m.Location = ExorcismSpell.GetNearestShrine( m );
			}

			m_KickTime = DateTime.UtcNow + m_KickDelay;
		}

		public static bool IsGhost( Mobile m )
		{
			if ( !m.Player || m.Alive )
				return false;

			Corpse c = m.Corpse as Corpse;
			Map map = m.Map;

			if ( c != null && !c.Deleted && map != null && c.Map == map )
			{
				if ( SpellHelper.IsAnyT2A( map, c.Location ) && SpellHelper.IsAnyT2A( map, m.Location ) )
					return false;

				if ( m.Region.IsPartOf( typeof( DungeonRegion ) ) == Region.Find( c.Location, map ).IsPartOf( typeof( DungeonRegion ) ) )
					return false;
			}

			return true;
		}

		public void OnLogin( LoginEventArgs e )
		{
			// Publish 59: Ghosts without corpses cannot enter Champ Spawn regions or log in to them

			Mobile m = e.Mobile;

			if ( m.Region.IsPartOf( m_Region ) && IsGhost( m ) )
				m.Location = ExorcismSpell.GetNearestShrine( m );
		}

		public void OnLogout( LogoutEventArgs e )
		{
			/* Publish 59: Players can no longer log out without repercussions in Champ Spawn regions
			 * - Players that log out in a champ spawn region will get teleported to a shrine
			 * - If a player logs out in a champ spawn area all the non equipped "cursed" items on them will drop to the ground
			 * - This will only affect players who log out beyond the 10 minute grace period; accidental disconnections will not be punished
			 */

			Mobile m = e.Mobile;

			if ( m.Region.IsPartOf( m_Region ) )
			{
				if ( m.Backpack != null )
				{
					List<Item> list = new List<Item>();

					RecurseGetCursedItems( m.Backpack, list );

					foreach ( Item item in list )
						item.MoveToWorld( m.Location, m.Map );
				}

				m.Location = ExorcismSpell.GetNearestShrine( m );
			}
		}

		private static void RecurseGetCursedItems( Container cont, List<Item> list )
		{
			foreach ( Item item in cont.Items )
			{
				if ( item is Container )
					RecurseGetCursedItems( (Container) item, list );
				else if ( item.LootType == LootType.Cursed )
					list.Add( item );
			}
		}

		public virtual void OnSpawnActivated()
		{
		}

		public virtual void OnSpawnDeactivated()
		{
		}

		public virtual void OnLevelAdvanced()
		{
		}

		public virtual void OnSubLevelAdvanced()
		{
		}

		public virtual void OnChampionSpawned()
		{
		}

		public void AdvanceLevel()
		{
			m_ExpireTime = DateTime.UtcNow + m_ExpireDelay;

			if ( Level < 16 )
			{
				int oldSubLevel = GetSubLevel();

				m_Kills = 0;
				++Level;
				InvalidateProperties();
				SetWhiteSkullCount( 0 );

				if ( m_Altar != null )
				{
					Effects.PlaySound( m_Altar.Location, m_Altar.Map, 0x29 );
					Effects.SendLocationEffect( new Point3D( m_Altar.X + 1, m_Altar.Y + 1, m_Altar.Z ), m_Altar.Map, 0x3728, 10 );
				}

				OnLevelAdvanced();

				if ( GetSubLevel() > oldSubLevel )
					OnSubLevelAdvanced();
			}
			else
			{
				SpawnChampion();
				OnChampionSpawned();
			}
		}

		public void SpawnChampion()
		{
			if ( m_Altar != null )
				m_Altar.Hue = 0x26;

			m_Kills = 0;
			Level = 0;
			InvalidateProperties();
			SetWhiteSkullCount( 0 );

			try
			{
				m_Champion = Activator.CreateInstance( ChampionSpawnInfo.GetInfo( m_Type ).Champion ) as Mobile;
			}
			catch { }

			if ( m_Champion != null )
			{
				var location = new Point3D( X, Y, Z - 15 );

				m_Champion.OnBeforeSpawn( location, Map );
				m_Champion.MoveToWorld( location, Map );
			}
		}

		public void Respawn()
		{
			if ( !m_Active || Deleted || m_Champion != null )
				return;

			while ( m_Creatures.Count < ( 200 - ( GetSubLevel() * 40 ) ) )
			{
				Mobile m = Spawn();

				if ( m == null )
					return;

				Point3D loc = GetSpawnLocation();

				// Allow creatures to turn into Paragons at Ilshenar champions.
				m.OnBeforeSpawn( loc, Map );

				m_Creatures.Add( m );
				m.MoveToWorld( loc, Map );

				if ( m is BaseCreature )
				{
					BaseCreature bc = m as BaseCreature;

					bc.Tamable = false;
					bc.Home = Location;
					bc.RangeHome = bc.RangeHome = (int) ( Math.Sqrt( m_SpawnArea.Width * m_SpawnArea.Width + m_SpawnArea.Height * m_SpawnArea.Height ) / 2 );
					bc.IsChampionMonster = true;

					int value = 0;

					if ( Level >= 0 && Level <= 5 )
						value = 1;
					else if ( Level >= 6 && Level <= 9 )
						value = 2;
					else if ( Level >= 10 && Level <= 13 )
						value = 3;
					else if ( Level >= 14 && Level <= 16 )
						value = 4;

					bc.SpawnLevel = value;

					bc.ChampionType = Type;
				}
			}
		}

		public Point3D GetSpawnLocation()
		{
			Map map = Map;

			if ( map == null )
				return Location;

			// Try 20 times to find a spawnable location.
			for ( int i = 0; i < 20; i++ )
			{
				int x = Utility.Random( m_SpawnArea.X, m_SpawnArea.Width );
				int y = Utility.Random( m_SpawnArea.Y, m_SpawnArea.Height );

				int z = Map.GetAverageZ( x, y );

				if ( Map.CanSpawnMobile( new Point2D( x, y ), z ) )
					return new Point3D( x, y, z );
			}

			return Location;
		}

		private static int Level1 = 5;  // First spawn level from 0-5 red skulls
		private static int Level2 = 9;  // Second spawn level from 6-9 red skulls
		private static int Level3 = 13; // Third spawn level from 10-13 red skulls

		public int GetSubLevel()
		{
			int level = this.Level;

			if ( level <= Level1 )
				return 0;
			else if ( level <= Level2 )
				return 1;
			else if ( level <= Level3 )
				return 2;

			return 3;
		}

		public int GetSubLevelFor( Mobile m )
		{
			Type[][] types = ChampionSpawnInfo.GetInfo( m_Type ).SpawnTypes;
			Type t = m.GetType();

			for ( int i = 0; i < types.GetLength( 0 ); i++ )
			{
				Type[] individualTypes = types[i];

				for ( int j = 0; j < individualTypes.Length; j++ )
				{
					if ( t == individualTypes[j] )
						return i;
				}
			}

			return -1;
		}

		public Mobile Spawn()
		{
			Type[][] types = ChampionSpawnInfo.GetInfo( m_Type ).SpawnTypes;

			int v = GetSubLevel();

			if ( v >= 0 && v < types.Length )
				return Spawn( types[v] );

			return null;
		}

		public Mobile Spawn( params Type[] types )
		{
			try
			{
				return Activator.CreateInstance( types[Utility.Random( types.Length )] ) as Mobile;
			}
			catch
			{
				return null;
			}
		}

		public void Expire()
		{
			m_Kills = 0;

			if ( m_WhiteSkulls.Count == 0 )
			{
				// They didn't even get 20%, go back a level

				if ( Level > 0 )
					--Level;

				InvalidateProperties();
			}
			else
			{
				SetWhiteSkullCount( 0 );
			}

			m_ExpireTime = DateTime.UtcNow + m_ExpireDelay;
		}

		public Point3D GetRedSkullLocation( int index )
		{
			int x, y;

			if ( index < 5 )
			{
				x = index - 2;
				y = -2;
			}
			else if ( index < 9 )
			{
				x = 2;
				y = index - 6;
			}
			else if ( index < 13 )
			{
				x = 10 - index;
				y = 2;
			}
			else
			{
				x = -2;
				y = 14 - index;
			}

			return new Point3D( X + x, Y + y, Z - 15 );
		}

		public Point3D GetWhiteSkullLocation( int index )
		{
			int x, y;

			switch ( index )
			{
				default:
				case 0:
					x = -1;
					y = -1;
					break;
				case 1:
					x = 1;
					y = -1;
					break;
				case 2:
					x = 1;
					y = 1;
					break;
				case 3:
					x = -1;
					y = 1;
					break;
			}

			return new Point3D( X + x, Y + y, Z - 15 );
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( "champion spawn" );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Active )
			{
				list.Add( 1060742 ); // active
				list.Add( 1060658, "Type\t{0}", m_Type ); // ~1_val~: ~2_val~
				list.Add( 1060659, "Level\t{0}", Level ); // ~1_val~: ~2_val~
				list.Add( 1060660, "Kills\t{0} of {1} ({2:F1}%)", m_Kills, MaxKills, 100.0 * ( (double) m_Kills / MaxKills ) ); // ~1_val~: ~2_val~
				//list.Add( 1060661, "Spawn Range\t{0}", m_SpawnRange ); // ~1_val~: ~2_val~
			}
			else
			{
				list.Add( 1060743 ); // inactive
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendGump( new PropertiesGump( from, this ) );
		}

		public override void OnLocationChange( Point3D oldLoc )
		{
			if ( Deleted )
				return;

			if ( m_Platform != null )
				m_Platform.Location = new Point3D( X, Y, Z - 20 );

			if ( m_Altar != null )
				m_Altar.Location = new Point3D( X, Y, Z - 15 );

			if ( m_Idol != null )
				m_Idol.Location = new Point3D( X, Y, Z - 15 );

			if ( m_RedSkulls != null )
			{
				for ( int i = 0; i < m_RedSkulls.Count; ++i )
					m_RedSkulls[i].Location = GetRedSkullLocation( i );
			}

			if ( m_WhiteSkulls != null )
			{
				for ( int i = 0; i < m_WhiteSkulls.Count; ++i )
					m_WhiteSkulls[i].Location = GetWhiteSkullLocation( i );
			}

			SetSpawnArea( 50 );

			UpdateRegion();
		}

		public override void OnMapChange()
		{
			if ( Deleted )
				return;

			if ( m_Platform != null )
				m_Platform.Map = Map;

			if ( m_Altar != null )
				m_Altar.Map = Map;

			if ( m_Idol != null )
				m_Idol.Map = Map;

			if ( m_RedSkulls != null )
			{
				for ( int i = 0; i < m_RedSkulls.Count; ++i )
					m_RedSkulls[i].Map = Map;
			}

			if ( m_WhiteSkulls != null )
			{
				for ( int i = 0; i < m_WhiteSkulls.Count; ++i )
					m_WhiteSkulls[i].Map = Map;
			}

			UpdateRegion();
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if ( m_Platform != null )
				m_Platform.Delete();

			if ( m_Altar != null )
				m_Altar.Delete();

			if ( m_Idol != null )
				m_Idol.Delete();

			if ( m_RedSkulls != null )
			{
				for ( int i = 0; i < m_RedSkulls.Count; ++i )
					m_RedSkulls[i].Delete();

				m_RedSkulls.Clear();
			}

			if ( m_WhiteSkulls != null )
			{
				for ( int i = 0; i < m_WhiteSkulls.Count; ++i )
					m_WhiteSkulls[i].Delete();

				m_WhiteSkulls.Clear();
			}

			if ( m_Creatures != null )
			{
				for ( int i = 0; i < m_Creatures.Count; ++i )
				{
					Mobile mob = m_Creatures[i];

					if ( !mob.Player )
						mob.Delete();
				}

				m_Creatures.Clear();
			}

			if ( m_Champion != null && !m_Champion.Player )
				m_Champion.Delete();

			Stop();

			UpdateRegion();
		}

		public ChampionSpawn( Serial serial )
			: base( serial )
		{
		}

		public virtual void RegisterDamageTo( Mobile m )
		{
			if ( m == null )
				return;

			foreach ( DamageEntry de in m.DamageEntries )
			{
				if ( de.HasExpired )
					continue;

				Mobile damager = de.Damager;

				Mobile master = damager.GetDamageMaster( m );

				if ( master != null )
					damager = master;

				RegisterDamage( damager, de.DamageGiven );
			}
		}

		public void RegisterDamage( Mobile from, int amount )
		{
			if ( from == null || !from.Player )
				return;

			if ( m_DamageEntries.ContainsKey( from ) )
				m_DamageEntries[from] += amount;
			else
				m_DamageEntries.Add( from, amount );
		}

		public void AwardArtifact( Item artifact )
		{
			if ( artifact == null )
				return;

			int totalDamage = 0;

			Dictionary<Mobile, int> validEntries = new Dictionary<Mobile, int>();

			foreach ( KeyValuePair<Mobile, int> kvp in m_DamageEntries )
			{
				if ( IsEligible( kvp.Key, artifact ) )
				{
					validEntries.Add( kvp.Key, kvp.Value );
					totalDamage += kvp.Value;
				}
			}

			int randomDamage = Utility.RandomMinMax( 1, totalDamage );

			totalDamage = 0;

			foreach ( KeyValuePair<Mobile, int> kvp in validEntries )
			{
				totalDamage += kvp.Value;

				if ( totalDamage > randomDamage )
				{
					GiveArtifact( kvp.Key, artifact );
					return;
				}
			}

			artifact.Delete();
		}

		public void GiveArtifact( Mobile to, Item artifact )
		{
			if ( to == null || artifact == null )
				return;

			Container pack = to.Backpack;

			if ( pack == null || !pack.TryDropItem( to, artifact, false ) )
				artifact.Delete();
			else
			{
				to.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.

				Effects.SendLocationParticles( EffectItem.Create( to.Location, to.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
				Effects.PlaySound( to.Location, to.Map, 0x243 );

				Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( to.X - 6, to.Y - 6, to.Z + 15 ), to.Map ), to, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
				Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( to.X - 4, to.Y - 6, to.Z + 15 ), to.Map ), to, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
				Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( to.X - 6, to.Y - 4, to.Z + 15 ), to.Map ), to, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );

				Effects.SendTargetParticles( to, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer) 255, 0x100 );
			}
		}

		public bool IsEligible( Mobile m, Item Artifact )
		{
			return m.Player && m.Alive && m.InRange( m_Champion.Corpse, 90 ) && m.Backpack != null && m.Backpack.CheckHold( m, Artifact, false );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 5 ); // version

			writer.Write( m_KickDelay );
			writer.Write( m_KickTime );

			writer.Write( m_SpawnArea );

			writer.Write( m_DamageEntries.Count );
			foreach ( KeyValuePair<Mobile, int> kvp in m_DamageEntries )
			{
				writer.Write( kvp.Key );
				writer.Write( kvp.Value );
			}

			writer.Write( m_HasBeenAdvanced );

			writer.Write( m_Idol );

			writer.Write( m_RandomizeType );

			//writer.Write( m_SpawnRange );
			writer.Write( m_Kills );

			writer.Write( (bool) m_Active );
			writer.Write( (int) m_Type );
			writer.Write( m_Creatures, true );
			writer.Write( m_RedSkulls, true );
			writer.Write( m_WhiteSkulls, true );
			writer.WriteItem<ChampionPlatform>( m_Platform );
			writer.WriteItem<ChampionAltar>( m_Altar );
			writer.Write( m_ExpireDelay );
			writer.WriteDeltaTime( m_ExpireTime );
			writer.Write( m_Champion );
			writer.Write( m_RestartDelay );

			writer.Write( m_RestartTimer != null );

			if ( m_RestartTimer != null )
				writer.WriteDeltaTime( m_RestartTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			m_DamageEntries = new Dictionary<Mobile, int>();

			int version = reader.ReadInt();

			switch ( version )
			{
				case 5:
					{
						m_KickDelay = reader.ReadTimeSpan();
						m_KickTime = reader.ReadDateTime();

						m_SpawnArea = reader.ReadRect2D();

						goto case 4;
					}
				case 4:
					{
						int entries = reader.ReadInt();
						Mobile m;
						int damage;
						for ( int i = 0; i < entries; ++i )
						{
							m = reader.ReadMobile();
							damage = reader.ReadInt();

							if ( m != null )
								m_DamageEntries.Add( m, damage );
						}

						goto case 3;
					}
				case 3:
					{
						m_HasBeenAdvanced = reader.ReadBool();

						m_Idol = reader.ReadItem() as IdolOfTheChampion;

						goto case 2;
					}
				case 2:
					{
						m_RandomizeType = reader.ReadBool();

						goto case 1;
					}
				case 1:
					{
						if ( version < 5 )
						{
							int oldRange = reader.ReadInt();

							m_SpawnArea = new Rectangle2D( new Point2D( X - oldRange, Y - oldRange ), new Point2D( X + oldRange, Y + oldRange ) );
						}

						m_Kills = reader.ReadInt();

						goto case 0;
					}
				case 0:
					{
						if ( version < 1 )
							SetSpawnArea( 50 );

						bool active = reader.ReadBool();
						m_Type = (ChampionSpawnType) reader.ReadInt();
						m_Creatures = reader.ReadStrongMobileList();
						m_RedSkulls = reader.ReadStrongItemList();
						m_WhiteSkulls = reader.ReadStrongItemList();
						m_Platform = reader.ReadItem<ChampionPlatform>();
						m_Altar = reader.ReadItem<ChampionAltar>();
						m_ExpireDelay = reader.ReadTimeSpan();
						m_ExpireTime = reader.ReadDeltaTime();
						m_Champion = reader.ReadMobile();
						m_RestartDelay = reader.ReadTimeSpan();

						if ( reader.ReadBool() )
						{
							m_RestartTime = reader.ReadDeltaTime();
							BeginRestart( m_RestartTime - DateTime.UtcNow );
						}

						if ( m_Platform == null || m_Altar == null )
							Delete();
						else if ( active )
							Start( false );

						break;
					}
			}

			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( UpdateRegion ) );
		}
	}

	public class ChampionSpawnRegion : BaseRegion
	{
		private ChampionSpawn m_Spawn;

		public ChampionSpawn ChampionSpawn
		{
			get { return m_Spawn; }
		}

		private static Rectangle2D GetRegionBounds( Rectangle2D spawnBounds )
		{
			return new Rectangle2D( spawnBounds.X - 20, spawnBounds.Y - 20, spawnBounds.Width + 40, spawnBounds.Height + 40 );
		}

		public ChampionSpawnRegion( ChampionSpawn spawn )
			: base( null, spawn.Map, Region.Find( spawn.Location, spawn.Map ), GetRegionBounds( spawn.SpawnArea ) )
		{
			m_Spawn = spawn;
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override void OnEnter( Mobile m )
		{
			// Publish 59: Ghosts without corpses cannot enter Champ Spawn regions or log in to them
			if ( ChampionSpawn.IsGhost( m ) )
				m.Location = ExorcismSpell.GetNearestShrine( m );
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			base.AlterLightLevel( m, ref global, ref personal );

			// This is a guesstimate. TODO: Verify & get exact values
			// OSI testing: at 2 red skulls, light = 0x3 ; 1 red = 0x3.; 3 = 8; 9 = 0xD 8 = 0xD 12 = 0x12 10 = 0xD
			global = Math.Max( global, 1 + m_Spawn.Level );
		}
	}

	[TypeAlias( "Server.Items.ChampionIdol" )]
	public class IdolOfTheChampion : Item
	{
		private ChampionSpawn m_Spawn;

		public ChampionSpawn Spawn { get { return m_Spawn; } }

		public IdolOfTheChampion( ChampionSpawn spawn )
			: base( 0x1F18 )
		{
			m_Spawn = spawn;
			Name = "Idol of the Champion";
			Movable = false;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if ( m_Spawn != null )
				m_Spawn.Delete();
		}

		public IdolOfTheChampion( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Spawn );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Spawn = reader.ReadItem() as ChampionSpawn;

						if ( m_Spawn == null )
							Delete();

						break;
					}
			}
		}
	}
}