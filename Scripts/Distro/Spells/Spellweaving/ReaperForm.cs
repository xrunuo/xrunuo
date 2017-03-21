using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
using Server.Events;

namespace Server.Spells.Spellweaving
{
	public class ReaperFormSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Reaper Form", "Tarisstree",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.5 ); } }

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( OnLogin );
			EventSink.PlayerDeath += new PlayerDeathEventHandler( OnPlayerDeath );
		}

		private static void OnLogin( LoginEventArgs e )
		{
			if ( UnderEffect( e.Mobile ) )
				e.Mobile.InvalidateSpeed();
		}

		private static void OnPlayerDeath( PlayerDeathEventArgs e )
		{
			if ( UnderEffect( e.Mobile ) )
				RemoveEffects( e.Mobile );
		}

		public override double RequiredSkill { get { return 24.0; } }
		public override int RequiredMana { get { return 34; } }

		private static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public ReaperFormSpell( Mobile caster, Item scroll )
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
			else if ( Caster.Flying )
			{
				Caster.SendLocalizedMessage( 1113415 ); // You cannot use this ability while flying.
				return false;
			}

			return base.CheckCast();
		}

		public static int GetSDIBonus( Mobile m )
		{
			if ( m_Table.ContainsKey( m ) )
			{
				int bonus = 10;

				ArcaneFocus focus = SpellweavingSpell.FindArcaneFocus( m );

				if ( focus != null )
					bonus += focus.StrengthBonus;

				return bonus;
			}

			return 0;
		}

		public static void RemoveEffects( Mobile m )
		{
			if ( m_Table.ContainsKey( m ) )
			{
				ArrayList mods = (ArrayList) m_Table[m];

				for ( int i = 0; i < mods.Count; ++i )
					m.RemoveResistanceMod( (ResistanceMod) mods[i] );

				m.BodyMod = 0;
				m.HueMod = -1;

				m_Table.Remove( m );
			}
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
			else if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) || ( Caster.IsBodyMod && !UnderEffect( Caster ) ) )
			{
				Caster.SendLocalizedMessage( 1063218 ); // You cannot use that ability in this form.
			}
			else if ( CheckSequence() )
			{
				if ( UnderEffect( Caster ) )
				{
					RemoveEffects( Caster );

					Caster.PlaySound( 0xFA );
				}
				else
				{
					int offset = 3;

					// Bonus por Arcane Circle
					offset += GetFocusLevel( Caster );

					// Declaramos las Resistencias
					ArrayList mods = new ArrayList();

					// Seteamos las Resistencias
					mods.Add( new ResistanceMod( ResistanceType.Physical, offset ) );
					mods.Add( new ResistanceMod( ResistanceType.Fire, -20 ) );
					mods.Add( new ResistanceMod( ResistanceType.Cold, offset ) );
					mods.Add( new ResistanceMod( ResistanceType.Poison, offset ) );
					mods.Add( new ResistanceMod( ResistanceType.Energy, offset ) );

					// Bajamos de la montura
					if ( Caster.Mount != null )
					{
						IMount mt = Caster.Mount;

						if ( mt != null )
							mt.Rider = null;
					}

					// Aspecto
					Caster.BodyMod = 285;
					Caster.HueMod = 0;

					// Aplicamos las Resistencias
					for ( int i = 0; i < mods.Count; ++i )
						Caster.AddResistanceMod( (ResistanceMod) mods[i] );

					// Salvamos las Resistencias en la Hashtable
					m_Table[Caster] = mods;

					Caster.PlaySound( 442 );
				}
			}

			FinishSequence();
		}
	}
}