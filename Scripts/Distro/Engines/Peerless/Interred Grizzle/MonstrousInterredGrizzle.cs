using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Monstrous Interred Grizzle corpse" )]
	public class MonstrousInterredGrizzle : BaseCreature
	{
		[Constructable]
		public MonstrousInterredGrizzle()
			: base( AIType.AI_Arcanist, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Monstrous Interred Grizzle";
			Body = 259;
			BaseSoundID = 589;

			SetStr( 1198, 1207 );
			SetDex( 127, 135 );
			SetInt( 595, 646 );

			SetHits( 50000 );

			SetDamage( 27, 32 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 48, 52 );
			SetResistance( ResistanceType.Fire, 77, 82 );
			SetResistance( ResistanceType.Cold, 56, 61 );
			SetResistance( ResistanceType.Poison, 32, 40 );
			SetResistance( ResistanceType.Energy, 69, 71 );

			SetSkill( SkillName.Wrestling, 112.6, 116.9 );
			SetSkill( SkillName.Tactics, 118.5, 119.2 );
			SetSkill( SkillName.MagicResist, 120 );
			SetSkill( SkillName.Anatomy, 111.0, 111.7 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.EvalInt, 100 );
			SetSkill( SkillName.Meditation, 100 );

			Fame = 32000;
			Karma = -32000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
			AddLoot( LootPack.PeerlessIngredients, 8 );
			AddLoot( LootPack.Talismans, Utility.RandomMinMax( 1, 5 ) );
		}

		public override int TreasureMapLevel { get { return 5; } }

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new CrimsonCincture() );

			if ( 15000 > Utility.Random( 100000 ) )
				c.DropItem( new GlobOfMonstrousInterredGrizzle() );

			if ( 15000 > Utility.Random( 100000 ) )
				c.DropItem( new GrizzledSkullCollection() );

			if ( 15000 > Utility.Random( 100000 ) )
				c.DropItem( new MonstrousInterredGrizzleMaggots() );

			if ( 15000 > Utility.Random( 100000 ) )
				c.DropItem( new TombstoneOfTheDamned() );

			if ( 5000 > Utility.Random( 100000 ) )
				c.DropItem( new GrizzledMareStatuette() );

			if ( 10000 > Utility.Random( 100000 ) )
				c.DropItem( new HumanFeyLeggings() );

			for ( int i = 0; i < 3; i++ )
			{
				if ( Utility.RandomBool() )
					c.DropItem( new GrizzledBones( Utility.RandomMinMax( 1, 3 ) ) );
			}

			if ( 5000 > Utility.Random( 100000 ) )
			{
				switch ( Utility.RandomMinMax( 1, 5 ) )
				{
					case 1:
						c.DropItem( new GauntletsOfTheGrizzle() );
						break;
					case 2:
						c.DropItem( new GreavesOfTheGrizzle() );
						break;
					case 3:
						c.DropItem( new VambracesOfTheGrizzle() );
						break;
					case 4:
						c.DropItem( new TunicOfTheGrizzle() );
						break;
					case 5:
						c.DropItem( new SkullHelmOfTheGrizzle() );
						break;
				}
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( Utility.RandomDouble() < 0.15 )
				CacophonicAttack( defender );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( Utility.RandomDouble() < 0.15 )
				CacophonicAttack( from );

			if ( Utility.RandomDouble() < 0.3 )
				DropOoze();

			base.OnDamage( amount, from, willKill );
		}

		private static Hashtable m_Table = new Hashtable();

		public virtual void CacophonicAttack( Mobile to )
		{
			if ( to == null )
				return;

			if ( to.Alive && to.Player && m_Table[to] == null )
			{
				to.ForcedWalk = true;
				to.SendLocalizedMessage( 1072069 ); // A cacophonic sound lambastes you, suppressing your ability to move.
				to.PlaySound( 0x584 );

				m_Table[to] = Timer.DelayCall( TimeSpan.FromSeconds( 30 ), new TimerStateCallback( EndCacophonic_Callback ), to );
			}
		}

		private void EndCacophonic_Callback( object state )
		{
			if ( state is Mobile )
				CacophonicEnd( (Mobile) state );
		}

		public virtual void CacophonicEnd( Mobile from )
		{
			if ( m_Table == null )
				m_Table = new Hashtable();

			m_Table[from] = null;

			from.ForcedWalk = false;
		}

		public static bool UnderCacophonicAttack( Mobile from )
		{
			if ( m_Table == null )
				m_Table = new Hashtable();

			return m_Table[from] != null;
		}

		private DateTime m_NextDrop = DateTime.UtcNow;

		public virtual void DropOoze()
		{
			if ( Map == null )
				return;

			int amount = Utility.RandomMinMax( 1, 3 );
			bool corrosive = Utility.RandomBool();

			for ( int i = 0; i < amount; i++ )
			{
				Item ooze = new InfernalOoze( corrosive );
				Point3D p = new Point3D( Location );

				for ( int j = 0; j < 5; j++ )
				{
					p = GetSpawnPosition( 2 );

					foreach ( Item item in Map.GetItemsInRange( p, 0 ) )
						if ( item is InfernalOoze )
							continue;
				}

				ooze.MoveToWorld( p, Map );
			}

			if ( Combatant != null )
			{
				if ( corrosive )
					Combatant.SendLocalizedMessage( 1072071 ); // A corrosive gas seeps out of your enemy's skin!
				else
					Combatant.SendLocalizedMessage( 1072072 ); // A poisonous gas seeps out of your enemy's skin!
			}
		}

		public virtual Point3D GetSpawnPosition( int range )
		{
			return GetSpawnPosition( Location, Map, range );
		}

		public static Point3D GetSpawnPosition( Point3D from, Map map, int range )
		{
			if ( map == null )
				return from;

			for ( int i = 0; i < 10; i++ )
			{
				int x = from.X + Utility.Random( range );
				int y = from.Y + Utility.Random( range );
				int z = map.GetAverageZ( x, y );

				if ( Utility.RandomBool() )
					x *= -1;

				if ( Utility.RandomBool() )
					y *= -1;

				Point3D p = new Point3D( x, y, from.Z );

				if ( map.CanSpawnMobile( p ) && map.LineOfSight( from, p ) )
					return p;

				p = new Point3D( x, y, z );

				if ( map.CanSpawnMobile( p ) && map.LineOfSight( from, p ) )
					return p;
			}

			return from;
		}

		public MonstrousInterredGrizzle( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class InfernalOoze : Item
	{
		private bool m_Corrosive;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Corrosive
		{
			get { return m_Corrosive; }
			set { m_Corrosive = value; }
		}

		[Constructable]
		public InfernalOoze()
			: this( false )
		{
		}

		[Constructable]
		public InfernalOoze( bool corrosive )
			: base( 0x122A )
		{
			Movable = false;
			Hue = 0x95;

			m_Corrosive = corrosive;
			Timer.DelayCall( TimeSpan.FromSeconds( 30 ), new TimerCallback( Morph ) );
		}

		private Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();

		public override bool OnMoveOver( Mobile m )
		{
			if ( !base.OnMoveOver( m ) )
				return false;

			if ( ( m.Player && m.Alive ) || ( m is BaseCreature && !m.IsDeadBondedPet && ( (BaseCreature) m ).Controlled ) )
				m_Table[m] = Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Damage_Callback ), m );

			return true;
		}

		public override bool OnMoveOff( Mobile m )
		{
			if ( !base.OnMoveOff( m ) )
				return false;

			StopTimer( m );

			return true;
		}

		public InfernalOoze( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) m_Corrosive );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Corrosive = reader.ReadBool();
		}

		private void Damage_Callback( object state )
		{
			if ( state is Mobile )
				Damage( (Mobile) state );
		}

		public virtual void Damage( Mobile m )
		{
			if ( !m.Alive || m.IsDeadBondedPet )
				StopTimer( m );

			if ( m_Corrosive )
			{
				// TODO

				/*for ( int i = 0; i < m.Items.Count; i ++ )
				{
					IDurability item = m.Items[ i ] as IDurability;
	
					if ( item != null && Utility.RandomDouble() < 0.25 )
					{						
						if ( item.HitPoints > 10 )
							item.HitPoints -= 10;
						else
							item.HitPoints -= 1;
					}
				}*/
			}
			else
			{
				int toDamage = 40;

				if ( BalmOfProtection.UnderEffect( m ) )
				{
					double scale = Utility.RandomMinMax( 50, 100 ) / 100.0;
					toDamage = (int) ( toDamage * scale );
				}

				AOS.Damage( m, toDamage, 0, 0, 0, 100, 0 );
			}
		}

		public virtual void Morph()
		{
			ItemID += 1;

			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( Delete ) );
		}

		public virtual void StopTimer( Mobile m )
		{
			if ( m_Table != null && m_Table.ContainsKey( m ) )
			{
				m_Table[m].Stop();
				m_Table.Remove( m );
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			foreach ( Timer t in m_Table.Values )
				t.Stop();

			m_Table.Clear();
			m_Table = null;
		}
	}
}