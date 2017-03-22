using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	public class Harrower : BaseCreature
	{
		public Type[] UniqueList { get { return new Type[] { typeof( AcidProofRobe ) }; } }
		public Type[] SharedList { get { return new Type[] { typeof( TheRobeOfBritanniaAri ) }; } }
		public Type[] DecorativeList { get { return new Type[] { typeof( EvilIdolSkull ), typeof( SkullPole ) }; } }

		private bool m_TrueForm;
		private Item m_GateItem;
		private ArrayList m_Tentacles;
		private Timer m_Timer;

		private class SpawnEntry
		{
			public Point3D m_Location;
			public Point3D m_Entrance;

			public SpawnEntry( Point3D loc, Point3D ent )
			{
				m_Location = loc;
				m_Entrance = ent;
			}
		}

		private static SpawnEntry[] m_Entries = new SpawnEntry[]
			{
				new SpawnEntry( new Point3D( 5284, 798, 0 ), new Point3D( 1176, 2638, 0 ) ),// Destard
				new SpawnEntry( new Point3D( 5469, 1855, 0 ), new Point3D( 2499, 921, 0 ) ),// Cove
				new SpawnEntry( new Point3D( 5175, 615, 0 ), new Point3D( 4111, 435, 5 ) ),// Deceit
				new SpawnEntry( new Point3D( 5825, 593, 0 ), new Point3D( 2043, 238, 10 ) ),// Wrong
				new SpawnEntry( new Point3D( 5922, 33, 44 ), new Point3D( 4721, 3824, 0 ) ),// Hythloth
				new SpawnEntry( new Point3D( 5485, 573, 60 ), new Point3D( 1302, 1081, 0 ) ),// Despise
				new SpawnEntry( new Point3D( 5404, 90, 10 ), new Point3D( 513, 1564, 0 ) ),// Shame
				new SpawnEntry( new Point3D( 5768, 1427, 27 ), new Point3D( 5763, 2908, 15 ) ),// Fire
				new SpawnEntry( new Point3D( 5851, 172, -2 ), new Point3D( 5213, 2322, 29 ) ),// Ice
				new SpawnEntry( new Point3D( 5475, 3117, -40 ), new Point3D( 5451, 3143,-60 ) )// Theratan Keep				
			};

		private static ArrayList m_Instances = new ArrayList();

		public static ArrayList Instances { get { return m_Instances; } }

		public static Harrower Spawn( Point3D platLoc, Map platMap )
		{
			if ( m_Instances.Count > 0 )
				return null;

			SpawnEntry entry = m_Entries[Utility.Random( m_Entries.Length )];

			Harrower harrower = new Harrower();

			harrower.MoveToWorld( entry.m_Location, Map.Felucca );

			harrower.m_GateItem = new HarrowerGate( harrower, platLoc, platMap, entry.m_Entrance, Map.Felucca );

			return harrower;
		}

		public static bool CanSpawn
		{
			get
			{
				return ( m_Instances.Count == 0 );
			}
		}

		[Constructable]
		public Harrower()
			: base( AIType.AI_Necromancer, FightMode.Closest, 18, 1, 0.2, 0.4 )
		{
			m_Instances.Add( this );

			Name = "the harrower";
			BodyValue = 146;

			SetStr( 900, 1000 );
			SetDex( 125, 135 );
			SetInt( 1000, 1200 );

			SetFameLevel( 5 );
			SetKarmaLevel( 5 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 70, 80 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 70, 80 );

			SetSkill( SkillName.Wrestling, 93.9, 96.5 );
			SetSkill( SkillName.Tactics, 96.9, 102.2 );
			SetSkill( SkillName.MagicResist, 131.4, 140.8 );
			SetSkill( SkillName.Magery, 156.2, 161.4 );
			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Meditation, 120.0 );

			m_Tentacles = new ArrayList();

			m_Timer = new TeleportTimer( this );
			m_Timer.Start();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
			AddLoot( LootPack.Meager );
		}

		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		private double[] m_Offsets = new double[]
			{
				Math.Cos( 000.0 / 180.0 * Math.PI ), Math.Sin( 000.0 / 180.0 * Math.PI ),
				Math.Cos( 040.0 / 180.0 * Math.PI ), Math.Sin( 040.0 / 180.0 * Math.PI ),
				Math.Cos( 080.0 / 180.0 * Math.PI ), Math.Sin( 080.0 / 180.0 * Math.PI ),
				Math.Cos( 120.0 / 180.0 * Math.PI ), Math.Sin( 120.0 / 180.0 * Math.PI ),
				Math.Cos( 160.0 / 180.0 * Math.PI ), Math.Sin( 160.0 / 180.0 * Math.PI ),
				Math.Cos( 200.0 / 180.0 * Math.PI ), Math.Sin( 200.0 / 180.0 * Math.PI ),
				Math.Cos( 240.0 / 180.0 * Math.PI ), Math.Sin( 240.0 / 180.0 * Math.PI ),
				Math.Cos( 280.0 / 180.0 * Math.PI ), Math.Sin( 280.0 / 180.0 * Math.PI ),
				Math.Cos( 320.0 / 180.0 * Math.PI ), Math.Sin( 320.0 / 180.0 * Math.PI ),
			};

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			int message = Utility.RandomList( 1049501, 1049502, 1049504, 1049508 );

			Say( message );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			int message = Utility.RandomList( 1049500, 1049505, 1049507, 1049509 );

			Say( message );
		}

		public override void OnDamagedBySpell( Mobile from )
		{
			int message = Utility.RandomList( 1049501, 1049502, 1049504, 1049508 );

			Say( message );
		}

		public void Morph()
		{
			if ( m_TrueForm )
				return;

			m_TrueForm = true;

			Name = "the true harrower";
			BodyValue = 780;
			Hue = 0x497;

			Hits = HitsMax;
			Stam = StamMax;
			Mana = ManaMax;

			ProcessDelta();

			Say( 1049499 ); // Behold my true form!

			Map map = this.Map;

			if ( map != null )
			{
				for ( int i = 0; i < m_Offsets.Length; i += 2 )
				{
					double rx = m_Offsets[i];
					double ry = m_Offsets[i + 1];

					int dist = 0;
					bool ok = false;
					int x = 0, y = 0, z = 0;

					while ( !ok && dist < 10 )
					{
						int rdist = 10 + dist;

						x = this.X + (int) ( rx * rdist );
						y = this.Y + (int) ( ry * rdist );
						z = map.GetAverageZ( x, y );

						if ( !( ok = map.CanFit( x, y, this.Z, 16, false, false ) ) )
							ok = map.CanFit( x, y, z, 16, false, false );

						if ( dist >= 0 )
							dist = -( dist + 1 );
						else
							dist = -( dist - 1 );
					}

					if ( !ok )
						continue;

					BaseCreature spawn = new HarrowerTentacles( this );

					spawn.Team = this.Team;

					spawn.MoveToWorld( new Point3D( x, y, z ), map );

					m_Tentacles.Add( spawn );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax { get { return m_TrueForm ? 65000 : 30000; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax { get { return 5000; } }

		public Harrower( Serial serial )
			: base( serial )
		{
			m_Instances.Add( this );
		}

		public override void OnAfterDelete()
		{
			m_Instances.Remove( this );

			base.OnAfterDelete();
		}

		public override bool DisallowAllMoves { get { return m_TrueForm; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_TrueForm );
			writer.Write( m_GateItem );
			writer.WriteMobileList( m_Tentacles );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_TrueForm = reader.ReadBool();
						m_GateItem = reader.ReadItem();
						m_Tentacles = reader.ReadMobileList();

						m_Timer = new TeleportTimer( this );
						m_Timer.Start();

						break;
					}
			}
		}

		public void GiveRewards()
		{
			ArrayList toGive = new ArrayList();
			List<DamageStore> rights = BaseCreature.GetLootingRights( this.DamageEntries, this.HitsMax );

			for ( int i = rights.Count - 1; i >= 0; --i )
			{
				DamageStore ds = rights[i];

				if ( ds.HasRight )
					toGive.Add( ds.Mobile );
			}

			if ( toGive.Count == 0 )
				return;

			// Randomize
			for ( int i = 0; i < toGive.Count; ++i )
			{
				int rand = Utility.Random( toGive.Count );
				object hold = toGive[i];
				toGive[i] = toGive[rand];
				toGive[rand] = hold;
			}

			GivePowerScrolls( toGive );

			GiveValor( toGive );

			GiveTitles( toGive );
		}

		public void GivePowerScrolls( ArrayList toGive )
		{
			if ( NoKillAwards )
				return;

			for ( int i = 0; i < toGive.Count; i++ )
			{
				Mobile m = (Mobile) toGive[i];

				if ( !m.Alive && m.Corpse == null )
					toGive.Remove( m );
			}

			for ( int i = 0; i < 16; ++i )
			{
				int level;
				double random = Utility.RandomDouble();

				if ( 0.1 >= random )
					level = 25;
				else if ( 0.25 >= random )
					level = 20;
				else if ( 0.45 >= random )
					level = 15;
				else if ( 0.70 >= random )
					level = 10;
				else
					level = 5;

				Mobile m = (Mobile) toGive[i % toGive.Count];

				StatCapScroll scs = new StatCapScroll( 225 + level );

				m.SendLocalizedMessage( 1049524 ); // You have received a scroll of power!

				if ( m.Alive )
				{
					m.AddToBackpack( scs );
				}
				else
				{
					Container corpse = m.Corpse;

					if ( corpse != null )
						corpse.DropItem( scs );
				}

				if ( m is PlayerMobile )
				{
					PlayerMobile pm = (PlayerMobile) m;

					for ( int j = 0; j < pm.JusticeProtectors.Count; ++j )
					{
						Mobile prot = (Mobile) pm.JusticeProtectors[j];

						if ( prot.Map != m.Map || prot.Kills >= 5 || prot.Criminal || !JusticeVirtue.CheckMapRegion( m, prot ) )
							continue;

						int chance = 0;

						switch ( VirtueHelper.GetLevel( prot, VirtueName.Justice ) )
						{
							case VirtueLevel.Seeker: chance = 60; break;
							case VirtueLevel.Follower: chance = 80; break;
							case VirtueLevel.Knight: chance = 100; break;
						}

						if ( chance > Utility.Random( 100 ) )
						{
							prot.SendLocalizedMessage( 1049368 ); // You have been rewarded for your dedication to Justice!
							prot.AddToBackpack( new StatCapScroll( 225 + level ) );
						}
					}
				}
			}
		}

		public void GiveTitles( ArrayList toGive )
		{
			for ( int i = 0; i < toGive.Count; ++i )
			{
				PlayerMobile pm = (PlayerMobile) toGive[i % toGive.Count];

				pm.SuperChampionTiers[0]++; // TODO: verify
			}
		}

		public void GiveValor( ArrayList toGive )
		{
			for ( int i = 0; i < toGive.Count; ++i )
			{
				Mobile m = (Mobile) toGive[i];

				if ( !( m is PlayerMobile ) )
					continue;

				bool gainedPath = false;

				int pointsToGain = 4000;

				if ( VirtueHelper.Award( m, VirtueName.Valor, pointsToGain, ref gainedPath ) )
				{
					if ( gainedPath )
						m.SendLocalizedMessage( 1054032 ); // You have gained a path in Valor!
					else
						m.SendLocalizedMessage( 1054030 ); // You have gained in Valor!

					// No delay on Valor gains
				}
				else
				{
					m.SendLocalizedMessage( 1054031 ); // You have achieved the highest path in Valor and can no longer gain any further.
				}
			}
		}

		public override bool OnBeforeDeath()
		{
			if ( m_TrueForm )
			{
				GiveRewards();

				if ( !NoKillAwards )
				{
					Map map = this.Map;

					if ( map != null )
					{
						for ( int x = -16; x <= 16; ++x )
						{
							for ( int y = -16; y <= 16; ++y )
							{
								double dist = Math.Sqrt( x * x + y * y );

								if ( dist <= 16 )
									new GoodiesTimer( map, X + x, Y + y ).Start();
							}
						}
					}

					m_DamageEntries = new Dictionary<Mobile, int>();

					for ( int i = 0; i < m_Tentacles.Count; ++i )
					{
						Mobile m = (Mobile) m_Tentacles[i];

						if ( !m.Deleted )
							m.Kill();

						RegisterDamageTo( m );
					}

					m_Tentacles.Clear();

					RegisterDamageTo( this );
					AwardArtifact( GetArtifact() );

					if ( m_GateItem != null )
						m_GateItem.Delete();
				}

				return base.OnBeforeDeath();
			}
			else
			{
				Morph();
				return false;
			}
		}

		Dictionary<Mobile, int> m_DamageEntries;

		public virtual void RegisterDamageTo( Mobile m )
		{
			if ( m == null )
				return;

			foreach ( DamageEntry de in m.DamageEntries )
			{
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

			//from.SendMessage( String.Format( "Total Damage: {0}", m_DamageEntries[from] ) );
		}

		public void AwardArtifact( Item artifact )
		{
			if ( artifact == null )
				return;

			int totalDamage = 0;

			Dictionary<Mobile, int> validEntries = new Dictionary<Mobile, int>();

			foreach ( KeyValuePair<Mobile, int> kvp in m_DamageEntries )
			{
				if ( IsEligable( kvp.Key, artifact ) )
				{
					validEntries.Add( kvp.Key, kvp.Value );
					totalDamage += kvp.Value;
				}
			}

			int randomDamage = Utility.RandomMinMax( 1, totalDamage );

			totalDamage = 0;

			foreach ( KeyValuePair<Mobile, int> kvp in m_DamageEntries )
			{
				totalDamage += kvp.Value;

				if ( totalDamage > randomDamage )
				{
					GiveArtifact( kvp.Key, artifact );
					break;
				}
			}
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

		public bool IsEligable( Mobile m, Item Artifact )
		{
			return m.Player && m.Alive && m.InRange( Location, 32 ) && m.Backpack != null && m.Backpack.CheckHold( m, Artifact, false );
		}

		public Item GetArtifact()
		{
			double random = Utility.RandomDouble();
			if ( 0.05 >= random )
				return CreateArtifact( UniqueList );
			else if ( 0.15 >= random )
				return CreateArtifact( SharedList );
			else if ( 0.30 >= random )
				return CreateArtifact( DecorativeList );
			return null;
		}

		public Item CreateArtifact( Type[] list )
		{
			if ( list.Length == 0 )
				return null;

			int random = Utility.Random( list.Length );

			Type type = list[random];

			return Loot.Construct( type );
		}

		private class TeleportTimer : Timer
		{
			private Mobile m_Owner;

			private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				0, -1,
				0,  1,
				1, -1,
				1,  0,
				1,  1
			};

			public TeleportTimer( Mobile owner )
				: base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				Map map = m_Owner.Map;

				if ( map == null )
					return;

				if ( 0.25 < Utility.RandomDouble() )
					return;

				Mobile toTeleport = null;

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 16 ) )
				{
					if ( m != m_Owner && m.Player && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) )
					{
						toTeleport = m;
						break;
					}
				}

				if ( toTeleport != null )
				{
					int offset = Utility.Random( 8 ) * 2;

					Point3D to = m_Owner.Location;

					for ( int i = 0; i < m_Offsets.Length; i += 2 )
					{
						int x = m_Owner.X + m_Offsets[( offset + i ) % m_Offsets.Length];
						int y = m_Owner.Y + m_Offsets[( offset + i + 1 ) % m_Offsets.Length];

						if ( map.CanSpawnMobile( x, y, m_Owner.Z ) )
						{
							to = new Point3D( x, y, m_Owner.Z );
							break;
						}
						else
						{
							int z = map.GetAverageZ( x, y );

							if ( map.CanSpawnMobile( x, y, z ) )
							{
								to = new Point3D( x, y, z );
								break;
							}
						}
					}

					Mobile m = toTeleport;

					Point3D from = m.Location;

					m.Location = to;

					Server.Spells.SpellHelper.Turn( m_Owner, toTeleport );
					Server.Spells.SpellHelper.Turn( toTeleport, m_Owner );

					m.ProcessDelta();

					Effects.SendLocationParticles( EffectItem.Create( from, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					Effects.SendLocationParticles( EffectItem.Create( to, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

					m.PlaySound( 0x1FE );

					int message = Utility.RandomList( 1049503, 1049506 );

					m_Owner.Say( message );

					m_Owner.Combatant = toTeleport;
				}
			}
		}

		private class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;

			public GoodiesTimer( Map map, int x, int y )
				: base( TimeSpan.FromSeconds( Utility.RandomDouble() * 10.0 ) )
			{
				m_Map = map;
				m_X = x;
				m_Y = y;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ( m_X, m_Y );
				bool canFit = m_Map.CanFit( m_X, m_Y, z, 6, false, false );

				for ( int i = -3; !canFit && i <= 3; ++i )
				{
					canFit = m_Map.CanFit( m_X, m_Y, z + i, 6, false, false );

					if ( canFit )
						z += i;
				}

				if ( !canFit )
					return;

				Gold g = new Gold( 750, 1250 );

				g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

				if ( 0.5 >= Utility.RandomDouble() )
				{
					switch ( Utility.Random( 3 ) )
					{
						case 0: // Fire column
							{
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
								Effects.PlaySound( g, g.Map, 0x208 );

								break;
							}
						case 1: // Explosion
							{
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
								Effects.PlaySound( g, g.Map, 0x307 );

								break;
							}
						case 2: // Ball of fire
							{
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36FE, 10, 10, 5052 );

								break;
							}
					}
				}
			}
		}
	}
}
