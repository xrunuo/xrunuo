using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.BuffIcons;

namespace Server.Spells.Spellweaving
{
	public class EssenceOfWindSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Essence of Wind", "Anathrae",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 3.0 ); } }

		public override double RequiredSkill { get { return 52.0; } }
		public override int RequiredMana { get { return 40; } }

		public EssenceOfWindSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.PlaySound( 0x5C6 );

				int focuslevel = GetFocusLevel( Caster );

				int range = 5 + focuslevel;
				int damage = 25 + focuslevel;

				double skill = Caster.Skills[SkillName.Spellweaving].Value;

				TimeSpan duration = TimeSpan.FromSeconds( (int) ( skill / 24 ) + focuslevel );

				int fcMalus = focuslevel + 1;
				int ssiMalus = 2 * ( focuslevel + 1 );

				ArrayList targets = new ArrayList();

				foreach ( Mobile m in Caster.GetMobilesInRange( range ) )
				{
					if ( Caster != m && Caster.InLOS( m ) && SpellHelper.ValidIndirectTarget( Caster, m ) && Caster.CanBeHarmful( m, false ) )
						targets.Add( m );
				}

				for ( int i = 0; i < targets.Count; i++ )
				{
					Mobile m = (Mobile) targets[i];

					Caster.DoHarmful( m );

					SpellHelper.Damage( this, m, damage, 0, 0, 100, 0, 0 );

					if ( !CheckResisted( m ) )	//No message on resist
					{
						m_Table[m] = new EssenceOfWindInfo( m, fcMalus, ssiMalus, duration );

						BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.EssenceOfWind, 1075802, duration, m, String.Format( "{0}\t{1}", fcMalus.ToString(), ssiMalus.ToString() ) ) );
					}
				}
			}

			FinishSequence();
		}

		private static Hashtable m_Table = new Hashtable();

		private class EssenceOfWindInfo
		{
			private Mobile m_Defender;
			private int m_FCMalus;
			private int m_SSIMalus;
			private ExpireTimer m_Timer;

			public Mobile Defender { get { return m_Defender; } }
			public int FCMalus { get { return m_FCMalus; } }
			public int SSIMalus { get { return m_SSIMalus; } }
			public ExpireTimer Timer { get { return m_Timer; } }

			public EssenceOfWindInfo( Mobile defender, int fcMalus, int ssiMalus, TimeSpan duration )
			{
				m_Defender = defender;
				m_FCMalus = fcMalus;
				m_SSIMalus = ssiMalus;

				m_Timer = new ExpireTimer( m_Defender, duration );
				m_Timer.Start();
			}
		}

		public static int GetFCMalus( Mobile m )
		{
			if ( m_Table.ContainsKey( m ) )
				return ( (EssenceOfWindInfo) m_Table[m] ).FCMalus;

			return 0;
		}

		public static int GetSSIMalus( Mobile m )
		{
			if ( m_Table.ContainsKey( m ) )
				return ( (EssenceOfWindInfo) m_Table[m] ).SSIMalus;

			return 0;
		}

		public static bool IsDebuffed( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public static void StopDebuffing( Mobile m, bool message )
		{
			if ( m_Table.ContainsKey( m ) )
				( (EssenceOfWindInfo) m_Table[m] ).Timer.DoExpire( message );
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;

			public ExpireTimer( Mobile m, TimeSpan delay )
				: base( delay )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				DoExpire( true );
			}

			public void DoExpire( bool message )
			{
				Stop();

				m_Table.Remove( m_Mobile );

				BuffInfo.RemoveBuff( m_Mobile, BuffIcon.EssenceOfWind );
			}
		}
	}
}