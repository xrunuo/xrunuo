using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Events;

namespace Server.Mobiles
{
	public enum SpawnFlag
	{
		None = 0x0000,
		Group = 0x0001,
		CantWalk = 0x0002,
		Blessed = 0x0004,
		Murderer = 0x0008,
		ScaledDelay = 0x0010,
		Saturable = 0x0020,
		RespawnAtRestart = 0x0040,
	}

	public class CreatureSpawner : Item
	{
		#region Untamed pets cleaning
		public static void Initialize()
		{
			EventSink.WorldBeforeSave += new WorldBeforeSaveEventHandler( OnBeforeSave );
		}

		private static void OnBeforeSave()
		{
			CleanUntamedPets();
		}

		private static void CleanUntamedPets()
		{
			List<Mobile> list = new List<Mobile>();

			foreach ( Mobile m in World.Mobiles )
			{
				if ( m is BaseCreature )
				{
					BaseCreature bc = m as BaseCreature;

					if ( bc.RemoveOnSave && !bc.Controlled && bc.ControlMaster == null )
						list.Add( bc );
				}
			}

			for ( int i = 0; i < list.Count; i++ )
			{
				list[i].Delete();
			}
		}
		#endregion

		private bool m_Active;
		private Type m_SpawnType;
		private int m_Count;
		private TimeSpan m_MinDelay;
		private TimeSpan m_MaxDelay;
		private int m_HomeRange;
		private int m_SpawnRange;
		private int m_Team;
		private WayPoint m_WayPoint;
		private SpawnFlag m_Flags;
		private Timer m_GroupRespawnTimer;
		private DateTime m_NextGroupRespawn;

		private List<SpawnInstance> m_Instances;

		protected bool GetFlag( SpawnFlag flag )
		{
			return ( m_Flags & flag ) != 0;
		}

		protected void SetFlag( SpawnFlag flag, bool value )
		{
			if ( value )
				m_Flags |= flag;
			else
				m_Flags &= ~flag;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set
			{
				m_Active = value;

				if ( !m_Active )
					Despawn();
				else
					TotalRespawn();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string SpawnName { get { return m_SpawnType != null ? m_SpawnType.Name : "(invalid name)"; } set { m_SpawnType = ValidateType( value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Count { get { return m_Count; } set { m_Count = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan MinDelay { get { return m_MinDelay; } set { m_MinDelay = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan MaxDelay { get { return m_MaxDelay; } set { m_MaxDelay = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int HomeRange { get { return m_HomeRange; } set { m_HomeRange = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpawnRange { get { return m_SpawnRange; } set { m_SpawnRange = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Team { get { return m_Team; } set { m_Team = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public WayPoint WayPoint { get { return m_WayPoint; } set { m_WayPoint = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Group
		{
			get { return GetFlag( SpawnFlag.Group ); }
			set { SetFlag( SpawnFlag.Group, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CantWalk
		{
			get { return GetFlag( SpawnFlag.CantWalk ); }
			set { SetFlag( SpawnFlag.CantWalk, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Blessed
		{
			get { return GetFlag( SpawnFlag.Blessed ); }
			set { SetFlag( SpawnFlag.Blessed, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Murderer
		{
			get { return GetFlag( SpawnFlag.Murderer ); }
			set { SetFlag( SpawnFlag.Murderer, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ScaledDelay
		{
			get { return GetFlag( SpawnFlag.ScaledDelay ); }
			set { SetFlag( SpawnFlag.ScaledDelay, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Saturable
		{
			get { return GetFlag( SpawnFlag.Saturable ); }
			set { SetFlag( SpawnFlag.Saturable, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RespawnAtRestart
		{
			get { return GetFlag( SpawnFlag.RespawnAtRestart ); }
			set { SetFlag( SpawnFlag.RespawnAtRestart, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TotalSpawned
		{
			get
			{
				int spawned = 0;

				for ( int i = 0; i < m_Instances.Count; i++ )
				{
					if ( m_Instances[i].Active )
						spawned++;
				}

				return spawned;
			}
		}

		public List<SpawnInstance> Instances { get { return m_Instances; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextGroupRespawn { get { return m_NextGroupRespawn; } }

		#region Constructors

		[Constructable]
		public CreatureSpawner()
			: this( "" )
		{
		}

		[Constructable]
		public CreatureSpawner( string spawnName )
			: this( spawnName, 1 )
		{
		}

		[Constructable]
		public CreatureSpawner( string spawnName, int amount )
			: this( spawnName, amount, 300, 600 )
		{
		}

		[Constructable]
		public CreatureSpawner( string spawnName, int count, int minDelay, int maxDelay )
			: this( spawnName, count, minDelay, maxDelay, 4, 4 )
		{
		}

		[Constructable]
		public CreatureSpawner( string spawnName, int count, int minDelay, int maxDelay, int homeRange, int spawnRange )
			: base( 0x1F13 )
		{
			Name = String.Intern( "CreatureSpawner" );

			Visible = false;
			Movable = false;

			m_SpawnType = ValidateType( spawnName );
			m_Active = m_SpawnType != null;
			m_Count = count;
			m_MinDelay = TimeSpan.FromSeconds( minDelay );
			m_MaxDelay = TimeSpan.FromSeconds( maxDelay );
			m_HomeRange = homeRange;
			m_SpawnRange = spawnRange;

			m_Flags = SpawnFlag.ScaledDelay | SpawnFlag.Saturable; // Add here default flags

			m_Instances = new List<SpawnInstance>();

			Timer.DelayCall( TimeSpan.FromSeconds( 0.1 ), TotalRespawn );
		}

		public CreatureSpawner( Serial serial )
			: base( serial )
		{
		}

		#endregion

		public int GetSurroundingPlayers()
		{
			int players = 0;

			foreach ( Mobile m in this.GetMobilesInRange( this.HomeRange ) )
				if ( m.Player && m.AccessLevel == AccessLevel.Player )
					players++;

			return players;
		}

		private Type ValidateType( String typeName )
		{
			Type type = ScriptCompiler.FindTypeByName( typeName );

			if ( type != null && typeof( ISpawnable ).IsAssignableFrom( type ) )
				return type;
			else
				return null;
		}

		public void Despawn()
		{
			m_Active = false;

			for ( int i = 0; i < m_Instances.Count; i++ )
			{
				SpawnInstance si = m_Instances[i];

				si.Free(); // also calls si.Despawn()
			}

			m_Instances.Clear();

			if ( m_GroupRespawnTimer != null )
			{
				m_GroupRespawnTimer.Stop();
				m_GroupRespawnTimer = null;
			}

			InvalidateProperties();
		}

		public void TotalRespawn()
		{
			Despawn();

			if ( m_SpawnType != null )
			{
				m_Active = true;

				for ( int i = 0; i < m_Count; ++i )
				{
					SpawnInstance si = SpawnInstance.Instantiate( this, Group );
					m_Instances.Add( si );
					si.Respawn();
				}
			}

			InvalidateProperties();
		}

		public void OnEntityAdded( ISpawnable spawnable )
		{
			InvalidateProperties();
		}

		public void OnEntityRemoved( ISpawnable spawnable )
		{
			if ( Group && TotalSpawned == 0 )
			{
				if ( m_GroupRespawnTimer != null )
				{
					m_GroupRespawnTimer.Stop();
					m_GroupRespawnTimer = null;
				}

				TimeSpan delay = TimeSpan.FromSeconds( Utility.RandomMinMax( (int) m_MinDelay.TotalSeconds, (int) m_MaxDelay.TotalSeconds ) );

				m_GroupRespawnTimer = Timer.DelayCall( delay, GroupRespawn );

				m_NextGroupRespawn = DateTime.UtcNow + delay;
			}

			InvalidateProperties();
		}

		private void GroupRespawn()
		{
			for ( int i = 0; i < m_Instances.Count; i++ )
			{
				m_Instances[i].Respawn();
			}

			InvalidateProperties();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				from.SendGump( new CreatureSpawnerGump( this ) );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Active )
				list.Add( 1060742 ); // active
			else
				list.Add( 1060743 ); // inactive

			list.Add( 1060656, String.Format( "{0}/{1}", TotalSpawned.ToString(), Count.ToString() ) );
			list.Add( 1060663, "SpawnName\t{0}", SpawnName );
			list.Add( 1060662, "Speed\t{0} to {1}", MinDelay, MaxDelay );
			list.Add( 1060658, "SpawnRange\t{0}", SpawnRange.ToString() );
			list.Add( 1060659, "HomeRange\t{0}", HomeRange.ToString() );
			list.Add( 1060660, "Group\t{0}", Group );
			list.Add( 1060661, "Team\t{0}", Team );
		}

		public virtual bool CanSpawn()
		{
			return this.Map != null && this.Map != Map.Internal;
		}

		protected virtual ISpawnable ConstructSpawnableObject()
		{
			try
			{
				return (ISpawnable) Activator.CreateInstance( m_SpawnType );
			}
			catch
			{
			}

			return null;
		}

		protected virtual ISpawnable Spawn()
		{
			if ( !CanSpawn() )
				return null;

			Map map = this.Map;

			ISpawnable spawnable = ConstructSpawnableObject();

			if ( spawnable != null )
			{
				Point3D loc = map.GetSpawnPosition( this.Location, m_SpawnRange );

				spawnable.OnBeforeSpawn( loc, map );
				spawnable.MoveToWorld( loc, map );

				if ( spawnable is BaseCreature )
				{
					BaseCreature bc = (BaseCreature) spawnable;

					bc.Home = this.Location;
					bc.RangeHome = this.HomeRange;
					bc.CurrentWayPoint = this.WayPoint;
					bc.CantWalk = this.CantWalk;
					bc.Blessed = this.Blessed;
					bc.Team = this.Team;

					if ( this.Blessed )
						bc.Tamable = false;

					if ( this.Murderer )
						bc.Kills = 100;
				}

				this.OnAfterSpawn( spawnable );

				spawnable.OnAfterSpawn();
			}

			return spawnable;
		}

		protected virtual void OnAfterSpawn( ISpawnable spawnable )
		{
		}

		public void BringToHome()
		{
			for ( int i = 0; i < m_Instances.Count; i++ )
			{
				SpawnInstance si = m_Instances[i];

				si.BringToHome();
			}
		}

		public override void OnAfterDelete()
		{
			Despawn();

			base.OnAfterDelete();
		}

		public class SpawnInstance : ISpawner
		{
			public enum SpawnState
			{
				Inactive,
				Active,
				Respawning
			}

			private CreatureSpawner m_Owner;
			private ISpawnable m_Spawned;
			private SpawnState m_State;
			private Timer m_Timer;
			private DateTime m_NextSpawn;
			private bool m_DeactivateOnRemove;

			public bool Active { get { return m_State == SpawnState.Active; } }

			public ISpawnable Spawned { get { return m_Spawned; } }
			public SpawnState State { get { return m_State; } }
			public DateTime NextSpawn { get { return m_NextSpawn; } }

			#region Object Pooling

			private static Stack<SpawnInstance> m_Pool = new Stack<SpawnInstance>();

			public static SpawnInstance Instantiate( CreatureSpawner owner, bool deactivateOnRemove )
			{
				if ( m_Pool.Count > 0 )
				{
					SpawnInstance si = m_Pool.Pop();
					si.Initialize( owner, deactivateOnRemove );
					return si;
				}

				return new SpawnInstance( owner, deactivateOnRemove );
			}

			public void Free()
			{
				Despawn();

				m_Pool.Push( this );
			}

			#endregion

			#region Constructors

			private SpawnInstance( CreatureSpawner owner, bool deactivateOnRemove )
			{
				Initialize( owner, deactivateOnRemove );
			}

			private void Initialize( CreatureSpawner owner, bool deactivateOnRemove )
			{
				Initialize( owner, deactivateOnRemove, SpawnState.Inactive, null, null );
			}

			private void Initialize( CreatureSpawner owner, bool deactivateOnRemove, SpawnState initialState, ISpawnable initialSpawned, Timer initialTimer )
			{
				m_Owner = owner;
				m_DeactivateOnRemove = deactivateOnRemove;

				m_State = initialState;

				m_Spawned = initialSpawned;
				m_Timer = initialTimer;
			}

			#endregion

			public void Despawn()
			{
				switch ( m_State )
				{
					case SpawnState.Respawning:
						m_Timer.Stop();
						break;

					case SpawnState.Inactive:
						break;

					case SpawnState.Active:
						m_Spawned.Spawner = null; // prevent re-entrancy
						m_Spawned.Delete();
						break;
				}
			}

			public void Respawn()
			{
				Despawn();

				ISpawnable spawnable = m_Owner.Spawn();

				if ( spawnable != null )
				{
					spawnable.Spawner = this;

					m_Spawned = spawnable;
					m_State = SpawnState.Active;
				}

				m_Owner.OnEntityAdded( spawnable );
			}

			public void BringToHome()
			{
				if ( m_State == SpawnState.Active )
				{
					m_Spawned.MoveToWorld( this.HomeLocation, m_Spawned.Map as Map );
				}
			}

			public void Remove( ISpawnable spawnable )
			{
				/* In this implementation we can ignore the argument since we
				 * already know it. This is more like a signal function so. */

				m_Spawned = null;

				if ( m_DeactivateOnRemove )
				{
					m_State = SpawnState.Inactive;
				}
				else
				{
					m_State = SpawnState.Respawning;

					TimeSpan respawnDelay = GetRespawnDelay();

					m_NextSpawn = DateTime.UtcNow + respawnDelay;

					m_Timer = Timer.DelayCall( respawnDelay, Respawn );
					m_Timer.Start();
				}

				m_Owner.OnEntityRemoved( spawnable );
			}

			public void Replace( ISpawnable oldEntity, ISpawnable newEntity )
			{
				if ( m_State == SpawnState.Active )
				{
					m_Spawned = newEntity;
				}
			}

			public TimeSpan GetRespawnDelay()
			{
				int seconds = Utility.RandomMinMax( (int) m_Owner.MinDelay.TotalSeconds, (int) m_Owner.MaxDelay.TotalSeconds );

				// TODO: Spawn saturation

				if ( m_Owner.ScaledDelay )
					seconds /= Math.Max( 1, m_Owner.GetSurroundingPlayers() );

				if ( seconds < 5 )
					seconds = 5;

				return TimeSpan.FromSeconds( seconds );
			}

			#region Serialization

			public void Serialize( GenericWriter writer )
			{
				writer.Write( (int) m_State );
				writer.Write( m_DeactivateOnRemove );

				switch ( m_State )
				{
					case SpawnState.Inactive:
						break;

					case SpawnState.Active:
						writer.Write( m_Spawned.Serial );
						break;

					case SpawnState.Respawning:
						writer.Write( m_NextSpawn );
						break;
				}
			}

			public SpawnInstance( GenericReader reader, CreatureSpawner owner )
			{
				m_Owner = owner;

				m_State = (SpawnState) reader.ReadInt();
				m_DeactivateOnRemove = reader.ReadBool();

				switch ( m_State )
				{
					// TODO: It seems a good place to use the State pattern...

					case SpawnState.Inactive:
						break;

					case SpawnState.Active:
						m_Spawned = World.FindEntity( reader.ReadInt() ) as ISpawnable;

						if ( m_Spawned != null )
							m_Spawned.Spawner = this;
						else
							m_State = SpawnState.Inactive;

						break;

					case SpawnState.Respawning:
						m_NextSpawn = reader.ReadDateTime();

						TimeSpan delay = m_NextSpawn - DateTime.UtcNow;

						if ( delay.TotalMilliseconds < 0 )
							delay = TimeSpan.Zero;

						m_Timer = Timer.DelayCall( delay, Respawn );
						break;
				}
			}
			#endregion

			public Point3D HomeLocation { get { return m_Owner.Location; } }
			public Map Map { get { return m_Owner.Map; } }
			public int HomeRange { get { return m_Owner.HomeRange; } }

			public bool UnlinkOnTaming { get { return true; } }
		}

		#region Serialization

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 6 ); // version

			writer.Write( (int) m_Flags );

			if ( Group )
				writer.Write( m_NextGroupRespawn );

			writer.Write( m_WayPoint );
			writer.Write( m_MinDelay );
			writer.Write( m_MaxDelay );
			writer.Write( m_Count );
			writer.Write( m_Team );
			writer.Write( m_SpawnRange );
			writer.Write( m_HomeRange );
			writer.Write( m_Active );
			writer.Write( SpawnName );
			writer.Write( m_Instances.Count );

			foreach ( SpawnInstance instance in m_Instances )
			{
				instance.Serialize( writer );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 6:
					{
						m_Flags = (SpawnFlag) reader.ReadInt();

						if ( Group )
						{
							TimeSpan delay = reader.ReadDateTime() - DateTime.UtcNow;

							if ( delay.TotalMilliseconds < 0 )
								delay = TimeSpan.Zero;

							m_GroupRespawnTimer = Timer.DelayCall( delay, GroupRespawn );
						}

						goto case 5;
					}
				case 5:
					{
						if ( version < 6 )
							Murderer = reader.ReadBool();

						goto case 4;
					}
				case 4:
					{
						if ( version < 6 )
							Blessed = reader.ReadBool();

						goto case 3;
					}
				case 3:
					{
						if ( version < 6 )
						{
							ScaledDelay = reader.ReadBool();
							reader.ReadBool(); // despawned
							reader.ReadBool(); // player range sensitive
						}

						goto case 2;
					}
				case 2:
					{
						if ( version < 6 )
							CantWalk = reader.ReadBool();

						goto case 1;
					}
				case 1:
					{
						m_WayPoint = reader.ReadItem() as WayPoint;

						if ( version < 6 )
							Group = reader.ReadBool();

						m_MinDelay = reader.ReadTimeSpan();
						m_MaxDelay = reader.ReadTimeSpan();
						m_Count = reader.ReadInt(); // old count
						m_Team = reader.ReadInt();
						m_SpawnRange = reader.ReadInt();
						m_HomeRange = reader.ReadInt();
						m_Active = reader.ReadBool();

						m_SpawnType = ValidateType( reader.ReadString() );

						int instanceCount = 0;

						if ( version >= 3 )
							instanceCount = reader.ReadInt();

						m_Instances = new List<SpawnInstance>();

						for ( int i = 0; i < instanceCount; ++i )
						{
							SpawnInstance instance = new SpawnInstance( reader, this );
							m_Instances.Add( instance );
						}

						break;
					}
			}
		}

		#endregion

		#region XML Serialization
		public void XmlSerialize( XmlTextWriter xml )
		{
			xml.WriteStartElement( "Spawner" );

			xml.WriteStartElement( "Name" );
			xml.WriteString( SpawnName );
			xml.WriteEndElement();

			xml.WriteStartElement( "Count" );
			xml.WriteString( m_Count.ToString() );
			xml.WriteEndElement();

			xml.WriteStartElement( "MinDelay" );
			xml.WriteString( XmlConvert.ToString( m_MinDelay ) );
			xml.WriteEndElement();

			xml.WriteStartElement( "MaxDelay" );
			xml.WriteString( XmlConvert.ToString( m_MaxDelay ) );
			xml.WriteEndElement();

			xml.WriteStartElement( "SpawnRange" );
			xml.WriteString( m_SpawnRange.ToString() );
			xml.WriteEndElement();

			xml.WriteStartElement( "HomeRange" );
			xml.WriteString( m_HomeRange.ToString() );
			xml.WriteEndElement();

			if ( m_Team != 0 )
			{
				xml.WriteStartElement( "Team" );
				xml.WriteString( m_Team.ToString() );
				xml.WriteEndElement();
			}

			if ( !m_Active )
			{
				xml.WriteStartElement( "Active" );
				xml.WriteString( m_Active.ToString() );
				xml.WriteEndElement();
			}

			if ( m_Flags != SpawnFlag.None )
			{
				xml.WriteStartElement( "Flags" );

				foreach ( SpawnFlag flag in Enum.GetValues(typeof(SpawnFlag)).Cast<SpawnFlag>() )
				{
					if ( ( m_Flags & flag ) != 0 )
					{
						xml.WriteStartElement( flag.ToString() );
						xml.WriteEndElement();
					}
				}

				xml.WriteEndElement();
			}

			if ( m_WayPoint != null )
			{
				xml.WriteStartElement( "Waypoint" );
				// TODO: serialize m_Waypoint
				xml.WriteEndElement();
			}

			xml.WriteEndElement();
		}

		#endregion
	}
}
