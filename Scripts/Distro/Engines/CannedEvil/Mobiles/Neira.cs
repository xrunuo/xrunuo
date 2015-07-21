using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	public class Neira : BaseChampion
	{
		public override ChampionSkullType SkullType { get { return ChampionSkullType.Death; } }

		public override Type[] UniqueList { get { return new Type[] { typeof( ShroudOfDeciet ) }; } }
		public override Type[] SharedList
		{
			get
			{
				return new Type[] {
					typeof( ANecromancerShroud ),
					typeof( DetectiveBoots ),
					typeof( CaptainJohnsHat )
				};
			}
		}
		public override Type[] DecorativeList { get { return new Type[] { typeof( WallBlood ), typeof( TatteredAncientMummyWrapping ) }; } }

		public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { }; } }

		[Constructable]
		public Neira()
			: base( AIType.AI_Mage )
		{
			Name = "Neira";
			Title = "the necromancer";
			Body = 401;
			Hue = 0x83EC;

			SetStr( 305, 425 );
			SetDex( 72, 150 );
			SetInt( 505, 750 );

			SetHits( 4800 );
			SetStam( 102, 300 );

			SetDamage( 25, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 97.6, 100.0 );
			SetSkill( SkillName.Swords, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			Female = true;

			AddItem( new HoodedShroudOfShadows() { Movable = false } );
			AddItem( new Scimitar() { Hue = 38, Movable = false } );

			new SkeletalMount().Rider = this;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 6 );
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.Meager );
		}

		protected override void OnAfterDeath( Container c )
		{
			NecromancerSpellbook book = new NecromancerSpellbook();
			book.Content = ( 1ul << book.BookCount ) - 1;
			c.DropItem( book );

			base.OnAfterDeath( c );
		}

		protected override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( mount != null )
			{
				mount.Rider = null;
			}

			if ( mount is Mobile )
			{
				( (Mobile) mount ).Delete();
			}

			return base.OnBeforeDeath();
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }

		public override bool ShowFameTitle { get { return false; } }
		public override bool ClickTitle { get { return false; } }

		public override int TreasureMapLevel { get { return 5; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to drop or throw an unholy bone
			{
				AddUnholyBone( defender, 0.25 );
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( this.Hits < (int) ( 0.25 * this.HitsMax ) )
			{
				PassiveSpeed /= 1.20;
				ActiveSpeed /= 1.20;
			}

			if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to drop or throw an unholy bone
			{
				AddUnholyBone( attacker, 0.25 );
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			base.AlterDamageScalarFrom( caster, ref scalar );

			if ( this.Hits < (int) ( 0.25 * this.HitsMax ) )
			{
				PassiveSpeed /= 1.20;
				ActiveSpeed /= 1.20;
			}

			if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to throw an unholy bone
			{
				AddUnholyBone( caster, 1.0 );
			}
		}

		public void AddUnholyBone( Mobile target, double chanceToThrow )
		{
			if ( chanceToThrow >= Utility.RandomDouble() )
			{
				Direction = this.GetDirectionTo( target );
				MovingEffect( target, 0xF7E, 10, 1, true, false, 0x496, 0 );
				new DelayTimer( this, target ).Start();
			}
			else
			{
				new UnholyBone().MoveToWorld( Location, Map );
			}
		}

		private class DelayTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile m_Target;

			public DelayTimer( Mobile m, Mobile target )
				: base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_Target = target;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.CanBeHarmful( m_Target ) )
				{
					m_Mobile.DoHarmful( m_Target );
					AOS.Damage( m_Target, m_Mobile, Utility.RandomMinMax( 10, 20 ), 100, 0, 0, 0, 0 );
					new UnholyBone().MoveToWorld( m_Target.Location, m_Target.Map );
				}
			}
		}

		public Neira( Serial serial )
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