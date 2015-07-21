using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.BuffIcons;

namespace Server.Spells.Spellweaving
{
	public class ThunderstormSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Thunderstorm", "Erelonia",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.5 ); } }

		public override double RequiredSkill { get { return 10.0; } }
		public override int RequiredMana { get { return 32; } }

		public ThunderstormSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.PlaySound( 0x5CE );

				int focuslevel = GetFocusLevel( Caster );

				int range = 2 + focuslevel;
				int damage = 15 + focuslevel;

				TimeSpan duration = TimeSpan.FromSeconds( 5 + focuslevel );

				ArrayList targets = new ArrayList();

				foreach ( Mobile m in Caster.GetMobilesInRange( range ) )
				{
					if ( Caster != m && SpellHelper.ValidIndirectTarget( Caster, m ) && Caster.CanBeHarmful( m, false ) && Caster.InLOS( m ) )
						targets.Add( m );
				}

				for ( int i = 0; i < targets.Count; i++ )
				{
					Mobile m = (Mobile) targets[i];

					Caster.DoHarmful( m );

					Spell oldSpell = m.Spell as Spell;

					SpellHelper.Damage( this, m, damage, 0, 0, 0, 0, 100 );

					if ( oldSpell != null && oldSpell != m.Spell && !CheckResisted( m ) )
					{
						m_Table[m] = Timer.DelayCall( duration, new TimerStateCallback( DoExpire ), m );

						BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Thunderstorm, 1075800, duration, m, GetCastRecoveryMalus( m ) ) );
					}
				}
			}

			FinishSequence();
		}

		private static Hashtable m_Table = new Hashtable();

		public static int GetCastRecoveryMalus( Mobile m )
		{
			return m_Table.ContainsKey( m ) ? 6 : 0;
		}

		public static void DoExpire( object state )
		{
			Mobile m = (Mobile) state;

			if ( m_Table.ContainsKey( m ) )
			{
				Timer t = (Timer) m_Table[m];

				t.Stop();
				m_Table.Remove( m );

				BuffInfo.RemoveBuff( m, BuffIcon.Thunderstorm );
			}
		}
	}
}