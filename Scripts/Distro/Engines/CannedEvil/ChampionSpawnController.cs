using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Engines.CannedEvil;

namespace Server.Engines.CannedEvil
{
	public class ChampionSpawnController : Item
	{
		private bool m_Active;

		private ArrayList m_AllSpawn;
		private ArrayList m_DungeonsSpawn;
		private ArrayList m_LostLandsSpawn;
		private ArrayList m_IlshenarSpawn;
		private ArrayList m_TokunoSpawn;

		private int m_ActiveAltars;

		public struct SpawnRecord
		{
			public int type, x, y, z;

			public SpawnRecord( int type, int x, int y, int z )
			{
				this.type = type;
				this.x = x;
				this.y = y;
				this.z = z;
			}
		}

		private SpawnRecord[] m_Dungeons = new SpawnRecord[]
			{
				new SpawnRecord( (int) ChampionSpawnType.UnholyTerror, 5178, 708, 20 ),
				new SpawnRecord( (int) ChampionSpawnType.VerminHorde, 5558, 823, 65 ),
				new SpawnRecord( (int) ChampionSpawnType.ColdBlood, 5259, 837, 64 ),
				new SpawnRecord( (int) ChampionSpawnType.Abyss, 5813, 1350, 1 ),
				new SpawnRecord( (int) ChampionSpawnType.Arachnid, 5188, 1607, 20 ),
			};

		private SpawnRecord[] m_LostLands = new SpawnRecord[]
			{
				new SpawnRecord( 0xff, 5636, 2916, 37 ), // Desert
				new SpawnRecord( 0xff, 5724, 3991, 42 ), // Tortoise
				new SpawnRecord( 0xff, 5511, 2360, 40 ), // Ice West
				new SpawnRecord( 0xff, 5549, 2640, 15 ), // Oasis
				new SpawnRecord( 0xff, 6035, 2944, 52 ), // Terra Sanctum
				new SpawnRecord( (int) ChampionSpawnType.ForestLord, 5559, 3757, 21 ), // Lord Oaks
				new SpawnRecord( 0xff, 5267, 3171, 104 ), // Marble
				new SpawnRecord( 0xff, 5954, 3475, 25 ), // Hoppers Boog
				new SpawnRecord( 0xff, 5982, 3882, 20 ), // Khaldun
				new SpawnRecord( 0xff, 6038, 2400, 46 ), // Ice East
				new SpawnRecord( 0xff, 5281, 3368, 51 ), // Damwin
				new SpawnRecord( 0xff, 5207, 3637, 20 ), // City of Death
			};

		private SpawnRecord[] m_Ilshenar = new SpawnRecord[]
			{
				new SpawnRecord( 0xff, 383, 329, -30 ), // Valor
				new SpawnRecord( 0xff, 462, 926, -67 ), // Humility
				new SpawnRecord( (int) ChampionSpawnType.ForestLord, 1645, 1108, 8 ), // Spirituality
			};

		private SpawnRecord[] m_Tokuno = new SpawnRecord[]
			{
				new SpawnRecord( (int) ChampionSpawnType.SleepingDragon, 947, 436, 29 ), // Isamu Jima
			};

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
		public int ActiveAltars { get { return m_ActiveAltars; } set { m_ActiveAltars = value; } }

		[Constructable]
		public ChampionSpawnController()
			: base( 0x1B7A )
		{
			if ( Check() )
			{
				World.Broadcast( 0x35, true, "Another champion spawn controller exist in the world !" );
				Delete();
				return;
			}

			Visible = false;
			Movable = false;
			Name = "champion spawn controller";

			m_Active = false;

			m_AllSpawn = new ArrayList();
			m_DungeonsSpawn = new ArrayList();
			m_LostLandsSpawn = new ArrayList();
			m_IlshenarSpawn = new ArrayList();
			m_TokunoSpawn = new ArrayList();

			m_ActiveAltars = 1;

			DeleteAll();
			Generate();

			World.Broadcast( 0x35, true, "Champion spawn generation complete. Old spawns removed." );

			Start();
		}

		private bool Check()
		{
			foreach ( Item item in World.Instance.Items )
				if ( item is ChampionSpawnController && !item.Deleted && item != this )
					return true;

			return false;
		}

		private void DeleteAll()
		{
			ArrayList list = new ArrayList();

			foreach ( Item item in World.Instance.Items )
			{
				if ( item is ChampionSpawn && !item.Deleted )
					list.Add( item );
			}

			foreach ( ChampionSpawn cs in list )
				cs.Delete();
		}

		private ChampionSpawn CreateAltar( SpawnRecord r, Map m, bool restartdisable )
		{
			ChampionSpawn cs = new ChampionSpawn();

			Point3D loc = new Point3D( r.x, r.y, r.z );

			if ( r.type == 0xff )
			{
				cs.RandomizeType = true;

				switch ( Utility.Random( 5 ) )
				{
					case 0:
						cs.Type = ChampionSpawnType.VerminHorde;
						break;
					case 1:
						cs.Type = ChampionSpawnType.UnholyTerror;
						break;
					case 2:
						cs.Type = ChampionSpawnType.ColdBlood;
						break;
					case 3:
						cs.Type = ChampionSpawnType.Abyss;
						break;
					case 4:
						cs.Type = ChampionSpawnType.Arachnid;
						break;
				}
			}
			else
			{
				cs.RandomizeType = false;
				cs.Type = (ChampionSpawnType) r.type;
			}

			// Prevent autorestart of felucca & t2a the spawns

			if ( restartdisable )
				cs.RestartDelay = TimeSpan.FromDays( 9999 );

			cs.MoveToWorld( loc, m );

			return cs;
		}

		private void Generate()
		{
			int i = 0;

			for ( i = 0; i < m_Dungeons.Length; i++ )
			{
				ChampionSpawn cs = CreateAltar( m_Dungeons[i], Map.Felucca, true );

				m_AllSpawn.Add( cs );
				m_DungeonsSpawn.Add( cs );
			}

			for ( i = 0; i < m_LostLands.Length; i++ )
			{
				ChampionSpawn cs = CreateAltar( m_LostLands[i], Map.Felucca, true );

				m_AllSpawn.Add( cs );
				m_LostLandsSpawn.Add( cs );
			}

			for ( i = 0; i < m_Ilshenar.Length; i++ )
			{
				ChampionSpawn cs = CreateAltar( m_Ilshenar[i], Map.Ilshenar, false );

				m_IlshenarSpawn.Add( cs );
				m_AllSpawn.Add( cs );
			}

			for ( i = 0; i < m_Tokuno.Length; i++ )
			{
				ChampionSpawn cs = CreateAltar( m_Tokuno[i], Map.Tokuno, false );

				m_TokunoSpawn.Add( cs );
				m_AllSpawn.Add( cs );
			}
		}

		public ChampionSpawnController( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Active )
				list.Add( 1060742 ); // active
			else
				list.Add( 1060743 ); // inactive
		}

		public override void OnDelete()
		{
			Stop();

			if ( m_AllSpawn != null )
			{
				foreach ( ChampionSpawn cs in m_AllSpawn )
				{
					if ( !cs.Deleted )
						cs.Delete();
				}
			}

			base.OnDelete();
		}

		public void Start()
		{
			if ( m_Active || Deleted )
				return;

			m_Active = true;

			Randomize( m_DungeonsSpawn );
			Randomize( m_LostLandsSpawn );

			foreach ( ChampionSpawn cs in m_IlshenarSpawn )
			{
				if ( !cs.Deleted )
					cs.Active = true;
			}

			foreach ( ChampionSpawn cs in m_TokunoSpawn )
			{
				if ( !cs.Deleted )
					cs.Active = true;
			}
		}

		public void Stop()
		{
			if ( !m_Active || Deleted )
				return;

			m_Active = false;

			foreach ( ChampionSpawn cs in m_AllSpawn )
			{
				if ( !cs.Deleted && cs.Active )
				{
					cs.Active = false;
					cs.HasBeenAdvanced = false;
				}
			}
		}

		public void Randomize( ArrayList list )
		{
			foreach ( ChampionSpawn cs in list )
			{
				if ( !cs.Deleted && cs.Active && cs.Level == 0 )
					cs.Active = false;
			}

			for ( int i = 0; i < m_ActiveAltars; i++ )
			{
				int trynum = 0;

				while ( trynum < 7 )
				{
					int index = Utility.RandomMinMax( 0, list.Count - 1 );

					if ( !( (ChampionSpawn) list[index] ).Active )
					{
						( (ChampionSpawn) list[index] ).Active = true;
						break;
					}

					trynum++;
				}
			}
		}

		private void Slice()
		{
			foreach ( ChampionSpawn cs in m_AllSpawn )
			{
				if ( cs.Level == 0 ) // Ignora el altar si hay alguien combatiendo
					cs.HasBeenAdvanced = false;
			}

			Randomize( m_DungeonsSpawn );
			Randomize( m_LostLandsSpawn );
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendGump( new PropertiesGump( from, this ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version

			writer.Write( m_Active );

			writer.WriteItemList( m_AllSpawn );
			writer.WriteItemList( m_DungeonsSpawn );
			writer.WriteItemList( m_LostLandsSpawn );
			writer.WriteItemList( m_IlshenarSpawn );
			writer.WriteItemList( m_TokunoSpawn );

			writer.Write( m_ActiveAltars );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */
			reader.ReadInt();

			m_Active = reader.ReadBool();

			m_AllSpawn = reader.ReadItemList();
			m_DungeonsSpawn = reader.ReadItemList();
			m_LostLandsSpawn = reader.ReadItemList();
			m_IlshenarSpawn = reader.ReadItemList();
			m_TokunoSpawn = reader.ReadItemList();

			m_ActiveAltars = reader.ReadInt();

			if ( m_Active )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( Slice ) );
		}
	}
}