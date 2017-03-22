using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Niporailem corpse" )]
	public class Niporailem : BaseCreature
	{
		private DateTime m_NextAbilityTime;
		private Mobile m_SpectralArmor;

		private const int MinAbilityTime = 4;
		private const int MaxAbilityTime = 8;

		[Constructable]
		public Niporailem()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Niporailem";
			Title = "the Thief";
			Body = 0x2D2;

			SetStr( 1000 );
			SetDex( 1200 );
			SetInt( 1200 );

			SetHits( 10000 );

			SetDamage( 15, 27 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Cold, 30, 45 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 78.3, 87.7 );
			SetSkill( SkillName.Tactics, 60.8, 88.7 );
			SetSkill( SkillName.Wrestling, 59.8, 69.4 );
			SetSkill( SkillName.Necromancy, 91.2, 98.3 );
			SetSkill( SkillName.SpiritSpeak, 97.5, 105.2 );

			Fame = 15000; // guessing here
			Karma = -15000;
		}

		public override int GetAngerSound() { return 0x175; }
		public override int GetIdleSound() { return 0x19D; }
		public override int GetAttackSound() { return 0xE2; }
		public override int GetHurtSound() { return 0x28B; }
		public override int GetDeathSound() { return 0x108; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 75; } }

		public override bool AlwaysMurderer { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.2 > Utility.RandomDouble() && ( m_SpectralArmor == null || m_SpectralArmor.Deleted ) )
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );

				m_SpectralArmor = new SpectralArmour();
				m_SpectralArmor.MoveToWorld( this.Location, this.Map );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.2 > Utility.RandomDouble() )
			{
				if ( defender.GetStatMod( "Niporailem Str Curse" ) == null )
					defender.AddStatMod( new StatMod( StatType.Str, "Niporailem Str Curse", -30, TimeSpan.FromSeconds( 15.0 ) ) );
			}
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( 0.1 > Utility.RandomDouble() && DateTime.UtcNow > m_NextAbilityTime && Combatant != null && this.InRange( Combatant, RangePerception ) ) // as per OSI, no check for LOS
			{
				Mobile to = Combatant;

				switch ( Utility.Random( 2 ) )
				{
					case 0: // Niporailem's Treasure
						{
							Effects.SendPacket( Location, Map, new HuedEffect( EffectType.Moving, this.Serial, to.Serial, 0xEEF, this.Location, to.Location, 10, 0, false, false, 0, 0 ) );
							Effects.PlaySound( to.Location, to.Map, 0x37 );

							int amount = Utility.RandomMinMax( 2, 4 );

							for ( int i = 0; i < amount; i++ )
							{
								Item treasure = new NiporailemsTreasure();

								if ( !to.IsPlayer || !to.PlaceInBackpack( treasure ) )
								{
									treasure.MoveToWorld( to.Location, to.Map );
									treasure.OnDroppedToWorld( this, to.Location );
								}
							}

							BaseMount.Dismount( to );
							to.Damage( Utility.Random( 18, 27 ), this );

							m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( MinAbilityTime, MaxAbilityTime ) );

							break;
						}
					case 1: // Strangle
						{
							if ( !Spells.Necromancy.StrangleSpell.UnderEffect( to ) )
							{
								to.PlaySound( 0x22F );
								to.FixedParticles( 0x36CB, 1, 9, 9911, 67, 5, EffectLayer.Head );
								to.FixedParticles( 0x374A, 1, 17, 9502, 1108, 4, (EffectLayer) 255 );

								Spells.Necromancy.StrangleSpell.StartTimer( this, to );
							}

							break;
						}
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 1 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			c.DropItem( new UndyingFlesh() );

			if ( 1500 > Utility.Random( 100000 ) )
				c.DropItem( new BladeOfBattle() );

			if ( 1500 > Utility.Random( 100000 ) )
				c.DropItem( new DemonBridleRing() );

			if ( 1500 > Utility.Random( 100000 ) )
				c.DropItem( new GiantSteps() );

			if ( 1500 > Utility.Random( 100000 ) )
				c.DropItem( new SwordOfShatteredHopes() );
		}

		public Niporailem( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( (Mobile) m_SpectralArmor );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_SpectralArmor = reader.ReadMobile();
		}
	}
}

namespace Server.Items
{
	public class NiporailemsTreasure : Item
	{
		public override int LabelNumber
		{
			get
			{
				return ItemID == 0xEEF
					? 1112113  // Niporailem's Treasure
					: 1112115; // Treasure Sand
			}
		}

		private InternalTimer m_Timer;
		private bool m_CanSpawn;

		public NiporailemsTreasure()
			: base( 0xEEF )
		{
			Weight = 25.0;

			m_Timer = new InternalTimer( this );
			m_Timer.Start();

			m_CanSpawn = true;
		}

		public void TurnToSand()
		{
			ItemID = 0x11EA + Utility.Random( 1 );
			m_CanSpawn = false;
		}

		public override bool OnDroppedToWorld( Mobile from, Point3D p )
		{
			if ( !base.OnDroppedToWorld( from, p ) )
				return false;

			if ( m_CanSpawn )
			{
				int amount = Utility.Random( 3 ); // 0-2

				for ( int i = 0; i < amount; i++ )
				{
					Mobile summon;

					if ( Utility.RandomBool() )
						summon = new CursedMetallicKnight();
					else
						summon = new CursedMetallicMage();

					summon.MoveToWorld( p, from.Map );
				}
			}

			TurnToSand();

			return true;
		}

		public NiporailemsTreasure( Serial serial )
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

		private class InternalTimer : Timer
		{
			private NiporailemsTreasure m_Owner;

			public InternalTimer( NiporailemsTreasure owner )
				: base( TimeSpan.FromSeconds( 60.0 ) )
			{
				
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.Deleted )
					m_Owner.TurnToSand();
			}
		}
	}

	public class TreasureSand : Item
	{
		public override int LabelNumber { get { return 1112115; } } // Treasure Sand

		public TreasureSand()
			: base( 0x11EA + Utility.Random( 1 ) )
		{
			Weight = 1.0;
		}

		public TreasureSand( Serial serial )
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