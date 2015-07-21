using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Engines.MLQuests;

namespace Server.Items
{
	public enum MovementSpawnType
	{
		// Citadel
		RandomSect,
		TigersClawSect,
		SerpentsFangSect,
		DragonsFlameSect,

		// Twisted Weald
		FourBlackWidows,
		SilkAndThreeWidows,
		MaleficAndVirulent,

		// Blighted Grove
		InsaneDryad,

		//Labyrinth
		Grim,
		Tempest,
		Flurry,
		Mistral
	}

	/*public class GenCitadel
	{
		public static void Initialize()
		{
			Server.Commands.Register( "GenCitadel", AccessLevel.Developer, new CommandEventHandler( GenCitadel_Command ) );
		}

		private static void GenCitadel_Command( CommandEventArgs e )
		{
			AddSecretDoorAndSwitch( new Point3D( 75, 1871, 0 ), new Point3D( 72, 1878, 10 ), new Point3D( 123, 1902, 0 ), false, false );
			AddSecretDoor( new Point3D( 123, 1903, 0 ), new Point3D( 75, 1872, 0 ), false );

			AddSecretDoor( new Point3D( 71, 1889, 0 ), new Point3D( 72, 1909, 0 ), true );
			AddSecretDoor( new Point3D( 73, 1909, 0 ), new Point3D( 72, 1889, 0 ), true );

			AddSecretDoor( new Point3D( 123, 1926, 0 ), new Point3D( 136, 1928, 0 ), true );
			AddSecretDoor( new Point3D( 135, 1928, 0 ), new Point3D( 122, 1926, 0 ), true );

			AddSecretDoor( new Point3D( 103, 1891, 0 ), new Point3D( 101, 1916, 0 ), true );
			AddSecretDoor( new Point3D( 100, 1916, 0 ), new Point3D( 102, 1891, 0 ), true );

			AddSecretDoor( new Point3D( 155, 1874, 0 ), new Point3D( 140, 1860, 0 ), false );
			AddSecretDoor( new Point3D( 140, 1859, 0 ), new Point3D( 155, 1873, 0 ), false );

			AddSecretDoor( new Point3D( 175, 1913, 0 ), new Point3D( 143, 1947, 0 ), true );
			AddSecretDoor( new Point3D( 144, 1947, 0 ), new Point3D( 176, 1913, 0 ), true );

			AddSecretDoor( new Point3D( 173, 1880, 0 ), new Point3D( 182, 1947, 0 ), true );
		
			AddSpawner( new Point3D( 82, 1892, 0 ), MovementSpawnType.RandomSect );
			AddSpawner( new Point3D( 82, 1875, 0 ), MovementSpawnType.RandomSect );
			AddSpawner( new Point3D( 183, 1897, 0 ), MovementSpawnType.RandomSect );
			AddSpawner( new Point3D( 145, 1899, 0 ), MovementSpawnType.RandomSect );
			AddSpawner( new Point3D( 156, 1869, 0 ), MovementSpawnType.RandomSect );
			AddSpawner( new Point3D( 141, 1974, 0 ), MovementSpawnType.RandomSect );
			AddSpawner( new Point3D( 132, 1947, 0 ), MovementSpawnType.RandomSect );
			AddSpawner( new Point3D( 140, 1948, 0 ), MovementSpawnType.DragonsFlameSect );
			AddSpawner( new Point3D( 170, 1975, 0 ), MovementSpawnType.SerpentsFangSect );
			AddSpawner( new Point3D( 143, 1920, 0 ), MovementSpawnType.TigersClawSect );

			AltarPeerless altar = new AltarPeerless();
			altar.Peerless = PeerlessList.Travesty;
			altar.MoveToWorld( new Point3D( 91, 1884, 0 ), Map.Malas );

			e.Mobile.SendMessage( "Citadel secret doors, switches and spawners have been generated." );
		}

		private static void AddSpawner( Point3D _spawnerlocation, MovementSpawnType _spawntype )
		{
			MovementSpawner _spawner = new MovementSpawner();
			_spawner.MoveToWorld( _spawnerlocation, Map.Malas );
			_spawner.SpawnType = _spawntype;
		}

		private static void AddSecretDoor( Point3D _doorlocation, Point3D _doordestination, bool _flipdoor )
		{
			CitadelSecretDoor _door = new CitadelSecretDoor();
			_door.MoveToWorld( _doorlocation, Map.Malas );
			_door.Destination = _doordestination;

			if ( _flipdoor )
				_door.ItemID = 0x2949;
		}

		private static void AddSwitch( Point3D _switchlocation, bool _flipswitch )
		{
			CitadelSwitch _switch = new CitadelSwitch();
			_switch.MoveToWorld( _switchlocation, Map.Malas );

			if ( _flipswitch )
				_switch.ItemID = 0x1091;
		}

		private static void AddSecretDoorAndSwitch( Point3D _doorlocation, Point3D _switchlocation, Point3D _doordestination, bool _flipdoor, bool _flipswitch )
		{
			CitadelSecretDoor _door = new CitadelSecretDoor();
			_door.MoveToWorld( _doorlocation, Map.Malas );
			_door.Destination = _doordestination;

			CitadelSwitch _switch = new CitadelSwitch();
			_switch.MoveToWorld( _switchlocation, Map.Malas );

			_door.Switch = _switch;
			_switch.Door = _door;

			if ( _flipdoor )
				_door.ItemID = 0x2949;

			if ( _flipswitch )
				_switch.ItemID = 0x1091;
		}
	}*/

	[FlipableAttribute( 0x294F, 0x2949 )]
	public class CitadelSecretDoor : Item
	{
		private Point3D m_Destination;
		private CitadelSwitch m_Switch;

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Destination { get { return m_Destination; } set { m_Destination = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public CitadelSwitch Switch { get { return m_Switch; } set { m_Switch = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Unlocked
		{
			get
			{
				if ( m_Switch == null )
					return true;

				return m_Switch.ON;
			}
		}

		[Constructable]
		public CitadelSecretDoor()
			: base( 0x294F )
		{
			Movable = false;
		}

		public CitadelSecretDoor( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InLOS( this.GetWorldLocation() ) )
			{
				from.SendLocalizedMessage( 502800 ); // You can't see that.
				return;
			}

			if ( from.GetDistanceToSqrt( this.GetWorldLocation() ) > 2 )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			if ( !Unlocked && from.AccessLevel == AccessLevel.Player )
			{
				this.PublicOverheadMessage( MessageType.Regular, 0x3B2, 500788 ); // This door appears to be locked.
				return;
			}

			BaseCreature.TeleportPets( from, m_Destination, Map.Malas );
			from.MoveToWorld( m_Destination, Map.Malas );

			from.SendLocalizedMessage( 1072790 ); // The wall becomes transparent, and you push your way through it.
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Destination );
			writer.Write( m_Switch );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Destination = reader.ReadPoint3D();
			m_Switch = reader.ReadItem() as CitadelSwitch;
		}
	}

	[FlipableAttribute( 0x108F, 0x1090, 0x1091, 0x1092 )]
	public class CitadelSwitch : Item
	{
		private CitadelSecretDoor m_Door;

		[CommandProperty( AccessLevel.GameMaster )]
		public CitadelSecretDoor Door { get { return m_Door; } set { m_Door = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ON
		{
			get
			{
				return ItemID == 0x1090 || ItemID == 0x1092;
			}
		}

		[Constructable]
		public CitadelSwitch()
			: base( 0x108F )
		{
			Movable = false;
		}

		public CitadelSwitch( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InLOS( this.GetWorldLocation() ) )
			{
				from.SendLocalizedMessage( 502800 ); // You can't see that.
				return;
			}

			if ( from.GetDistanceToSqrt( this.GetWorldLocation() ) > 1 )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			if ( ItemID == 0x108F )
				ItemID = 0x1090;
			else if ( ItemID == 0x1090 )
				ItemID = 0x108F;
			else if ( ItemID == 0x1091 )
				ItemID = 0x1092;
			else if ( ItemID == 0x1092 )
				ItemID = 0x1091;

			Effects.PlaySound( Location, Map, 0x3E8 );

			from.SendLocalizedMessage( 1072739 ); // You hear a click behind the wall.
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Door );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Door = reader.ReadItem() as CitadelSecretDoor;
		}
	}

	public class CitadelEntranceCrate : Container
	{
		[Constructable]
		public CitadelEntranceCrate()
			: base( 0xE3F )
		{
			Weight = 0.0;
			Movable = false;
		}

		public CitadelEntranceCrate( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.Alive )
				return;

			if ( !from.InLOS( this.GetWorldLocation() ) )
			{
				from.SendLocalizedMessage( 502800 ); // You can't see that.
			}
			else if ( from.GetDistanceToSqrt( this.GetWorldLocation() ) > 2 )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
			else
			{
				Point3D dest = new Point3D( 107, 1883, 0 );

				BaseCreature.TeleportPets( from, dest, Map.Malas );
				from.MoveToWorld( dest, Map.Malas );

				from.SendLocalizedMessage( 1072730 ); // The manor lord is unavailable. Please use the teleporter on your right.
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Items.CitadelSpawner" )]
	public class MovementSpawner : Item
	{
		private static readonly TimeSpan DespawnTime = TimeSpan.FromMinutes( 30.0 );
		private static readonly int DefaultRange = 5;

		private DateTime m_NextSpawn;
		private MovementSpawnType m_SpawnType;
		private int m_Range;
		private Timer m_DespawnTimer;
		private ArrayList m_Creatures;
		private bool m_Active;

		[CommandProperty( AccessLevel.GameMaster )]
		public MovementSpawnType SpawnType
		{
			get { return m_SpawnType; }
			set { m_SpawnType = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; }
		}

		[Constructable]
		public MovementSpawner()
			: base( 0x1F13 )
		{
			Name = "Movement Spawner";
			Movable = false;
			Visible = false;

			m_Creatures = new ArrayList();

			m_NextSpawn = DateTime.Now;
			m_Range = DefaultRange;
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );

			if ( !m_Active )
				return;

			if ( !( m is PlayerMobile ) )
				return;

			if ( !m.Alive || m.AccessLevel >= AccessLevel.Counselor )
				return;

			foreach ( BaseCreature bc in m_Creatures )
			{
				if ( bc != null && !bc.Deleted )
					return;
			}

			if ( DateTime.Now < m_NextSpawn )
				return;

			if ( m.InRange( this.Location, m_Range ) /*&& ( m.Direction & Direction.Running ) != 0*/ )
				Spawn( m );
		}

		public void Spawn( Mobile summoner )
		{
			int msg = -1;

			ArrayList creatures = new ArrayList();

			switch ( m_SpawnType )
			{
				default:
				case MovementSpawnType.RandomSect:
					{
						msg = 1072732; // Your presence here has not gone unnoticed. Citadel protectors spring forth from the shadows!
						creatures.Add( new BlackOrderAssassin() );
						creatures.Add( new BlackOrderMage() );
						creatures.Add( new BlackOrderThief() );
						creatures.Add( new EliteNinja() );
						break;
					}
				case MovementSpawnType.DragonsFlameSect:
					{
						msg = 1072734; // The floor crackles beneath your feet, alerting the protectors. The Flame of the Dragon is upon you!
						creatures.Add( new BlackOrderMage() );
						creatures.Add( new BlackOrderMage() );
						creatures.Add( new BlackOrderMage() );
						creatures.Add( new BlackOrderGrandMage() );
						break;
					}
				case MovementSpawnType.SerpentsFangSect:
					{
						msg = 1072736; // The Serpent's Fang Sect leaps from the shadows with their blades drawn!
						creatures.Add( new BlackOrderAssassin() );
						creatures.Add( new BlackOrderAssassin() );
						creatures.Add( new BlackOrderAssassin() );
						creatures.Add( new BlackOrderHighExecutioner() );
						break;
					}
				case MovementSpawnType.TigersClawSect:
					{
						msg = 1072735; // From the dark corners spring the Claw of the Tiger.
						creatures.Add( new BlackOrderThief() );
						creatures.Add( new BlackOrderThief() );
						creatures.Add( new BlackOrderThief() );
						creatures.Add( new BlackOrderMasterThief() );
						break;
					}
				case MovementSpawnType.FourBlackWidows:
					{
						msg = 1073858; // Without warning, several spiders come out of hiding and attack you!
						creatures.Add( new GiantBlackWidow() );
						creatures.Add( new GiantBlackWidow() );
						creatures.Add( new GiantBlackWidow() );
						creatures.Add( new GiantBlackWidow() );
						break;
					}
				case MovementSpawnType.SilkAndThreeWidows:
					{
						msg = 1073858; // Without warning, several spiders come out of hiding and attack you!
						creatures.Add( new Silk() );
						creatures.Add( new GiantBlackWidow() );
						creatures.Add( new GiantBlackWidow() );
						creatures.Add( new GiantBlackWidow() );
						break;
					}
				case MovementSpawnType.MaleficAndVirulent:
					{
						msg = 1073857; // As you approach the cocoon, two dread spiders climb down the trees and attack you!
						creatures.Add( new Malefic() );
						creatures.Add( new Virulent() );
						break;
					}
				case MovementSpawnType.InsaneDryad:
					{
						msg = 1074164; // An insane dryad comes out of a nearby tree and attacks you!
						creatures.Add( new InsaneDryad() );
						break;
					}
				case MovementSpawnType.Grim:
					{
						creatures.Add( new Grim() );
						break;
					}
				case MovementSpawnType.Tempest:
					{
						creatures.Add( new Tempest() );
						break;
					}
				case MovementSpawnType.Mistral:
					{
						creatures.Add( new Mistral() );
						break;
					}
				case MovementSpawnType.Flurry:
					{
						creatures.Add( new Flurry() );
						break;
					}
			}

			foreach ( BaseCreature bc in creatures )
			{
				m_Creatures.Add( bc );

				bc.OnAfterSpawn();
				bc.MoveToWorld( this.Location, this.Map );

				bc.Combatant = summoner;
			}

			if ( msg != -1 )
				summoner.SendLocalizedMessage( msg );

			if ( m_DespawnTimer != null )
				m_DespawnTimer.Stop();

			m_DespawnTimer = Timer.DelayCall( DespawnTime, new TimerCallback( Despawn ) );

			m_NextSpawn = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 300, 600 ) );
		}

		public void Despawn()
		{
			foreach ( BaseCreature bc in m_Creatures )
			{
				if ( bc != null && !bc.Deleted )
					bc.Delete();
			}

			m_Creatures.Clear();
		}

		public MovementSpawner( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (bool) m_Active );
			writer.Write( (int) m_Range );
			writer.Write( (int) m_SpawnType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Active = reader.ReadBool();
						goto case 0;
					}
				case 0:
					{
						m_Range = reader.ReadInt();
						m_SpawnType = (MovementSpawnType) reader.ReadInt();

						break;
					}
			}

			m_Creatures = new ArrayList();

			if ( version < 1 )
				m_Active = true;
		}
	}
}