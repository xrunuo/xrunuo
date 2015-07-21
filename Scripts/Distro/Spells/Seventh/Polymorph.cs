using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Engines.BuffIcons;

namespace Server.Spells.Seventh
{
	public class PolymorphSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Polymorph", "Vas Ylem Rel",
				221,
				9002,
				Reagent.Bloodmoss,
				Reagent.SpidersSilk,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		private int m_NewBody;

		public PolymorphSpell( Mobile caster, Item scroll, int body )
			: base( caster, scroll, m_Info )
		{
			m_NewBody = body;
		}

		public PolymorphSpell( Mobile caster, Item scroll )
			: this( caster, scroll, 0 )
		{
		}

		public override bool CheckCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1010521 ); // You cannot polymorph while you have a Town Sigil
				return false;
			}
			else if ( Necromancy.TransformationSpell.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061633 ); // You cannot polymorph while in that form.
				return false;
			}
			else if ( Ninjitsu.AnimalForm.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1063221 ); // You cannot polymorph while mimicking an animal.
				return false;
			}
			else if ( DisguiseGump.IsDisguised( Caster ) )
			{
				Caster.SendLocalizedMessage( 502167 ); // You cannot polymorph while disguised.
				return false;
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042512 ); // You cannot polymorph while wearing body paint
				return false;
			}
			else if ( Caster.Flying )
			{
				Caster.SendLocalizedMessage( 1113415 ); // You cannot use this ability while flying.
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				return false;
			}
			else if ( m_NewBody == 0 )
			{
				Caster.SendGump( new PolymorphGump( Caster, Scroll ) );

				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1010521 ); // You cannot polymorph while you have a Town Sigil
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
			}
			else if ( Necromancy.TransformationSpell.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061633 ); // You cannot polymorph while in that form.
			}
			else if ( DisguiseGump.IsDisguised( Caster ) )
			{
				Caster.SendLocalizedMessage( 502167 ); // You cannot polymorph while disguised.
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendLocalizedMessage( 1042512 ); // You cannot polymorph while wearing body paint
			}
			else if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) || Caster.IsBodyMod )
			{
				DoFizzle();
			}
			else if ( CheckSequence() )
			{
				if ( Caster.BeginAction( typeof( PolymorphSpell ) ) )
				{
					if ( m_NewBody != 0 )
					{
						if ( !( (Body) m_NewBody ).IsHuman )
						{
							Mobiles.IMount mt = Caster.Mount;

							if ( mt != null )
								mt.Rider = null;
						}

						Caster.BodyMod = m_NewBody;

						if ( m_NewBody == 400 || m_NewBody == 401 )
							Caster.HueMod = Utility.RandomSkinHue();
						else
							Caster.HueMod = 0;

						BaseArmor.ValidateMobile( Caster );
						BaseClothing.ValidateMobile( Caster );

						StopTimer( Caster );

						Timer t = new InternalTimer( Caster );

						m_Timers[Caster] = t;

						BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.Polymorph, 1075824, 1075823, t.Delay, Caster, String.Format( "{0}\t{1}", GetArticleCliloc( m_NewBody ), GetFormCliloc( m_NewBody ) ) ) );

						t.Start();
					}
				}
				else
				{
					Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				}
			}

			FinishSequence();
		}

		private static TextDefinition GetArticleCliloc( int body )
		{
			if ( body == 0x11 || body == 0x01 )
				return "an";

			return "a";
		}

		private static TextDefinition GetFormCliloc( int body )
		{
			switch ( body )
			{
				case 0xD9: return 1028476; // dog
				case 0xE1: return 1028482; // wolf
				case 0xD6: return 1028450; // panther
				case 0x1D: return 1028437; // gorilla
				case 0xD3: return 1028472; // black bear
				case 0xD4: return 1028478; // grizzly bear
				case 0xD5: return 1018276; // polar bear
				case 0x190: return 1028454; // human male
				case 0x191: return 1028455; // human female
				case 0x11: return 1018110; // orc
				case 0x21: return 1018128; // lizardman
				case 0x04: return 1018097; // gargoyle
				case 0x01: return 1018094; // ogre
				case 0x36: return 1018147; // troll
				case 0x02: return 1018111; // ettin
				case 0x09: return 1018103; // daemon
				default: return -1;
			}
		}

		private static Hashtable m_Timers = new Hashtable();

		public static bool StopTimer( Mobile m )
		{
			Timer t = (Timer) m_Timers[m];

			if ( t != null )
			{
				t.Stop();
				m_Timers.Remove( m );
			}

			return ( t != null );
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Owner;

			public InternalTimer( Mobile owner )
				: base( TimeSpan.FromSeconds( 0 ) )
			{
				m_Owner = owner;

				int val = (int) owner.Skills[SkillName.Magery].Value;

				Utility.FixMax( ref val, 60 );

				Delay = TimeSpan.FromSeconds( val );
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );

					BaseArmor.ValidateMobile( m_Owner );
					BaseClothing.ValidateMobile( m_Owner );

					BuffInfo.RemoveBuff( m_Owner, BuffIcon.Polymorph );
				}
			}
		}
	}
}