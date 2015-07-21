using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Engines.BuffIcons;

namespace Server.Spells.Spellweaving
{
	public class GiftOfLifeSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Gift of Life", "Illorae",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 4.0 ); } }

		public override double RequiredSkill { get { return 38.0; } }
		public override int RequiredMana { get { return 70; } }

		public static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			RemoveEffect( m );
		}

		public static void RemoveEffect( Mobile m )
		{
			m_Table.Remove( m );

			m.SendLocalizedMessage( 1074776 ); // You are no longer protected with Gift of Life.

			BuffInfo.RemoveBuff( m, BuffIcon.GiftOfLife );
		}

		public GiftOfLifeSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( m is PlayerMobile && m != Caster )
			{
				Caster.SendLocalizedMessage( 1072077 ); // You may only cast this spell on yourself or a bonded pet.
			}
			else if ( m is BaseCreature && ( !( (BaseCreature) m ).IsBonded || ( (BaseCreature) m ).ControlMaster != Caster ) )
			{
				Caster.SendLocalizedMessage( 1072077 ); // You may only cast this spell on yourself or a bonded pet.
			}
			else if ( UnderEffect( m ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
			}
			else if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, m );

				double seconds = ( Caster.Skills[SkillName.Spellweaving].Fixed / 240 ) * 60;

				seconds += GetFocusLevel( Caster ) * 60;

				TimeSpan duration = TimeSpan.FromSeconds( seconds );

				Timer t = (Timer) m_Table[m];

				if ( t != null )
					t.Stop();

				m_Table[m] = t = Timer.DelayCall( duration, new TimerStateCallback( Expire_Callback ), m );

				m.PlaySound( 0x244 );
				m.FixedParticles( 0x3709, 1, 30, 9965, 5, 7, EffectLayer.Waist );
				m.FixedParticles( 0x376A, 1, 30, 9502, 5, 3, EffectLayer.Waist );

				BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.GiftOfLife, 1075806, duration, Caster ) );

				if ( m == Caster )
					Caster.SendLocalizedMessage( 1074774 ); // You weave powerful magic, protecting yourself from death.
				else
					Caster.SendLocalizedMessage( 1074775 ); // You weave powerful magic, protecting your pet from death.
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private GiftOfLifeSpell m_Owner;

			public InternalTarget( GiftOfLifeSpell owner )
				: base( -1, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile) o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}