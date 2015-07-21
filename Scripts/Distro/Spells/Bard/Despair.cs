using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Bard
{
	public class Despair : TargetedSpellsong
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Despair", "Kal Des Mani Tym",
				-1,
				9002
			);

		public override BardMastery RequiredMastery { get { return BardMastery.Discordance; } }

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.5 ); } }

		public override int RequiredMana { get { return 26; } }
		public override int UpkeepCost { get { return 10; } }

		// A wave of despair overcomes you.
		public override int StartEffectMessage { get { return 1115777; } }

		public Despair( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		private int m_Damage;
		private StatMod[] m_StatMods;

		protected override void OnTarget( Mobile target )
		{
			if ( CheckHSequence( target ) )
			{
				int music, peace, provo, disco;
				GetSkillBonus( out music, out peace, out provo, out disco );

				int resist = (int) Math.Max( 0, ( Target.Skills.MagicResist.Value - 80 ) / 10 );

				int strMalus = 4 + ( 2 * ( disco + music ) + provo + peace );
				strMalus -= ( 3 * resist );

				if ( strMalus <= 0 )
				{
					Caster.SendLocalizedMessage( 1115932 ); // Your target resists the effects of your spellsong.
					target.SendLocalizedMessage( 1115933 ); // You resist the effects of the spellsong.
				}
				else
				{
					m_StatMods = new StatMod[]
					{
						new StatMod( StatType.Str, "Despair Str", -strMalus )
					};

					m_Damage = 9 + ( 5 * disco ) + ( 4 * music ) + ( 2 * ( provo + peace ) ) - resist;

					if ( !target.IsPlayer )
					{
						if ( CheckInstrumentSlays( target ) )
							m_Damage = (int) ( m_Damage * 3 );
						else
							m_Damage = (int) ( m_Damage * 1.5 );
					}

					// Despair / Target: ~1_val~ <br>Damage: ~2_val~
					Caster.AddBuff( new BuffInfo( BuffIcon.DespairCaster, 1151389, 1151390,
						String.Format( "{0}\t{1}", target.Name, m_Damage ) ) );

					// Despair / ~1_STR~ Strength.<br>~2_DAM~ physical damage every 2 seconds while spellsong remains in effect.<br>
					target.AddBuff( new BuffInfo( BuffIcon.DespairTarget, 1115741, 1115743,
						String.Format( "-{0}\t{1}", strMalus, m_Damage ) ) );

					StartSong();
				}
			}

			FinishSequence();
		}

		private bool CheckInstrumentSlays( Mobile target )
		{
			BaseInstrument instrument = BaseInstrument.GetInstrument( Caster );

			return instrument != null && instrument.Slays( target );
		}

		protected override void OnUpkeep()
		{
			var element = GetElementDamageType( Caster );

			int phys, fire, cold, pois, nrgy;
			GetDamageTypesFromElement( element, out phys, out fire, out cold, out pois, out nrgy );

			AOS.Damage( Target, Caster, m_Damage, phys, fire, cold, pois, nrgy );

			base.OnUpkeep();
		}

		private static ResistanceType GetElementDamageType( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return default( ResistanceType );

			return pm.BardElementDamage;
		}

		private static void GetDamageTypesFromElement( ResistanceType element, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = fire = cold = pois = nrgy = 0;

			switch ( element )
			{
				case ResistanceType.Physical:
					phys = 100;
					break;
				case ResistanceType.Fire:
					fire = 100;
					break;
				case ResistanceType.Cold:
					cold = 100;
					break;
				case ResistanceType.Poison:
					pois = 100;
					break;
				case ResistanceType.Energy:
					nrgy = 100;
					break;
			}
		}

		protected override void OnTargetAdded( Mobile m )
		{
			base.OnTargetAdded( m );

			foreach ( StatMod mod in m_StatMods )
				m.AddStatMod( mod );
		}

		protected override void OnTargetRemoved( Mobile m )
		{
			base.OnTargetRemoved( m );

			foreach ( StatMod mod in m_StatMods )
				m.RemoveStatMod( mod.Name );
		}

		protected override void OnSongInterrupted()
		{
			base.OnSongInterrupted();

			Caster.RemoveBuff( BuffIcon.DespairCaster );
			Target.RemoveBuff( BuffIcon.DespairTarget );
		}
	}
}