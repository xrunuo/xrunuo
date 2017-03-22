using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;

namespace Server.Spells.Bard
{
	public class Resilience : AreaSpellsong
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Resilience", "Kal Many Tym",
				-1,
				9002
			);

		public override BardMastery RequiredMastery { get { return BardMastery.Peacemaking; } }

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.5 ); } }

		public override int RequiredMana { get { return 16; } }
		public override int UpkeepCost { get { return 5; } }

		public override BuffIcon BuffIcon { get { return BuffIcon.Resilience; } }

		// The bard's spellsong fills you with a feeling of resilience.
		public override int StartEffectMessage { get { return 1115738; } }

		public Resilience( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public int CurseReduction { get; private set; }

		protected override void AddMods( ISet<AttributeMod> mods, ISet<StatMod> statMods )
		{
			int music, peace, provo, disco;
			GetSkillBonus( out music, out peace, out provo, out disco );

			int regenBonus = ( 1 + music ) * ( 1 + peace ) + provo + disco;
			mods.Add( new AttributeMod( AosAttribute.RegenHits, regenBonus ) );
			mods.Add( new AttributeMod( AosAttribute.RegenStam, regenBonus ) );
			mods.Add( new AttributeMod( AosAttribute.RegenMana, regenBonus ) );

			int curseReduction = ( Math.Min( 1 + music, 1 + peace ) + ( disco / 3 ) + ( peace / 3 ) ) * 10;
			CurseReduction = curseReduction;

			// Resilience / +~1_HPR~ Hit Point Regeneration.<br>+~2_SR~ Stamina Regeneration.<br>+~3_MR~ Mana Regeneration.<br>Curse Durations Reduced.<br>Resistance to Poison.<br>Bleed Duration Reduced.<br>Mortal Wound Duration Reduced.
			this.BuffInfo = new BuffInfo( this.BuffIcon, 1115614, 1115731,
				String.Format( "{0}\t{0}\t{0}", regenBonus ), false );
		}

		// TODO: Poison, Bleed, Curse & Mortal Wound resistance.
	}
}