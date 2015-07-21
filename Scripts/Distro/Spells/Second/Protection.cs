using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Engines.BuffIcons;

namespace Server.Spells.Second
{
	public class ProtectionSpell : MagerySpell
	{
		private static Hashtable m_Registry = new Hashtable();
		public static Hashtable Registry { get { return m_Registry; } }

		private static SpellInfo m_Info = new SpellInfo(
				"Protection", "Uus Sanct",
				236,
				9011,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public ProtectionSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			return true;
		}

		private static Hashtable m_Table = new Hashtable();

		public static void Toggle( Mobile caster, Mobile target, bool archprotection )
		{
			/* Players under the protection spell effect can no longer have their spells "disrupted" when hit.
			 * Players under the protection spell have decreased physical resistance stat value,
			 * a decreased "resisting spells" skill value by -35,
			 * and a slower casting speed modifier (technically, a negative "faster cast speed") of 2 points.
			 * The protection spell has an indefinite duration, becoming active when cast, and deactivated when re-cast.
			 * Reactive Armor, Protection, and Magic Reflection will stay on—even after logging out,
			 * even after dying—until you “turn them off” by casting them again.
			 */

			object[] mods = (object[]) m_Table[target];

			if ( mods == null )
			{
				target.PlaySound( 0x1E9 );
				target.FixedParticles( 0x375A, 9, 20, 5016, EffectLayer.Waist );

				int physmod = -15 + (int) ( caster.Skills[SkillName.Inscribe].Value / 20 );
				int resistmod = -35 + (int) ( caster.Skills[SkillName.Inscribe].Value / 20 );

				mods = new object[2]
					{
						new ResistanceMod( ResistanceType.Physical, physmod ),
						new DefaultSkillMod( SkillName.MagicResist, true, resistmod )
					};

				m_Table[target] = mods;
				Registry[target] = 100.0;

				target.AddResistanceMod( (ResistanceMod) mods[0] );
				target.AddSkillMod( (SkillMod) mods[1] );

				BuffInfo.AddBuff( target, new BuffInfo( archprotection ? BuffIcon.ArchProtection : BuffIcon.Protection, archprotection ? 1075816 : 1075814, 1075815, String.Format( "{0}\t{1}", physmod, resistmod ), true ) );
			}
			else
			{
				target.PlaySound( 0x1ED );
				target.FixedParticles( 0x375A, 9, 20, 5016, EffectLayer.Waist );

				m_Table.Remove( target );
				Registry.Remove( target );

				target.RemoveResistanceMod( (ResistanceMod) mods[0] );
				target.RemoveSkillMod( (SkillMod) mods[1] );

				BuffInfo.RemoveBuff( target, BuffIcon.Protection );
				BuffInfo.RemoveBuff( target, BuffIcon.ArchProtection );
			}
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
				Toggle( Caster, Caster, false );

			FinishSequence();
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_Table[m] != null;
		}

		public static void RemoveWard( Mobile target )
		{
			object[] mods = (object[]) m_Table[target];

			if ( mods != null )
			{
				m_Table.Remove( target );
				Registry.Remove( target );

				target.RemoveResistanceMod( (ResistanceMod) mods[0] );
				target.RemoveSkillMod( (SkillMod) mods[1] );

				BuffInfo.RemoveBuff( target, BuffIcon.Protection );
				BuffInfo.RemoveBuff( target, BuffIcon.ArchProtection );
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster;

			public InternalTimer( Mobile caster )
				: base( TimeSpan.FromSeconds( 0 ) )
			{
				double val = caster.Skills[SkillName.Magery].Value * 2.0;
				Utility.FixMinMax( ref val, 15, 240 );

				m_Caster = caster;
				Delay = TimeSpan.FromSeconds( val );
			}

			protected override void OnTick()
			{
				ProtectionSpell.Registry.Remove( m_Caster );
				DefensiveSpell.Nullify( m_Caster );
			}
		}
	}
}