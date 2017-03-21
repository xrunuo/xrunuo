using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
using Server.Engines.BuffIcons;
using Server.Events;

namespace Server.Spells.Mysticism
{
	public class StoneFormSpell : MysticismSpell
	{
		public static void Initialize()
		{
			EventSink.PlayerDeath += new PlayerDeathEventHandler( OnPlayerDeath );
		}

		private static SpellInfo m_Info = new SpellInfo(
				"Stone Form", "In Rel Ylem",
				-1,
				9002,
				Reagent.Bloodmoss,
				Reagent.FertileDirt,
				Reagent.Garlic
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.5 ); } }

		public override double RequiredSkill { get { return 33.0; } }
		public override int RequiredMana { get { return 11; } }

		private static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public StoneFormSpell( Mobile caster, Item scroll )
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
					Caster.Delta( MobileDelta.Resistances );
				}
				else
				{
					int offset = (int) ( ( GetBaseSkill( Caster ) + GetBoostSkill( Caster ) ) / 24.0 );

					// Declaramos las Resistencias
					ArrayList mods = new ArrayList();

					// Seteamos las Resistencias
					mods.Add( new ResistanceMod( ResistanceType.Physical, offset ) );
					mods.Add( new ResistanceMod( ResistanceType.Fire, offset ) );
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
					Caster.BodyMod = 0x2C1;
					Caster.HueMod = 0;

					// Aplicamos las Resistencias
					for ( int i = 0; i < mods.Count; ++i )
						Caster.AddResistanceMod( (ResistanceMod) mods[i] );

					// Salvamos las Resistencias en la Hashtable
					m_Table[Caster] = mods;

					Caster.PlaySound( 0x65A );
					Caster.Delta( MobileDelta.Resistances );

					BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.StoneForm, 1080145, 1080146, String.Format( "-10\t-2\t{0}\t{1}\t{2}", offset, GetResistCapBonus( Caster ), GetDIBonus( Caster ) ), false ) );
				}
			}

			FinishSequence();
		}

		public static int GetDIBonus( Mobile m )
		{
			return (int) ( ( GetBaseSkill( m ) + GetBoostSkill( m ) ) / 12.0 );
		}

		public static int GetResistCapBonus( Mobile m )
		{
			return (int) ( ( GetBaseSkill( m ) + GetBoostSkill( m ) ) / 48.0 );
		}

		public static void RemoveEffects( Mobile m )
		{
			ArrayList mods = (ArrayList) m_Table[m];

			for ( int i = 0; i < mods.Count; ++i )
				m.RemoveResistanceMod( (ResistanceMod) mods[i] );

			m.BodyMod = 0;
			m.HueMod = -1;

			m_Table.Remove( m );

			BuffInfo.RemoveBuff( m, BuffIcon.StoneForm );
		}

		private static void OnPlayerDeath( PlayerDeathEventArgs e )
		{
			Mobile m = e.Mobile;

			if ( UnderEffect( m ) )
				RemoveEffects( m );
		}
	}
}