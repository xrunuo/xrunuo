using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	public abstract class BaseChampion : BaseCreature
	{
		public BaseChampion( AIType aiType )
			: this( aiType, FightMode.Closest )
		{
		}

		public BaseChampion( AIType aiType, FightMode mode )
			: base( aiType, mode, 18, 1, 0.1, 0.2 )
		{
		}

		public BaseChampion( Serial serial )
			: base( serial )
		{
		}

		public virtual ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }
		public virtual bool DropSkull { get { return true; } }

		public abstract Type[] UniqueList { get; }
		public abstract Type[] SharedList { get; }
		public abstract Type[] DecorativeList { get; }
		public abstract MonsterStatuetteType[] StatueTypes { get; }

		public virtual bool NoGoodies { get { return false; } }

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

			Item artifact = Loot.Construct( type );

			if ( artifact is MonsterStatuette && StatueTypes.Length > 0 )
			{
				( (MonsterStatuette) artifact ).Type = StatueTypes[Utility.Random( StatueTypes.Length )];
				( (MonsterStatuette) artifact ).LootType = LootType.Regular;
			}

			return artifact;
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

			if ( Map != Map.Felucca )
				return;

			for ( int i = 0; i < toGive.Count; i++ )
			{
				Mobile m = (Mobile) toGive[i];

				if ( !( m.Map == this.Map && m.InRange( this, 90 ) ) || ( !m.Alive && m.Corpse == null ) )
					toGive.Remove( m );
			}

			if ( toGive.Count == 0 )
				return;

			for ( int i = 0; i < 6; ++i )
			{
				int level;
				double random = Utility.RandomDouble();

				/* Powerscroll type chance:
				 * - 80% chance to get a 110 powerscroll
				 * - 15% chance to get a 115 powerscroll
				 * - 5% chance to get a 120 powerscroll
				 */

				if ( 0.05 >= random )
					level = 20;
				else if ( 0.2 >= random )
					level = 15;
				else
					level = 10;

				Mobile m = (Mobile) toGive[i % toGive.Count];

				PowerScroll ps = PowerScroll.CreateRandomNoCraft( level, level );

				m.SendLocalizedMessage( 1049524 ); // You have received a scroll of power!

				if ( m.Alive )
				{
					m.AddToBackpack( ps );
				}
				else
				{
					Container corpse = m.Corpse;

					if ( corpse != null )
						corpse.DropItem( ps );
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
							case VirtueLevel.Seeker:
								chance = 60;
								break;
							case VirtueLevel.Follower:
								chance = 80;
								break;
							case VirtueLevel.Knight:
								chance = 100;
								break;
						}

						if ( chance > Utility.Random( 100 ) )
						{
							PowerScroll pps = PowerScroll.CreateRandomNoCraft( level, level );
							prot.SendLocalizedMessage( 1049368 ); // You have been rewarded for your dedication to Justice!
							prot.AddToBackpack( pps );
						}
					}
				}
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

				int pointsToGain = 800;

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

		public void GiveTitles( ArrayList toGive )
		{
			// TODO: verify scores for killing champions

			for ( int i = 0; i < toGive.Count; ++i )
			{
				PlayerMobile pm = (PlayerMobile) toGive[i % toGive.Count];

				switch ( SkullType )
				{
					case ChampionSkullType.Power:
						pm.ChampionTiers[0] += 2;
						pm.SuperChampionTiers[1]++;
						break;
					case ChampionSkullType.Enlightenment:
						pm.ChampionTiers[1] += 2;
						pm.SuperChampionTiers[2]++;
						break;
					case ChampionSkullType.Venom:
						pm.ChampionTiers[2] += 2;
						pm.SuperChampionTiers[3]++;
						break;
					case ChampionSkullType.Pain:
						pm.ChampionTiers[3] += 2;
						pm.SuperChampionTiers[4]++;
						break;
					case ChampionSkullType.Greed:
						pm.ChampionTiers[4] += 2;
						pm.SuperChampionTiers[5]++;
						break;
					case ChampionSkullType.Death:
						pm.ChampionTiers[5] += 2;
						pm.SuperChampionTiers[6]++;
						break;
				}

				if ( this is Serado )
				{
					pm.ChampionTiers[6] += 2;
					pm.SuperChampionTiers[7]++;
				}

				if ( this is IlhenirTheStained )
				{
					pm.ChampionTiers[7] += 2;
					pm.SuperChampionTiers[8]++;
				}

				if ( this is TwauloOfTheGlade )
				{
					pm.ChampionTiers[8] += 2;
					pm.SuperChampionTiers[9]++;
				}

				if ( this is PrimevalLich )
				{
					pm.ChampionTiers[9] += 2;
					pm.SuperChampionTiers[10]++;
				}

				if ( this is AbyssalInfernal )
				{
					pm.ChampionTiers[10] += 2;
					pm.SuperChampionTiers[11]++;
				}
			}
		}

		protected override bool OnBeforeDeath()
		{
			GiveRewards();

			if ( !NoKillAwards )
			{
				if ( NoGoodies )
					return base.OnBeforeDeath();

				Map map = this.Map;

				if ( map != null )
				{
					for ( int x = -12; x <= 12; ++x )
					{
						for ( int y = -12; y <= 12; ++y )
						{
							double dist = Math.Sqrt( x * x + y * y );

							if ( dist <= 12 )
								new GoodiesTimer( map, X + x, Y + y ).Start();
						}
					}
				}
			}

			return base.OnBeforeDeath();
		}

		protected override void OnAfterDeath( Container c )
		{
			if ( Map == Map.Felucca && DropSkull )
			{
				// Get the first mobile in the top attackers list that is in range.
				Mobile to = BaseCreature.GetLootingRights( this.DamageEntries, this.HitsMax )
					.OrderByDescending( e => e.DamagePercent )
					.Select( e => e.Mobile )
					.FirstOrDefault( m => m.InRange( c, 90 ) );

				Container target = null;

				if ( to != null )
					target = to.Backpack;
				else
					target = c; // If no candidates, the skull goes to the champion's corpse.

				if ( target != null ) // Sanity.
					target.DropItem( new ChampionSkull( SkullType ) );
			}

			base.OnAfterDeath( c );
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

				Gold g = new Gold( 500, 1000 );

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
