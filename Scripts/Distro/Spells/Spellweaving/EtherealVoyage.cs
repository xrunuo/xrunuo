using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
using Server.Engines.BuffIcons;

namespace Server.Spells.Spellweaving
{
	public class EtherealVoyageSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Ethereal Voyage", "Orlavdra",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 3.5 ); } }

		public override double RequiredSkill { get { return 24.0; } }
		public override int RequiredMana { get { return 32; } }

		private static Hashtable m_Table = new Hashtable();

		private static Hashtable m_Table2 = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void RemoveEffect( Mobile m )
		{
			m_Table.Remove( m );

			m.BodyMod = 0;
			m.HueMod = -1;

			Timer t = (Timer) m_Table2[m];

			if ( t != null )
			{
				t.Stop();
			}

			m_Table2[m] = t = Timer.DelayCall( TimeSpan.FromMinutes( 5.0 ), new TimerStateCallback( ExpireTime_Callback ), m );

			m.SendLocalizedMessage( 1074771 ); // You are no longer under the effects of Ethereal Voyage.
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			RemoveEffect( m );
		}

		private static void ExpireTime_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m_Table2.Remove( m );
		}

		public EtherealVoyageSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.
				return false;
			}
			else if ( Ninjitsu.AnimalForm.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1063218 ); // You cannot use that ability in this form.
				return false;
			}
			else if ( m_Table.Contains( Caster ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				return false;
			}
			else if ( m_Table2.Contains( Caster ) )
			{
				Caster.SendLocalizedMessage( 1075124 ); // You must wait before casting that spell again.
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.
			}
			else if ( Caster.IsBodyMod || !Caster.CanBeginAction( typeof( IncognitoSpell ) ) || !Caster.CanBeginAction( typeof( PolymorphSpell ) ) || Necromancy.TransformationSpell.UnderTransformation( Caster ) )
			{
				DoFizzle();
			}
			else if ( CheckSequence() )
			{
				if ( Caster.Mount != null )
				{
					IMount mt = Caster.Mount;

					if ( mt != null )
						mt.Rider = null;
				}

				Caster.Flying = false;

				double duration = 25 + ( Caster.Skills[SkillName.Spellweaving].Fixed / 240 );

				duration += SpellweavingSpell.GetFocusLevel( Caster ) * 2;

				Caster.BodyMod = 770;
				Caster.HueMod = 1167;

				Timer t = (Timer) m_Table[Caster];

				if ( t != null )
					t.Stop();

				m_Table[Caster] = t = Timer.DelayCall( TimeSpan.FromSeconds( duration ), new TimerStateCallback( Expire_Callback ), Caster );

				Caster.PlaySound( 0x5C8 );

				Caster.SendLocalizedMessage( 1074770 ); // You are now under the effects of Ethereal Voyage.

				BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.EtherealVoyage, 1075804, TimeSpan.FromSeconds( duration ), Caster ) );
			}

			FinishSequence();
		}
	}
}