using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Events;

namespace Server.Engines.MiniChamps
{
	public class MiniChampSpawnInfo
	{
		public static void Initialize()
		{
			EventSink.WorldBeforeSave += new WorldBeforeSaveEventHandler( CleanupSpawn );
		}

		private static void CleanupSpawn()
		{
			foreach ( BaseCreature bc in m_Despawn )
				bc.Delete();

			m_Despawn.Clear();
		}

		private static List<BaseCreature> m_Despawn = new List<BaseCreature>();

		private MiniChampController m_Owner;

		private List<BaseCreature> m_Creatures;

		private Type m_MonsterType;

		private int m_Killed;
		private int m_Spawned;
		private int m_Required;

		public Type MonsterType { get { return m_MonsterType; } }

		public int Killed { get { return m_Killed; } }
		public int Spawned { get { return m_Spawned; } }
		public int Required { get { return m_Required; } }

		public int MaxSpawned
		{
			get { return ( m_Required * 2 ) - 1; }
		}

		public bool Done
		{
			get { return m_Killed >= m_Required; }
		}

		public MiniChampSpawnInfo( MiniChampController controller, MiniChampTypeInfo typeInfo )
		{
			m_Owner = controller;

			m_Required = typeInfo.Required;
			m_MonsterType = typeInfo.SpawnType;

			m_Creatures = new List<BaseCreature>();
			m_Killed = 0;
			m_Spawned = 0;
		}

		public bool Slice()
		{
			bool killed = false;

			for ( int i = 0; i < m_Creatures.Count; i++ )
			{
				Mobile creature = m_Creatures[i];

				if ( creature == null || creature.Deleted )
				{
					m_Creatures.RemoveAt( i );
					--i;
					++m_Killed;

					killed = true;
				}
				else if ( !creature.InRange( m_Owner.Location, m_Owner.SpawnRange + 10 ) )
				{
					// bring to home

					Map map = m_Owner.Map;
					Point3D loc = map.GetSpawnPosition( m_Owner.Location, m_Owner.SpawnRange );

					creature.MoveToWorld( loc, map );
				}
			}

			return killed;
		}

		public bool Respawn()
		{
			bool spawned = false;

			while ( m_Creatures.Count < m_Required && m_Spawned < MaxSpawned )
			{
				BaseCreature bc = Activator.CreateInstance( MonsterType ) as BaseCreature;

				Map map = m_Owner.Map;
				Point3D loc = map.GetSpawnPosition( m_Owner.Location, m_Owner.SpawnRange );

				if ( bc is Meraktus )
					loc = m_Owner.GetBossSpawnPoint();

				bc.Home = m_Owner.Location;
				bc.RangeHome = m_Owner.SpawnRange;

				bc.IsMinichampMonster = true;
				bc.MinichampType = m_Owner.Type;

				bc.OnBeforeSpawn( loc, map );

				bc.MoveToWorld( loc, map );
				m_Creatures.Add( bc );

				++m_Spawned;

				spawned = true;
			}

			return spawned;
		}

		public void Despawn()
		{
			foreach ( BaseCreature bc in m_Creatures )
				m_Despawn.Add( bc );

			m_Creatures.Clear();
		}

		public void AddProperties( ObjectPropertyList list, int cliloc )
		{
			list.Add( cliloc, "{0}: Killed {1}/{2}, Spawned {3}/{4}",
				m_MonsterType.Name, m_Killed, m_Required, m_Spawned, MaxSpawned );
		}

		public void Serialize( GenericWriter writer )
		{
			writer.WriteItem<MiniChampController>( m_Owner );
			writer.Write( m_Killed );
			writer.Write( m_Spawned );
			writer.Write( m_Required );
			writer.Write( m_MonsterType.FullName );

			writer.Write( m_Creatures.Count );

			for ( int i = 0; i < m_Creatures.Count; i++ )
				writer.Write( m_Creatures[i] );
		}

		public MiniChampSpawnInfo( GenericReader reader )
		{
			m_Creatures = new List<BaseCreature>();

			m_Owner = reader.ReadItem<MiniChampController>();
			m_Killed = reader.ReadInt();
			m_Spawned = reader.ReadInt();
			m_Required = reader.ReadInt();
			m_MonsterType = ScriptCompiler.FindTypeByFullName( reader.ReadString() );

			int count = reader.ReadInt();

			for ( int i = 0; i < count; i++ )
				m_Creatures.Add( reader.ReadMobile() as BaseCreature );
		}
	}

	public class MiniChampController : Item
	{
		private bool m_Active;
		private MiniChampType m_Type;
		private List<MiniChampSpawnInfo> m_Spawn;
		private int m_Level;
		private int m_SpawnRange;
		private TimeSpan m_RestartDelay;
		private Timer m_Timer, m_RestartTimer;

		private Point3D m_BossSpawnPoint;

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D BossSpawnPoint
		{
			get { return m_BossSpawnPoint; }
			set
			{
				m_BossSpawnPoint = value;
			}
		}

		[Constructable]
		public MiniChampController()
			: base( 0xBD2 )
		{
			Movable = false;
			Visible = false;

			m_Spawn = new List<MiniChampSpawnInfo>();

			m_RestartDelay = TimeSpan.FromMinutes( 5.0 );

			m_SpawnRange = 30;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpawnRange
		{
			get { return m_SpawnRange; }
			set { m_SpawnRange = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan RestartDelay
		{
			get { return m_RestartDelay; }
			set { m_RestartDelay = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public MiniChampType Type
		{
			get { return m_Type; }
			set { m_Type = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
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
		public int Level
		{
			get { return m_Level; }
			set { m_Level = value; InvalidateProperties(); }
		}

		public void Start()
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

			AdvanceLevel();
		}

		public void Stop()
		{
			if ( !m_Active || Deleted )
				return;

			m_Active = false;
			m_Level = 0;

			ClearSpawn();

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;

			if ( m_RestartTimer != null )
				m_RestartTimer.Stop();

			m_RestartTimer = null;
		}

		public void OnSlice()
		{
			if ( !m_Active || Deleted )
				return;

			bool changed = false;
			bool done = true;

			foreach ( MiniChampSpawnInfo spawnInfo in m_Spawn )
			{
				changed |= spawnInfo.Slice();
				done &= spawnInfo.Done;
			}

			if ( done )
				AdvanceLevel();

			if ( m_Active )
			{
				foreach ( MiniChampSpawnInfo spawnInfo in m_Spawn )
					changed |= spawnInfo.Respawn();
			}

			if ( done || changed )
				InvalidateProperties();
		}

		public void ClearSpawn()
		{
			foreach ( MiniChampSpawnInfo spawn in m_Spawn )
				spawn.Despawn();

			m_Spawn.Clear();
		}

		public void AdvanceLevel()
		{
			MiniChampLevelInfo levelInfo = MiniChampInfo.GetInfo( m_Type ).GetLevelInfo( ++Level );

			if ( levelInfo != null )
			{
				ClearSpawn();

				if ( m_Type == MiniChampType.Minotaur )
				{
					MinotaurShouts();
				}

				foreach ( MiniChampTypeInfo typeInfo in levelInfo.Types )
					m_Spawn.Add( new MiniChampSpawnInfo( this, typeInfo ) );
			}
			else // begin restart
			{
				Stop();

				m_RestartTimer = Timer.DelayCall( m_RestartDelay, new TimerCallback( Start ) );
			}
		}

		private void MinotaurShouts()
		{
			int cliloc = 0;

			switch ( Level )
			{
				case 1:
					return;
				case 2:
					cliloc = 1073370;
					break;
				case 3:
					cliloc = 1073367;
					break;
				case 4:
					cliloc = 1073368;
					break;
				case 5:
					cliloc = 1073369;
					break;
			}

			foreach ( Mobile m in this.GetMobilesInRange( this.m_SpawnRange ) )
			{
				if ( m is PlayerMobile )
					m.SendLocalizedMessage( cliloc );
			}
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( "minichamp controller" );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060658, "Type\t{0}", m_Type ); // ~1_val~: ~2_val~
			list.Add( 1060661, "Spawn Range\t{0}", m_SpawnRange ); // ~1_val~: ~2_val~

			if ( m_Active )
			{
				list.Add( 1060742 ); // active
				list.Add( 1060659, "Level\t{0}", Level ); // ~1_val~: ~2_val~

				for ( int i = 0; i < m_Spawn.Count; i++ )
					m_Spawn[i].AddProperties( list, i + 1150301 );
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

		public override void OnAfterDelete()
		{
			Stop();

			base.OnAfterDelete();
		}

		public Point3D GetBossSpawnPoint()
		{
			return m_BossSpawnPoint;
		}

		public MiniChampController( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_BossSpawnPoint );
			writer.Write( (bool) m_Active );
			writer.Write( (int) m_Type );
			writer.Write( (int) m_Level );
			writer.Write( (int) m_SpawnRange );
			writer.Write( (TimeSpan) m_RestartDelay );

			writer.Write( (int) m_Spawn.Count );

			for ( int i = 0; i < m_Spawn.Count; i++ )
				m_Spawn[i].Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_BossSpawnPoint = reader.ReadPoint3D();
						goto case 0;
					}
				case 0:
					{
						m_Spawn = new List<MiniChampSpawnInfo>();

						m_Active = reader.ReadBool();
						m_Type = (MiniChampType) reader.ReadInt();
						m_Level = reader.ReadInt();
						m_SpawnRange = reader.ReadInt();
						m_RestartDelay = reader.ReadTimeSpan();

						int spawnCount = reader.ReadInt();
						for ( int i = 0; i < spawnCount; i++ )
							m_Spawn.Add( new MiniChampSpawnInfo( reader ) );

						if ( m_Active )
						{
							m_Timer = new SliceTimer( this );
							m_Timer.Start();
						}
						else
						{
							m_RestartTimer = Timer.DelayCall( m_RestartDelay, new TimerCallback( Start ) );
						}

						break;
					}
			}
		}
	}

	public class SliceTimer : Timer
	{
		private MiniChampController m_Controller;

		public SliceTimer( MiniChampController controller )
			: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 10.0 ) )
		{
			m_Controller = controller;
		}

		protected override void OnTick()
		{
			m_Controller.OnSlice();
		}
	}
}