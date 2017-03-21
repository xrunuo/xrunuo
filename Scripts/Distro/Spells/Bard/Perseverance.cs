using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;
using Server.Events;

namespace Server.Spells.Bard
{
	public class Perseverance : AreaSpellsong
	{
		public static void Initialize()
		{
			EventSink.BeforeDamage += new BeforeDamageEventHandler( EventSink_BeforeDamage );
		}

		private static void EventSink_BeforeDamage( BeforeDamageEventArgs e )
		{
			Perseverance spellsong = Spellsong.GetEffectSpellsong<Perseverance>( e.Mobile );

			if ( spellsong != null )
				e.Amount = (int) ( e.Amount * ( 100 - spellsong.DamageTaken ) / 100 );
		}

		private static SpellInfo m_Info = new SpellInfo(
				"Perseverance", "Uus Jux Sanct",
				-1,
				9002
			);

		public override BardMastery RequiredMastery { get { return BardMastery.Peacemaking; } }

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }

		public override int RequiredMana { get { return 18; } }
		public override int UpkeepCost { get { return 6; } }

		public override BuffIcon BuffIcon { get { return BuffIcon.Perseverance; } }

		// The bard's spellsong fills you with a feeling of invincibility.
		public override int StartEffectMessage { get { return 1115739; } }

		public Perseverance( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public int DamageTaken { get; private set; }

		protected override void AddMods( ISet<AttributeMod> mods, ISet<StatMod> statMods )
		{
			int music, peace, provo, disco;
			GetSkillBonus( out music, out peace, out provo, out disco );

			int defenseChance = 1 + GetMusicBonus( music ) + GetPeaceBonus( peace ) + provo + disco;
			mods.Add( new AttributeMod( MagicalAttribute.DefendChance, defenseChance ) );

			int castingFocus = 1 + ( music + peace ) / 2 + ( provo + disco ) / 3;
			mods.Add( new AttributeMod( MagicalAttribute.CastingFocus, castingFocus ) );

			int damageTaken = 1 + GetMusicBonus( music ) + GetPeaceBonus( peace ) + provo + disco;
			this.DamageTaken = damageTaken;

			this.BuffInfo = new BuffInfo( this.BuffIcon, 1115615, 1115732,
				String.Format( "{0}\t-{1}\t{2}", defenseChance, damageTaken, castingFocus ), false );
		}

		private static int GetMusicBonus( int music )
		{
			if ( music > 0 )
				return ( music * music ) + 1;

			return 0;
		}

		private static int GetPeaceBonus( int peace )
		{
			if ( peace > 0 )
				return peace * ( peace + 1 ) + 1;

			return 0;
		}
	}
}