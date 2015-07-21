using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	public class Mephitis : BaseChampion
	{
		public override ChampionSkullType SkullType { get { return ChampionSkullType.Venom; } }

		private const double ChanceToThrowWeb = 0.25; // 25% chance for throwing web

		public static Hashtable m_Table = new Hashtable();

		public static bool UnderWebEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public override Type[] UniqueList { get { return new Type[] { typeof( Calm ) }; } }
		public override Type[] SharedList { get { return new Type[] { typeof( OblivionsNeedle ), typeof( ANecromancerShroud ) }; } }
		public override Type[] DecorativeList { get { return new Type[] { typeof( Web ), typeof( MonsterStatuette ) }; } }

		public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { MonsterStatuetteType.Spider }; } }

		[Constructable]
		public Mephitis()
			: base( AIType.AI_Mephitis )
		{
			Body = 173;
			Name = "Mephitis";

			BaseSoundID = 0x183;

			SetStr( 505, 1000 );
			SetDex( 102, 300 );
			SetInt( 402, 600 );

			SetStam( 105, 600 );

			SetDamage( 21, 33 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetHits( 12000 );

			SetResistance( ResistanceType.Physical, 75, 80 );
			SetResistance( ResistanceType.Fire, 65, 75 );
			SetResistance( ResistanceType.Cold, 65, 75 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			SetSkill( SkillName.MagicResist, 70.7, 140.0 );
			SetSkill( SkillName.Tactics, 110.0, 130.0 );
			SetSkill( SkillName.Wrestling, 115.0, 125.0 );

			Fame = 22500;
			Karma = -22500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 3 );
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 5; } }

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			Mobile person = attacker;

			if ( attacker is BaseCreature )
			{
				if ( ( (BaseCreature) attacker ).Summoned )
				{
					person = ( (BaseCreature) attacker ).SummonMaster;
				}
				else
				{
					person = attacker;
				}
			}
			else
			{
				person = attacker;
			}

			if ( person == null )
			{
				return;
			}

			if ( ChanceToThrowWeb >= Utility.RandomDouble() && ( person is PlayerMobile ) && !UnderWebEffect( person ) )
			{
				Direction = this.GetDirectionTo( person );

				MovingEffect( person, 0xF7E, 10, 1, true, false, 0x496, 0 );

				MephitisCocoon mCocoon = new MephitisCocoon( (PlayerMobile) person );

				m_Table[person] = true;

				mCocoon.MoveToWorld( person.Location, person.Map );
				mCocoon.Movable = false;
			}
		}

		public Mephitis( Serial serial )
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

		public class MephitisCocoon : Item
		{
			private DelayTimer m_Timer;

			public MephitisCocoon( PlayerMobile target )
				: base( 0x10da )
			{
				Weight = 1.0;
				int nCocoonID = (int) ( 3 * Utility.RandomDouble() );
				ItemID = 4314 + nCocoonID; // is this correct itemid?

				target.Frozen = true;
				target.Hidden = true;

				target.SendLocalizedMessage( 1042555 ); // You become entangled in the spider web.

				m_Timer = new DelayTimer( this, target );
				m_Timer.Start();
			}

			public MephitisCocoon( Serial serial )
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

		private class DelayTimer : Timer
		{
			private PlayerMobile m_Target;
			private MephitisCocoon m_Cocoon;

			private int m_Ticks;

			public DelayTimer( MephitisCocoon mCocoon, PlayerMobile target )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Target = target;
				m_Cocoon = mCocoon;

				m_Ticks = 0;
			}

			protected override void OnTick()
			{
				m_Ticks++;

				if ( !m_Target.Alive )
				{
					FreeMobile( true );
					return;
				}

				if ( m_Ticks != 6 )
					return;

				FreeMobile( true );
			}

			public void FreeMobile( bool recycle )
			{
				if ( !m_Target.Deleted )
				{
					m_Target.Frozen = false;
					m_Target.SendLocalizedMessage( 1042532 ); // You free yourself from the web!

					Mephitis.m_Table.Remove( m_Target );

					if ( m_Target.Alive )
						m_Target.Hidden = false;
				}

				if ( recycle )
					m_Cocoon.Delete();

				this.Stop();
			}
		}
	}
}
