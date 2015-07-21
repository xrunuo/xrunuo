using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Engines.BuffIcons;

namespace Server.Spells.Fifth
{
	public class IncognitoSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Incognito", "Kal In Ex",
				206,
				9002,
				Reagent.Bloodmoss,
				Reagent.Garlic,
				Reagent.Nightshade
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public IncognitoSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1010445 ); // You cannot incognito if you have a sigil
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				return false;
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042402 ); // You cannot use incognito while wearing body paint
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1010445 ); // You cannot incognito if you have a sigil
			}
			else if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042402 ); // You cannot use incognito while wearing body paint
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) || Caster.IsBodyMod )
			{
				DoFizzle();
			}
			else if ( CheckSequence() )
			{
				if ( Caster.BeginAction( typeof( IncognitoSpell ) ) )
				{
					DisguiseGump.StopTimer( Caster );

					Race race = Caster.Race;

					Caster.BodyMod = Utility.RandomList( race.MaleBody, race.FemaleBody );
					Caster.HueMod = race.RandomSkinHue();
					Caster.NameMod = Caster.Body.IsFemale ? NameList.RandomName( "female" ) : NameList.RandomName( "male" );

					PlayerMobile pm = Caster as PlayerMobile;

					if ( pm != null )
					{
						pm.SetHairMods( race.RandomHair( pm.Body.IsFemale ), race.RandomFacialHair( pm.Body.IsFemale ) );

						pm.HairHue = race.RandomHairHue();
						pm.FacialHairHue = race.RandomHairHue();
					}

					Caster.FixedParticles( 0x373A, 10, 15, 5036, EffectLayer.Head );
					Caster.PlaySound( 0x3BD );

					BaseArmor.ValidateMobile( Caster );
					BaseClothing.ValidateMobile( Caster );

					StopTimer( Caster );

					int timeVal = ( ( 6 * Caster.Skills.Magery.Fixed ) / 50 ) + 1;

					Utility.FixMax( ref timeVal, 144 );

					TimeSpan length = TimeSpan.FromSeconds( timeVal );

					Timer t = new InternalTimer( Caster, length );

					m_Timers[Caster] = t;

					t.Start();

					BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.Incognito, 1075819, length, Caster ) );
				}
				else
				{
					Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				}
			}

			FinishSequence();
		}

		private static Hashtable m_Timers = new Hashtable();

		public static bool StopTimer( Mobile m )
		{
			Timer t = (Timer) m_Timers[m];

			if ( t != null )
			{
				t.Stop();
				m_Timers.Remove( m );

				BuffInfo.RemoveBuff( m, BuffIcon.Incognito );
			}

			return ( t != null );
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Owner;

			public InternalTimer( Mobile owner, TimeSpan length )
				: base( length )
			{
				m_Owner = owner;

			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( IncognitoSpell ) ) )
				{
					if ( m_Owner is PlayerMobile )
						( (PlayerMobile) m_Owner ).SetHairMods( -1, -1 );

					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.NameMod = null;
					m_Owner.EndAction( typeof( IncognitoSpell ) );

					BaseArmor.ValidateMobile( m_Owner );
					BaseClothing.ValidateMobile( m_Owner );
				}
			}
		}
	}
}