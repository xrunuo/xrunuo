using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;

namespace Server
{
	public class ExpireTimer : Timer
	{
		private Mobile m_Mobile;
		private ResistanceMod[] m_Mod;
		private Hashtable m_Hashtbl;

		public ExpireTimer( Mobile m, ResistanceMod[] mod, TimeSpan delay, Hashtable hashtbl )
			: base( delay )
		{
			m_Mobile = m;

			m_Mod = mod;

			m_Hashtbl = hashtbl;
		}

		public void DoExpire()
		{
			for ( int i = 0; i < m_Mod.Length; i++ )
			{
				m_Mobile.RemoveResistanceMod( m_Mod[i] );
			}

			m_Mobile.PlaySound( 0x1ED );
			m_Mobile.FixedParticles( 0x376A, 9, 32, 5008, EffectLayer.Waist );

			Stop();

			m_Hashtbl.Remove( m_Mobile );
		}

		protected override void OnTick()
		{
			DoExpire();
		}
	}

	public class RageTimer : Timer
	{
		private Mobile m_Mobile;
		private Mobile m_Target;
		private int m_Count;
		private int m_CountMax;

		public RageTimer( Mobile m, Mobile target )
			: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
		{
			m_Mobile = m;
			m_Target = target;
			m_Count = 0;
			m_CountMax = 2 + Utility.Random( 3 );
		}

		public void DoExpire()
		{
			m_Target.SendLocalizedMessage( 1070824 ); // The creature's rage subsides.

			Stop();

			BaseAttackHelperSE.m_RageTable.Remove( m_Target );
		}

		protected override void OnTick()
		{
			int damage = 2 + Utility.Random( 3 );

			if ( m_Target.Deleted || !m_Target.Alive )
			{
				DoExpire();

				return;
			}

			m_Target.PlaySound( 0x133 );
			m_Target.Damage( damage, m_Mobile );

			m_Target.SendLocalizedMessage( 1070825 ); // The creature continues to rage!

			Blood blood = new Blood();
			blood.ItemID = Utility.Random( 0x122A, 5 );
			blood.MoveToWorld( m_Target.Location, m_Target.Map );

			if ( m_Count >= m_CountMax )
			{
				DoExpire();

				return;
			}

			m_Count++;
		}
	}
}