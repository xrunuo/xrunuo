using System;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	public class LordOaks : BaseChampion
	{
		public override Type[] UniqueList { get { return new Type[] { typeof( OrcChieftainHelm ) }; } }
		public override Type[] SharedList
		{
			get
			{
				return new Type[] {
					typeof( RoyalGuardSurvivalKnife ),
					typeof( DjinnisRing ),
					typeof( LieutenantOfTheBritannianRoyalGuard ),
					typeof( SamaritanRobe ),
					typeof( DetectiveBoots ),
					typeof( TheMostKnowledgePerson )
				};
			}
		}
		public override Type[] DecorativeList
		{
			get
			{
				return new Type[] {
					typeof( WaterTile ),
					typeof( WindSpirit ),
					typeof( Pier )
				};
			}
		}

		public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { }; } }

		private Mobile m_Queen;
		private bool m_SpawnedQueen;

		public override ChampionSkullType SkullType { get { return ChampionSkullType.Enlightenment; } }

		[Constructable]
		public LordOaks()
			: base( AIType.AI_Mage, FightMode.Evil )
		{
			Body = 175;
			Name = "Lord Oaks";

			SetStr( 403, 850 );
			SetDex( 101, 150 );
			SetInt( 503, 800 );

			SetStam( 202, 400 );

			SetDamage( 21, 33 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetHits( 12000 );

			SetResistance( ResistanceType.Physical, 85, 90 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 80, 90 );
			SetResistance( ResistanceType.Energy, 80, 90 );

			SetSkill( SkillName.Anatomy, 75.1, 100.0 );
			SetSkill( SkillName.EvalInt, 120.1, 130.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 120.1, 130.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 115.0, 125.0 );
			SetSkill( SkillName.Wrestling, 115.0, 130.0 );

			Fame = 22500;
			Karma = 22500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 3 );
		}

		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override int TreasureMapLevel { get { return 5; } }

		public void SpawnPixies( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;

			this.Say( 1042154 ); // You shall never defeat me as long as I have my queen!

			int newPixies = Utility.RandomMinMax( 3, 6 );

			for ( int i = 0; i < newPixies; ++i )
			{
				Pixie pixie = new Pixie();

				pixie.Team = this.Team;
				pixie.FightMode = FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				pixie.MoveToWorld( loc, map );
				pixie.Combatant = target;
			}
		}

		public override int GetAngerSound()
		{
			return 0x2F8;
		}

		public override int GetIdleSound()
		{
			return 0x2F8;
		}

		public override int GetAttackSound()
		{
			return Utility.Random( 0x2F5, 2 );
		}

		public override int GetHurtSound()
		{
			return 0x2F9;
		}

		public override int GetDeathSound()
		{
			return 0x2F7;
		}

		public void CheckQueen()
		{
			if ( !m_SpawnedQueen )
			{
				this.Say( 1042153 ); // Come forth my queen!

				m_Queen = new Silvani();

				( (BaseCreature) m_Queen ).Team = this.Team;

				m_Queen.MoveToWorld( this.Location, this.Map );

				m_SpawnedQueen = true;
			}
			else if ( m_Queen != null && m_Queen.Deleted )
			{
				m_Queen = null;
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			CheckQueen();

			if ( m_Queen != null )
			{
				scalar *= 0.1;

				if ( 0.1 >= Utility.RandomDouble() )
					SpawnPixies( caster );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Damage( Utility.Random( 20, 10 ), this );
			defender.Stam -= Utility.Random( 20, 10 );
			defender.Mana -= Utility.Random( 20, 10 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			CheckQueen();

			if ( m_Queen != null && 0.1 >= Utility.RandomDouble() )
				SpawnPixies( attacker );

			attacker.Damage( Utility.Random( 20, 10 ), this );
			attacker.Stam -= Utility.Random( 20, 10 );
			attacker.Mana -= Utility.Random( 20, 10 );
		}

		public LordOaks( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Queen );
			writer.Write( m_SpawnedQueen );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Queen = reader.ReadMobile();
						m_SpawnedQueen = reader.ReadBool();

						break;
					}
			}
		}
	}
}