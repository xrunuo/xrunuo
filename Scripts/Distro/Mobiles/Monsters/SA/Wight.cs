using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a wight's corpse" )]
	public class Wight : BaseCreature
	{
		[Constructable]
		public Wight()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a wight";
			Body = 252;
			Hue = 1150;
			BaseSoundID = 0x482;

			SetStr( 150, 200 );
			SetDex( 50, 60 );
			SetInt( 150, 200 );

			SetHits( 150, 250 );

			SetDamage( 13, 20 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.Magery, 60.1, 80.0 );
			SetSkill( SkillName.Meditation, 50.1, 60.0 );
			SetSkill( SkillName.MagicResist, 40.1, 50.0 );
			SetSkill( SkillName.Tactics, 45.1, 55.0 );
			SetSkill( SkillName.Wrestling, 50.1, 60.0 );

			Fame = 15000;
			Karma = -15000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public override int GetDeathSound() { return 0x258; }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			// TODO: Taken from "Lady of the snow". Check real ability at OSI.

			if ( 0.1 > Utility.RandomDouble() )
			{
				ExpireTimer timer = (ExpireTimer) m_Table[defender];

				if ( timer != null )
				{
					timer.DoExpire();
					defender.SendLocalizedMessage( 1070831 ); // The freezing wind continues to blow!
				}
				else
					defender.SendLocalizedMessage( 1070832 ); // An icy wind surrounds you, freezing your lungs as you breathe!

				timer = new ExpireTimer( defender, this );
				timer.Start();

				m_Table[defender] = timer;
			}
		}

		private static Hashtable m_Table = new Hashtable();

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile m_From;
			private int m_Count;

			public ExpireTimer( Mobile m, Mobile from )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				}

			public void DoExpire()
			{
				Stop();
				m_Table.Remove( m_Mobile );
			}

			public void DrainLife()
			{
				if ( m_Mobile.Alive )
					m_Mobile.Damage( 2, m_From );
				else
					DoExpire();
			}

			protected override void OnTick()
			{
				DrainLife();

				if ( ++m_Count >= 5 )
				{
					DoExpire();
					m_Mobile.SendLocalizedMessage( 1070830 ); // The icy wind dissipates.
				}
			}
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 3; } }

		public Wight( Serial serial )
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