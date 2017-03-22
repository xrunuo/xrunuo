using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Items;
using Server.Spells;
using Server.Engines.CannedEvil;
using Server.Engines.Loyalty;
using Server.Network;

namespace Server.Mobiles
{
	public class AbyssalInfernal : BaseChampion
	{
		public override bool DropSkull { get { return false; } }

		public override Type[] UniqueList
		{
			get
			{
				return new Type[]
				{
					typeof( TongueOfTheBeast ),
					typeof( DeathsHead ),
					typeof( AbyssalBlade ),
					typeof( WallOfHungryMouths )
				};
			}
		}

		public override Type[] SharedList
		{
			get
			{
				return new Type[]
				{
					typeof( RoyalGuardInvestigator ),
					typeof( JadeArmband )
				};
			}
		}

		public override Type[] DecorativeList
		{
			get
			{
				return new Type[]
				{
					typeof( MagicalDoor )
				};
			}
		}

		// TODO (SA): "Archdemon Statue"
		public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { }; } }

		private DateTime m_NextAbilityTime;

		[Constructable]
		public AbyssalInfernal()
			: base( AIType.AI_Mage )
		{
			Body = 713;
			Name = "Abyssal Infernal";

			SetStr( 1165, 1262 );
			SetDex( 104, 143 );
			SetInt( 572, 675 );

			SetHits( 30000 );

			SetDamage( 11, 18 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 70, 90 );
			SetResistance( ResistanceType.Cold, 55, 65 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			SetSkill( SkillName.Anatomy, 110.1, 119.3 );
			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 111.0, 117.6 );
			SetSkill( SkillName.Wrestling, 111.0, 120.0 );
			SetSkill( SkillName.Magery, 109.0, 129.6 );
			SetSkill( SkillName.EvalInt, 113.6, 135.2 );
			SetSkill( SkillName.Meditation, 100.0 );

			Fame = 28000;
			Karma = -28000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 5 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			c.DropItem( new AbyssalHorn() );
		}

		private static Type[] m_SummonTypes = new Type[]
			{
				typeof( Efreet ),		typeof( FireGargoyle ),
				typeof( FireSteed ),	typeof( Gargoyle ),
				typeof( HellHound ),	typeof( HellCat ),
				typeof( Imp ),			typeof( LavaElemental ),
				typeof( Nightmare ),	typeof( Phoenix )
			};

		private static Point2D[] m_ColumnOffset = new Point2D[]
			{
				new Point2D(  0, -1 ),
				new Point2D( -1,  0 ),
				new Point2D(  1,  0 ),
				new Point2D(  0,  1 )
			};

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( willKill || Map == null || from == null )
				return;

			if ( 0.05 > Utility.RandomDouble() )
			{
				Point3D loc = Map.GetSpawnPosition( Location, 8 );
				Type type = m_SummonTypes[Utility.Random( m_SummonTypes.Length )];

				for ( int i = 0; i < 4; i++ )
				{
					BaseCreature summon = (BaseCreature) Activator.CreateInstance( type );

					if ( summon != null )
					{
						summon.SetHits( summon.HitsMax / 2 );

						summon.OnBeforeSpawn( loc, Map );
						summon.MoveToWorld( loc, Map );

						summon.Combatant = from;
					}
				}
			}

			if ( 0.1 > Utility.RandomDouble() && DateTime.UtcNow > m_NextAbilityTime )
			{
				Say( 1112362 ); // You will burn to a pile of ash!

				from.PlaySound( 0x349 );

				for ( int i = 0; i < m_ColumnOffset.Length; i++ )
				{
					Point2D offset = m_ColumnOffset[i];

					Point3D effectLocation = new Point3D( Location.X + offset.X, Location.Y + offset.Y, Location.Z );

					Effects.SendPacket( effectLocation, Map,
						new LocationEffect( effectLocation, 0x3709, 10, 30, 0, 0 ) );
				}

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback(
					delegate
					{
						// TODO: Based on OSI taken videos, not accurate, but an aproximation

						Effects.SendPacket( this.Location, this.Map, new FlashEffect( FlashType.LightFlash ) );

						PlaySound( 0x44B );

						int range = 8;

						Point3D loc = this.Location;
						Map map = this.Map;

						for ( int x = -range; x <= range; x++ )
						{
							for ( int y = -range; y <= range; y++ )
							{
								Point3D p = new Point3D( loc.X + x, loc.Y + y, loc.Z - 5 );
								int dist = (int) Math.Round( Utility.GetDistanceToSqrt( loc, p ) );

								if ( dist <= range )
								{
									int itemId, renderMode, duration;

									if ( dist >= 8 )
									{
										itemId = 0x36CB;
										duration = 9;
										renderMode = 4;
									}
									else if ( dist >= 6 )
									{
										itemId = 0x3728;
										duration = 13;
										renderMode = 4;
									}
									else
									{
										itemId = 0x3728;
										duration = 13;
										renderMode = 3;
									}

									Effects.SendPacket( loc, map, new HuedEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, itemId, p, p, 1, duration, true, false, 1150, renderMode ) );
								}
							}
						}

						foreach ( Mobile m in this.GetMobilesInRange( 8 ).ToArray() )
						{
							if ( this != m && this.GetDistanceToSqrt( m ) <= 8 && CanBeHarmful( m ) )
							{
								if ( m is BaseCreature && !( (BaseCreature) m ).Controlled && !( (BaseCreature) m ).Summoned )
									continue;

								DoHarmful( m );

								// TODO: Where do we freeze target and teleport them away?

								AOS.Damage( m, this, Utility.RandomMinMax( 90, 110 ), 0, 100, 0, 0, 0 );
							}
						}
					} ) );

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 35, 45 ) );
			}
		}

		public override int GetAttackSound() { return 0x5D4; }
		public override int GetDeathSound() { return 0x5D5; }
		public override int GetHurtSound() { return 0x5D6; }
		public override int GetIdleSound() { return 0x5D7; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 150; } }

		public AbyssalInfernal( Serial serial )
			: base( serial )
		{
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
}
