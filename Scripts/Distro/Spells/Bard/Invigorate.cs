using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;

namespace Server.Spells.Bard
{
	public class Invigorate : AreaSpellsong
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Invigorate", "An Zu",
				-1,
				9002
			);

		public override BardMastery RequiredMastery { get { return BardMastery.Provocation; } }

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.75 ); } }

		public override int RequiredMana { get { return 22; } }
		public override int UpkeepCost { get { return 5; } }

		public override BuffIcon BuffIcon { get { return BuffIcon.Invigorate; } }

		// You feel invigorated by the bard's spellsong.
		public override int StartEffectMessage { get { return 1115737; } }

		public Invigorate( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		protected override void AddMods( ISet<AttributeMod> mods, ISet<StatMod> statMods )
		{
			int music, peace, provo, disco;
			GetSkillBonus( out music, out peace, out provo, out disco );

			int statBonus = 2 + music + provo + peace + disco;
			statMods.Add( new StatMod( StatType.Str, "Invigorate Str", statBonus ) );
			statMods.Add( new StatMod( StatType.Dex, "Invigorate Dex", statBonus ) );
			statMods.Add( new StatMod( StatType.Int, "Invigorate Int", statBonus ) );

			int hitsBonus = 5 + ( 2 * music ) + ( 3 * provo ) + peace + disco;
			mods.Add( new AttributeMod( MagicalAttribute.BonusHits, hitsBonus ) );

			this.BuffInfo = new BuffInfo( this.BuffIcon, 1115613, 1115730,
				String.Format( "{0}\t{1}\t{1}\t{1}", hitsBonus, statBonus ), false );
		}

		private Timer m_HealTimer;

		protected override void OnSongStarted()
		{
			base.OnSongStarted();

			m_HealTimer = Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 4.0 ), () =>
				{
					int music, peace, provo, disco;
					GetSkillBonus( out music, out peace, out provo, out disco );

					int points = (int) Math.Ceiling( 4 + 1.5 * ( music + provo ) + disco + peace );
					Targets.Each( m => m.Heal( points + Utility.Random( 4 ), Caster ) );
				} );
		}

		protected override void OnSongInterrupted()
		{
			base.OnSongInterrupted();

			if ( m_HealTimer != null )
			{
				m_HealTimer.Stop();
				m_HealTimer = null;
			}
		}

		protected override void OnTargetRemoved( Mobile m )
		{
			base.OnTargetRemoved( m );

			m.Hits = m.Hits;
		}
	}
}
