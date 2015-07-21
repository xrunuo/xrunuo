using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a wisp corpse" )]
	public class DarkWisp : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Wisp; } }

		public override bool AlwaysMurderer { get { return true; } }

		private bool m_ActiveBarrier;
		private DateTime m_NextBarrierTime;
		private Timer m_BarrierTimer;

		[Constructable]
		public DarkWisp()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dark wisp";
			Body = 165;
			BaseSoundID = 466;

			SetStr( 302, 323 );
			SetDex( 207, 224 );
			SetInt( 202, 222 );

			SetHits( 119, 130 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Physical, 5 );
			SetDamageType( ResistanceType.Energy, 95 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.MagicResist, 80.1, 90.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.Wrestling, 50.0 );
			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 100.1, 110.0 );
			SetSkill( SkillName.Meditation, 20.0 );
			SetSkill( SkillName.Necromancy, 80.1, 90.0 );
			SetSkill( SkillName.SpiritSpeak, 100.1, 110.0 );

			Fame = 15000;
			Karma = -15000;

			AddItem( new LightSource() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Average );
		}

		#region Magic Barrier
		public override void AlterSpellDamageFrom( Mobile from, ref int damage )
		{
			base.AlterSpellDamageFrom( from, ref damage );

			if ( m_ActiveBarrier && from.Backpack != null && from.Backpack.FindItemByType<SmallPieceOfBlackrock>() == null )
				damage = 0;
		}

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			base.AlterMeleeDamageFrom( from, ref damage );

			if ( m_ActiveBarrier && from.Backpack != null && from.Backpack.FindItemByType<SmallPieceOfBlackrock>() == null )
				damage = 0;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( Combatant != null && m_BarrierTimer == null && 0.1 > Utility.RandomDouble() && DateTime.Now > m_NextBarrierTime )
			{
				HueMod = 1;
				Frozen = true;
				Effects.SendPacket( Location, Map, new LocationEffect( new Point3D( X, Y, Z + 5 ), 0x3660, 16, 244, 0x497, 6 ) );

				m_ActiveBarrier = true;

				m_BarrierTimer = new BarrierTimer( this );
				m_BarrierTimer.Start();
			}
		}

		public void RemoveBarrier()
		{
			HueMod = -1;
			Frozen = false;
			Effects.SendPacket( Location, Map, new LocationEffect( new Point3D( X, Y, Z + 5 ), 0x3728, 13, 13, 0x481, 4 ) );

			m_ActiveBarrier = false;

			m_BarrierTimer = null;
			m_NextBarrierTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 24, 31 ) );
		}

		private class BarrierTimer : Timer
		{
			private DarkWisp m_Owner;
			private int m_Ticks;

			public BarrierTimer( DarkWisp owner )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Owner = owner;
				m_Ticks = Utility.RandomMinMax( 9, 11 );
				}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
					Stop();

				m_Owner.Hits += 20;

				if ( --m_Ticks == 0 )
				{
					m_Owner.RemoveBarrier();
					Stop();
				}
			}
		}
		#endregion

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new FaeryDust() );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 8; } }

		public DarkWisp( Serial serial )
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
}
