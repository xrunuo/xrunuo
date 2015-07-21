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
	public class PrimevalLich : BaseChampion
	{
		public override bool DropSkull { get { return false; } }

		public override Type[] UniqueList
		{
			get
			{
				return new Type[]
				{
					typeof( LightsRampart ),					
					typeof( ProtectorOfTheBattleMage ),
					typeof( CastOffZombieSkin ),
					typeof( BansheesCall )
				};
			}
		}

		public override Type[] SharedList
		{
			get
			{
				return new Type[]
				{
					typeof( TheMostKnowledgePerson ),
					typeof( ChannelersDefender ),
					typeof( LieutenantOfTheBritannianRoyalGuard )
				};
			}
		}

		public override Type[] DecorativeList
		{
			get
			{
				return new Type[]
				{
					typeof( MummifiedCorpse )
				};
			}
		}

		public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { }; } }

		private DateTime m_NextAbilityTime;
		private Timer m_Timer;

		[Constructable]
		public PrimevalLich()
			: base( AIType.AI_Necromancer )
		{
			Body = 830;
			Name = "Primeval Lich";

			SetStr( 500 );
			SetDex( 100 );
			SetInt( 1000 );

			SetHits( 30000 );
			SetMana( 5000 );

			SetDamage( 17, 21 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 30 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 120.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Necromancy, 100.0 );
			SetSkill( SkillName.SpiritSpeak, 100.0 );

			Fame = 28000;
			Karma = -28000;

			m_Timer = new TeleportTimer( this );
			m_Timer.Start();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			c.DropItem( new PrimevalLichDust() );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 150; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		#region Blast Radius
		private static readonly int BlastRange = 16;

		private static readonly double[] BlastChance = new double[]
			{
				0.0, 0.0, 0.05,	0.95, 0.95, 0.95, 0.05, 0.95, 0.95,
				0.95, 0.05, 0.95, 0.95, 0.95, 0.05, 0.95, 0.95
			};

		private void BlastRadius()
		{
			// TODO: Based on OSI taken videos, not accurate, but an aproximation

			Point3D loc = this.Location;

			for ( int x = -BlastRange; x <= BlastRange; x++ )
			{
				for ( int y = -BlastRange; y <= BlastRange; y++ )
				{
					Point3D p = new Point3D( loc.X + x, loc.Y + y, loc.Z );
					int dist = (int) Math.Round( Utility.GetDistanceToSqrt( loc, p ) );

					if ( dist <= BlastRange && BlastChance[dist] > Utility.RandomDouble() )
					{
						Timer.DelayCall( TimeSpan.FromSeconds( 0.1 * dist ), new TimerCallback(
							delegate
							{
								int hue = Utility.RandomList( 90, 95 );

								Effects.SendPacket( loc, this.Map, new HuedEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x3709, p, p, 20, 30, true, false, hue, 4 ) );
							}
						) );
					}
				}
			}

			PlaySound( 0x64C );

			foreach ( Mobile m in this.GetMobilesInRange( BlastRange ).ToArray() )
			{
				if ( this != m && this.GetDistanceToSqrt( m ) <= BlastRange && CanBeHarmful( m ) )
				{
					if ( m is BaseCreature && !( (BaseCreature) m ).Controlled && !( (BaseCreature) m ).Summoned )
						continue;

					DoHarmful( m );

					AOS.Damage( m, this, Utility.RandomMinMax( 100, 120 ), 0, 0, 100, 0, 0 );
				}
			}
		}
		#endregion

		#region Lightning
		private void Lightning()
		{
			int count = 0;

			foreach ( Mobile m in this.GetMobilesInRange( BlastRange ).ToArray() )
			{
				if ( m.IsPlayer && this.GetDistanceToSqrt( m ) <= BlastRange && CanBeHarmful( m ) )
				{
					DoHarmful( m );

					Effects.SendBoltEffect( m, false, 0 );
					Effects.PlaySound( m, m.Map, 0x51D );

					AOS.Damage( m, this, Utility.RandomMinMax( 100, 120 ), 0, 0, 0, 0, 100 );

					count++;

					if ( count >= 6 )
						break;
				}
			}
		}
		#endregion

		#region Teleport
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
					if ( m != m_Owner && m.IsPlayer && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) )
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

					m_Owner.Combatant = toTeleport;
				}
			}
		}
		#endregion

		#region Unholy Touch
		private static Dictionary<Mobile, Timer> m_UnholyTouched = new Dictionary<Mobile, Timer>();

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 > Utility.RandomDouble() && !m_UnholyTouched.ContainsKey( defender ) )
			{
				double resist = defender.Skills[SkillName.MagicResist].Value;

				double scalar = -( 0.4 - ( resist / 360.0 ) );
				double seconds = 130.0 - resist;

				ArrayList mods = new ArrayList();

				mods.Add( new ResistanceMod( ResistanceType.Physical, (int) ( defender.PhysicalResistance * scalar ) ) );
				mods.Add( new ResistanceMod( ResistanceType.Fire, (int) ( defender.FireResistance * scalar ) ) );
				mods.Add( new ResistanceMod( ResistanceType.Cold, (int) ( defender.ColdResistance * scalar ) ) );
				mods.Add( new ResistanceMod( ResistanceType.Poison, (int) ( defender.PoisonResistance * scalar ) ) );
				mods.Add( new ResistanceMod( ResistanceType.Energy, (int) ( defender.EnergyResistance * scalar ) ) );

				for ( int i = 0; i < defender.Skills.Length; ++i )
				{
					if ( defender.Skills[i].Value > 0 )
						mods.Add( new DefaultSkillMod( (SkillName) i, true, defender.Skills[i].Value * scalar ) );
				}

				ApplyMods( defender, mods );

				m_UnholyTouched[defender] = Timer.DelayCall( TimeSpan.FromSeconds( seconds ), new TimerCallback(
					delegate
					{
						ClearMods( defender, mods );

						m_UnholyTouched.Remove( defender );
					} ) );
			}
		}

		private static void ApplyMods( Mobile from, ArrayList mods )
		{
			for ( int i = 0; i < mods.Count; ++i )
			{
				object mod = mods[i];

				if ( mod is ResistanceMod )
					from.AddResistanceMod( (ResistanceMod) mod );
				else if ( mod is StatMod )
					from.AddStatMod( (StatMod) mod );
				else if ( mod is SkillMod )
					from.AddSkillMod( (SkillMod) mod );
			}
		}

		private static void ClearMods( Mobile from, ArrayList mods )
		{
			for ( int i = 0; i < mods.Count; ++i )
			{
				object mod = mods[i];

				if ( mod is ResistanceMod )
					from.RemoveResistanceMod( (ResistanceMod) mod );
				else if ( mod is StatMod )
					from.RemoveStatMod( ( (StatMod) mod ).Name );
				else if ( mod is SkillMod )
					from.RemoveSkillMod( (SkillMod) mod );
			}
		}
		#endregion

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( willKill || Map == null )
				return;

			if ( DateTime.Now > m_NextAbilityTime && 0.2 > Utility.RandomDouble() )
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0: BlastRadius(); break;
					case 1: Lightning(); break;
				}

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 25, 35 ) );
			}
		}

		public override int GetAttackSound() { return 0x61E; }
		public override int GetDeathSound() { return 0x61F; }
		public override int GetHurtSound() { return 0x620; }
		public override int GetIdleSound() { return 0x621; }

		public PrimevalLich( Serial serial )
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

			m_Timer = new TeleportTimer( this );
			m_Timer.Start();
		}
	}
}
