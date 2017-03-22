using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ancient liche's corpse" )]
	public class AncientLich : BaseCreature
	{
		private static bool OverrideRules = true;

		private static double m_AbilityChance = 0.40;

		private static int m_ReturnTime = 300;

		private static int m_MinTime = 10;
		private static int m_MaxTime = 20;

		private DateTime m_NextAbilityTime;

		private DateTime m_NextReturnTime;

		private ArrayList m_Minions;

		[Constructable]
		public AncientLich()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ancient lich" );
			Body = 78;
			BaseSoundID = 412;

			SetStr( 216, 305 );
			SetDex( 96, 115 );
			SetInt( 966, 1045 );

			SetHits( 560, 595 );

			SetDamage( 15, 27 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.EvalInt, 120.1, 130.0 );
			SetSkill( SkillName.Magery, 120.1, 130.0 );
			SetSkill( SkillName.Meditation, 100.1, 101.0 );
			SetSkill( SkillName.Poisoning, 100.1, 101.0 );
			SetSkill( SkillName.MagicResist, 175.2, 200.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 75.1, 100.0 );
			SetSkill( SkillName.Necromancy, 120.1, 130.0 );
			SetSkill( SkillName.SpiritSpeak, 120.1, 130.0 );

			Fame = 23000;
			Karma = -23000;

			PackNecroReg( 30, 275 );

			PackItem( new GnarledStaff() );

			m_Minions = new ArrayList();
		}

		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

		public override bool CanPaperdollBeOpenedBy( Mobile from )
		{
			return false;
		}

		private int CountAliveMinions()
		{
			int alive = 0;

			foreach ( Mobile m in m_Minions )
			{
				if ( m.Alive && !m.Deleted )
					alive++;
			}

			return alive;
		}

		private void SpawnMinions()
		{
			if ( CountAliveMinions() != 0 )
				return;

			m_Minions.Clear();

			Map map = this.Map;

			if ( map == null )
				return;

			int type = Utility.Random( 2 );

			BaseMobileHelper.ShowMorphEffect( this );

			switch ( type )
			{
				default:
				case 0:
					BodyMod = Utility.RandomList( 50, 56 );
					break;
				case 1:
					BodyMod = 3;
					break;
			}

			int minions = Utility.RandomMinMax( 5, 8 );

			for ( int i = 0; i < minions; ++i )
			{
				BaseCreature minion;

				switch ( type )
				{
					default:
					case 0:
						minion = new Skeleton();
						break;
					case 1:
						minion = new Zombie();
						break;
				}

				minion.Team = this.Team;

				bool validLocation = false;

				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 5; ++j )
				{
					int x = X + Utility.Random( 8 ) - 4;
					int y = Y + Utility.Random( 8 ) - 4;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				minion.MoveToWorld( loc, map );
				minion.Combatant = Combatant;

				m_Minions.Add( minion );
			}

			m_NextReturnTime = DateTime.UtcNow + TimeSpan.FromSeconds( m_ReturnTime );
		}

		private void PoisonAttack()
		{
			Combatant.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
			Combatant.PlaySound( 0x474 );

			Combatant.ApplyPoison( this, Poison.GetPoison( 4 ) );
		}

		public override void OnThink()
		{
			if ( BodyMod != 0 )
			{
				if ( CountAliveMinions() == 0 || DateTime.UtcNow > m_NextReturnTime )
				{
					m_Minions.Clear();

					BaseMobileHelper.ShowMorphEffect( this );

					BodyMod = 0;
				}
			}

			if ( !OverrideRules || Combatant == null )
			{
				base.OnThink();

				return;
			}

			if ( DateTime.UtcNow >= m_NextAbilityTime )
			{
				if ( m_AbilityChance > Utility.RandomDouble() )
				{
					if ( Utility.RandomBool() )
					{
						PoisonAttack();
					}
					else
					{
						SpawnMinions();
					}
				}

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}

			base.OnThink();
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
		}

		public override int GetIdleSound()
		{
			return 0x19D;
		}

		public override int GetAngerSound()
		{
			return 0x175;
		}

		public override int GetDeathSound()
		{
			return 0x108;
		}

		public override int GetAttackSound()
		{
			return 0xE2;
		}

		public override int GetHurtSound()
		{
			return 0x28B;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool Unprovokable { get { return true; } }
		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 5; } }

		public AncientLich( Serial serial )
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
			/*int version = */reader.ReadInt();

			m_Minions = new ArrayList();
		}
	}
}
