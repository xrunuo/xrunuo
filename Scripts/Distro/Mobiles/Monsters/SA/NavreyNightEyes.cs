using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Network;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Navrey Night-Eyes corpse" )]
	public class NavreyNightEyes : BaseCreature
	{
		private NavreysController m_Spawner;
		private bool m_UsedPillars;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool UsedPillars
		{
			get { return m_UsedPillars; }
			set { m_UsedPillars = value; }
		}

		[Constructable]
		public NavreyNightEyes( NavreysController spawner )
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			m_Spawner = spawner;

			Name = "Navrey Night-Eyes";
			Body = 735;

			SetStr( 1000, 1500 );
			SetDex( 200, 250 );
			SetInt( 150, 200 );

			SetHits( 50000 );

			SetDamage( 22, 29 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60, 80 );

			SetSkill( SkillName.Anatomy, 50.0, 80.0 );
			SetSkill( SkillName.Poisoning, 100, 100 );
			SetSkill( SkillName.MagicResist, 100.0, 130.0 );
			SetSkill( SkillName.Tactics, 90.0, 100.0 );
			SetSkill( SkillName.Wrestling, 90.0, 100.0 );
			SetSkill( SkillName.EvalInt, 90.0, 100.0 );
			SetSkill( SkillName.Magery, 90.0, 100.0 );
			SetSkill( SkillName.Meditation, 80.0, 100.0 );

			Fame = 30000;
			Karma = -30000;

			for ( int i = 0; i < 6; i++ )
				PackMysticScroll( Utility.Random( 10, 6 ) ); // 6th - 8th circle
		}

		public override int GetAttackSound() { return 0x61A; }
		public override int GetDeathSound() { return 0x61B; }
		public override int GetHurtSound() { return 0x61C; }
		public override int GetIdleSound() { return 0x61D; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 75; } }

		public override bool AlwaysMurderer { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		
		public override int TreasureMapLevel { get { return 5; } }		

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.5 >= Utility.RandomDouble() )
				c.DropItem( new SpiderCarapace() );

			if ( !m_UsedPillars )
			{
				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new Venom() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new TokenOfHolyFavor() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new DemonBridleRing() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new BladeOfBattle() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new StormCaller() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new BreastplateOfTheBerserker() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new GiantSteps() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new SwordOfShatteredHopes() );

				if ( 1500 > Utility.Random( 100000 ) )
					c.DropItem( new SummonersKilt() );
			}
		}

		public override bool OnBeforeDeath()
		{
			if ( !base.OnBeforeDeath() )
				return false;

			m_Spawner.OnNavreyKilled();

			return true;
		}

		public override void OnKilledBy( Mobile mob )
		{
			base.OnKilledBy( mob );

			PlayerMobile pm = mob as PlayerMobile;

			if ( pm != null && pm.Backpack != null )
			{
				if ( QuestHelper.HasQuest<GreenWithEnvyQuest>( pm ) )
				{
					// As Navrey Night-Eyes dies, you find and claim one of her eyes as proof of her demise.
					pm.SendLocalizedMessage( 1095155 );

					pm.Backpack.DropItem( new EyeOfNavrey() );
				}

				if ( !m_UsedPillars && 0.015 >= Utility.RandomDouble() )
				{
					Item reward;

					switch ( Utility.Random( 3 ) )
					{
						default:
						case 0: reward = new NightEyes(); break;
						case 1: reward = new TangleA(); break;
						case 2: reward = new ScrollOfTranscendence( Utility.RandomSkill(), 3.0 ); break;
					}

					pm.Backpack.DropItem( reward );
					pm.SendLocalizedMessage( 502088 ); // A special gift has been placed in your backpack.
				}
			}
		}

		private Mobile SelectWebTarget()
		{
			foreach ( Mobile mob in this.GetMobilesInRange( RangePerception ) )
			{
				Mobile m = mob;

				if ( m.AccessLevel > AccessLevel.Player )
					continue;

				if ( mob is BaseCreature )
				{
					BaseCreature bc = mob as BaseCreature;

					if ( bc.Summoned )
						m = bc.SummonMaster;

					if ( bc.Controlled )
						m = bc.ControlMaster;
				}

				if ( m.Player && !NavreysWeb.IsTrapped( m ) && this.InRange( m, RangePerception ) )
					return m;
			}

			return null;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( 0.1 >= Utility.RandomDouble() )
			{
				PlayerMobile target = SelectWebTarget() as PlayerMobile;

				if ( target != null )
				{
					Direction = this.GetDirectionTo( target );

					if ( 0.25 > Utility.RandomDouble() )
						target.MoveToWorld( TeleportLocations[Utility.Random( TeleportLocations.Length )], target.Map );
					else
						Combatant = target;

					target.PlaySound( 0x389 );

					Effects.SendPacket( Location, Map, new HuedEffect( EffectType.Moving, Serial.Zero, Serial.Zero, 0xEE3 + Utility.Random( 4 ), Location, target.Location, 10, 0, false, false, 0, 0 ) );

					NavreysWeb web = new NavreysWeb( (PlayerMobile) target );

					web.MoveToWorld( target.Location, target.Map );
					web.Movable = false;

					target.Frozen = true;
					target.SendLocalizedMessage( 1113247 ); // You are wrapped in spider webbing and cannot move!
				}
			}
		}

		private static Point3D[] TeleportLocations = new Point3D[]
			{
				new Point3D( 1075, 861, -16 ),
				new Point3D( 1082, 883, -3 ),
				new	Point3D( 1080, 845, 2 ),
				new Point3D( 1064, 841, -8 ),
				new Point3D( 1050, 848, -20 ),
				new Point3D( 1030, 879, -4 ),
			};

		public NavreyNightEyes( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.Write( (Item) m_Spawner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				m_Spawner = reader.ReadItem() as NavreysController;
		}
	}
}

namespace Server.Items
{
	public class NavreysWeb : Item
	{
		private PlayerMobile m_Target;
		private InternalTimer m_Timer;

		public NavreysWeb( PlayerMobile target )
			: base( 0xEE3 + Utility.Random( 4 ) )
		{
			Weight = 1.0;

			m_Target = target;

			m_Timer = new InternalTimer( this, target );
			m_Timer.Start();

			m_WebVictims.Add( target );
		}

		public NavreysWeb( Serial serial )
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

		public void Burn()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_WebVictims.Remove( m_Target );

			Delete();
		}

		public static bool IsTrapped( Mobile m )
		{
			return m_WebVictims.Contains( m );
		}

		private static List<Mobile> m_WebVictims = new List<Mobile>();

		private class InternalTimer : Timer
		{
			private PlayerMobile m_Target;
			private NavreysWeb m_Web;

			private int m_Ticks;

			public InternalTimer( NavreysWeb web, PlayerMobile target )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Target = target;
				m_Web = web;

				m_Ticks = 10;
			}

			protected override void OnTick()
			{
				m_Ticks--;

				if ( !m_Target.Alive || m_Ticks == 0 )
				{
					m_Target.Frozen = false;
					m_Target.SendLocalizedMessage( 1113248 ); // You escape the spider's web!

					m_WebVictims.Remove( m_Target );

					m_Web.Delete();

					Stop();
				}
			}
		}
	}
}
