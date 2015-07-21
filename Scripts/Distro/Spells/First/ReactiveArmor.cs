using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Engines.BuffIcons;

namespace Server.Spells.First
{
	public class ReactiveArmorSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Reactive Armor", "Flam Sanct",
				236,
				9011,
				Reagent.Garlic,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.First; } }

		public ReactiveArmorSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			return true;
		}

		private static Hashtable m_Table = new Hashtable();

		public override void OnCast()
		{
			/* The reactive armor spell increases the caster's physical resistance, while lowering the caster's elemental resistances.
			 * 15 + (Inscription/20) Physcial bonus
			 * -5 Elemental
			 * The reactive armor spell has an indefinite duration, becoming active when cast, and deactivated when re-cast. 
			 * Reactive Armor, Protection, and Magic Reflection will stay on—even after logging out, even after dying—until you “turn them off” by casting them again. 
			 * (+20 physical -5 elemental at 100 Inscription)
			 */

			if ( CheckSequence() )
			{
				Mobile targ = Caster;

				ResistanceMod[] mods = (ResistanceMod[]) m_Table[targ];

				if ( mods == null )
				{
					targ.PlaySound( 0x1E9 );
					targ.FixedParticles( 0x376A, 9, 32, 5008, EffectLayer.Waist );

					int physmod = 15 + (int) ( targ.Skills[SkillName.Inscribe].Value / 20 );

					mods = new ResistanceMod[5]
						{
							new ResistanceMod( ResistanceType.Physical, physmod ), 
							new ResistanceMod( ResistanceType.Fire, -5 ),
							new ResistanceMod( ResistanceType.Cold, -5 ),
							new ResistanceMod( ResistanceType.Poison, -5 ),
							new ResistanceMod( ResistanceType.Energy, -5 )
						};

					string buffFormat = String.Format( "{0}\t{1}\t{1}\t{1}\t{1}", physmod, 5 );

					BuffInfo.AddBuff( targ, new BuffInfo( BuffIcon.ReactiveArmor, 1075812, buffFormat, true ) );

					m_Table[targ] = mods;

					for ( int i = 0; i < mods.Length; ++i )
						targ.AddResistanceMod( mods[i] );
				}
				else
				{
					targ.PlaySound( 0x1ED );
					targ.FixedParticles( 0x376A, 9, 32, 5008, EffectLayer.Waist );

					BuffInfo.RemoveBuff( targ, BuffIcon.ReactiveArmor );

					m_Table.Remove( targ );

					for ( int i = 0; i < mods.Length; ++i )
						targ.RemoveResistanceMod( mods[i] );
				}
			}

			FinishSequence();
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_Table[m] != null;
		}

		public static void RemoveWard( Mobile targ )
		{
			ResistanceMod[] mods = (ResistanceMod[]) m_Table[targ];

			if ( mods != null )
			{
				m_Table.Remove( targ );

				for ( int i = 0; i < mods.Length; ++i )
					targ.RemoveResistanceMod( mods[i] );

				BuffInfo.RemoveBuff( targ, BuffIcon.ReactiveArmor );
			}
		}
	}
}