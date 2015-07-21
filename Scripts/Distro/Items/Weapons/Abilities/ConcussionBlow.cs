using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	/// <summary>
	/// This devastating strike is most effective against those who are in good health and whose reserves of mana are low, or vice versa.
	/// </summary>
	public class ConcussionBlow : WeaponAbility
	{
		public ConcussionBlow()
		{
		}

		public override int BaseMana { get { return 25; } }

		private static Dictionary<Mobile, InternalTimer> m_Table = new Dictionary<Mobile, InternalTimer>();

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			attacker.SendLocalizedMessage( 1060165 ); // You have delivered a concussion!
			defender.SendLocalizedMessage( 1060166 ); // You feel disoriented!

			defender.PlaySound( 0x213 );
			defender.FixedParticles( 0x377A, 1, 32, 9949, 1153, 0, EffectLayer.Head );

			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + 10 ), defender.Map ), new DummyEntity( Serial.Zero, new Point3D( defender.X, defender.Y, defender.Z + 20 ), defender.Map ), 0x36FE, 1, 0, false, false, 1133, 3, 9501, 1, 0, EffectLayer.Waist, 0x100 );

			defender.Damage( 10, attacker );

			InternalTimer timer;
			int mana;

			if ( m_Table.ContainsKey( defender ) )
			{
				timer = m_Table[defender];
				timer.Stop();

				mana = timer.Mana;
			}
			else
			{
				mana = defender.Mana / 2;
				defender.Mana -= mana;
			}

			timer = m_Table[defender] = new InternalTimer( defender, mana );
			timer.Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_Mana;

			public int Mana { get { return m_Mana; } }

			public InternalTimer( Mobile m, int mana )
				: base( TimeSpan.FromSeconds( 6.0 ) )
			{
				m_Mobile = m;
				m_Mana = mana;

				}

			protected override void OnTick()
			{
				if ( m_Mobile.Alive && !m_Mobile.IsDeadBondedPet )
				{
					m_Mobile.Mana += m_Mana;

					m_Mobile.FixedEffect( 0x3779, 10, 25 );
					m_Mobile.PlaySound( 0x28E );
				}

				m_Table.Remove( m_Mobile );
			}
		}
	}
}