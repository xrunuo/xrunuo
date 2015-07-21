using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Chief Paroxysmus corpse" )]
	public class ChiefParoxysmus : BaseCreature
	{
		private Timer m_Timer;
		private ArrayList m_Summons = new ArrayList();

		public override int GetDeathSound() { return 0x56F; }
		public override int GetAttackSound() { return 0x570; }
		public override int GetIdleSound() { return 0x571; }
		public override int GetAngerSound() { return 0x572; }
		public override int GetHurtSound() { return 0x573; }

		[Constructable]
		public ChiefParoxysmus()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Chief Paroxysmus";
			Body = 0x100;

			SetStr( 1200, 1400 );
			SetDex( 75, 85 );
			SetInt( 75, 85 );

			SetHits( 50000 );

			SetDamage( 28, 36 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 75, 85 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );
			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.Poisoning, 120.0 );

			Fame = 32000;
			Karma = -32000;

			m_Timer = new TeleportTimer( this );
			m_Timer.Start();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
			AddLoot( LootPack.PeerlessIngredients, 8 );
			AddLoot( LootPack.Talismans, Utility.RandomMinMax( 1, 5 ) );
		}

		public override int Meat { get { return 1; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override bool BardImmune { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override void OnBeforeSpawn( Point3D location, Map map )
		{
			for ( int i = 0; i < 4; i++ )
			{
				BaseCreature bulbous = new BulbousPutrification();

				bulbous.Team = this.Team;
				bulbous.MoveToWorld( location, map );

				m_Summons.Add( bulbous );
			}

			base.OnBeforeSpawn( location, map );
		}

		public override void OnAfterDelete()
		{
			for ( int n = 0; n < m_Summons.Count; n++ )
			{
				Mobile m = (Mobile) m_Summons[n];

				if ( m != null && !m.Deleted )
					m.Kill();
			}

			base.OnAfterDelete();
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new CrimsonCincture() );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new ScepterOfTheChief() );

			if ( 5000 > Utility.Random( 100000 ) )
				c.DropItem( new ParoxysmusSwampDragonStatuette() );

			if ( 10000 > Utility.Random( 100000 ) )
				c.DropItem( new HumanFeyLeggings() );

			c.DropItem( new SweatOfParoxysmus() );

			switch ( Utility.RandomMinMax( 1, 3 ) )
			{
				case 1:
					c.DropItem( new ParoxysmusDinner() );
					break;
				case 2:
					c.DropItem( new StringOfPartsOfParoxysmusVictims() );
					break;
				case 3:
					c.DropItem( new ParoxysmusCorrodedStein() );
					break;
			}

			for ( int i = 0; i < 3; i++ )
			{
				if ( Utility.RandomBool() )
					c.DropItem( new LardOfParoxysmus() );
			}

			for ( int i = 0; i < 3; i++ )
			{
				if ( 5000 > Utility.Random( 100000 ) )
					c.DropItem( SetItemsHelper.GetRandomSetItem() );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			Poison p = HitPoison;

			if ( p != null && HitPoisonChance >= Utility.RandomDouble() )
			{
				defender.ApplyPoison( this, p );

				foreach ( Mobile m in this.GetMobilesInRange( 5 ) )
				{
					if ( m.IsPlayer )
					{
						PlayerMobile pm = m as PlayerMobile;
						if ( pm.HonorActive )
							continue;
					}
					m.ApplyPoison( this, p );
				}
			}

			PlaySound( 0x574 );

			if ( defender is BaseCreature )
			{
				BaseCreature bc = defender as BaseCreature;

				if ( bc.SummonMaster != null )
					bc.SummonMaster.SendLocalizedMessage( 1072079 ); // Your pet has succumbed to the hunger of your enemy!
				else if ( bc.ControlMaster != null )
					bc.ControlMaster.SendLocalizedMessage( 1072079 ); // Your pet has succumbed to the hunger of your enemy!

				bc.Kill();

				Hits = HitsMax;
			}
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
					if ( m != m_Owner && m.IsPlayer && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) )
					{
						PlayerMobile pm = m as PlayerMobile;
						if ( !pm.HonorActive )
						{
							toTeleport = m;
							break;
						}
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

		public ChiefParoxysmus( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			m_Timer = new TeleportTimer( this );
			m_Timer.Start();
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}