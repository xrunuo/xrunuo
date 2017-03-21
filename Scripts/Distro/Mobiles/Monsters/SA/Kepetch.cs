using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a kepetch's corpse" )]
	public class Kepetch : BaseCreature, ICarvable
	{
		private bool m_Shorn;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Shorn
		{
			get { return m_Shorn; }
			set { m_Shorn = value; }
		}

		[Constructable]
		public Kepetch()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a kepetch";
			Body = 726;

			SetStr( 347, 353 );
			SetDex( 186, 206 );
			SetInt( 32, 34 );

			SetHits( 328, 357 );

			SetDamage( 7, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 40, 45 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			SetSkill( SkillName.Anatomy, 115.0, 130.0 );
			SetSkill( SkillName.MagicResist, 80.0, 100.0 );
			SetSkill( SkillName.Tactics, 115.0, 125.0 );
			SetSkill( SkillName.Wrestling, 105.0, 115.0 );

			Fame = 6000;
			Karma = -6000;
		}

		public override int GetAttackSound() { return 0x606; }
		public override int GetDeathSound() { return 0x607; }
		public override int GetHurtSound() { return 0x608; }
		public override int GetIdleSound() { return 0x609; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 10; } }

		public override int Meat { get { return 5; } }
		public override int Blood { get { return 8; } }
		public override int Hides { get { return 14; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override int Fur { get { return 15; } }
		public override FurType FurType { get { return FurType.DarkGreen; } }

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 > Utility.RandomDouble() && !m_Table.ContainsKey( defender ) )
			{
				defender.SendLocalizedMessage( 1112472 ); // You've suffered a vicious bite!
				defender.SendLocalizedMessage( 1113211 ); // The kepetch gives you a particularly vicious bite!

				defender.FixedParticles( 0x37CC, 1, 10, 0x3EB, 0, 0, EffectLayer.Waist );
				defender.PlaySound( 0x51D );

				Timer timer = new InternalTimer( defender );
				timer.Start();
				m_Table[defender] = timer;
			}
		}

		private static Hashtable m_Table = new Hashtable();

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_Damage;

			public InternalTimer( Mobile m )
				: base( TimeSpan.FromMinutes( 1.0 ), TimeSpan.FromSeconds( 20.0 ), 10 )
			{
				m_Mobile = m;
				
				m_Damage = 5;
			}

			protected override void OnTick()
			{
				if ( !m_Mobile.Alive )
				{
					Stop();
					m_Table.Remove( m_Mobile );
				}
				else
				{
					m_Mobile.Damage( m_Damage );
					m_Mobile.SendLocalizedMessage( 1112473 ); // Your vicious wound is festering!

					m_Damage += 5;

					if ( m_Damage > 50 )
						m_Table.Remove( m_Mobile );
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public void Carve( Mobile from, Item item )
		{
			if ( m_Shorn )
			{
				// The Kepetch nimbly escapes your attempts to shear its mane.
				PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1112358, from.NetState );
				return;
			}

			Item fur = new DarkGreenFur( Fur );

			if ( from.PlaceInBackpack( fur ) )
			{
				from.SendLocalizedMessage( 1112360 ); // You place the gathered kepetch fur into your backpack.
			}
			else
			{
				from.SendLocalizedMessage( 1112359 ); // You would not be able to place the gathered kepetch fur in your backpack!
				fur.MoveToWorld( from.Location, from.Map );
			}

			m_Shorn = true;
		}

		public Kepetch( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.Write( (bool) m_Shorn );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Shorn = reader.ReadBool();
						break;
					}
			}
		}
	}
}
