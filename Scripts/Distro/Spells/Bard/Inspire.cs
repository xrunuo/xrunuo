using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;
using Server.Events;

namespace Server.Spells.Bard
{
	public class Inspire : AreaSpellsong
	{
		public static void Initialize()
		{
			EventSink.BeforeDamage += new BeforeDamageEventHandler( EventSink_BeforeDamage );
		}

		private static void EventSink_BeforeDamage( BeforeDamageEventArgs e )
		{
			if ( e.From == null )
				return;

			Inspire spellsong = Spellsong.GetEffectSpellsong<Inspire>( e.From );

			if ( spellsong != null )
				e.Amount = (int) ( e.Amount * ( 100 + spellsong.DamageModifier ) / 100 );
		}

		private static SpellInfo m_Info = new SpellInfo(
				"Inspire", "Uus Por",
				-1,
				9002
			);

		public override BardMastery RequiredMastery { get { return BardMastery.Provocation; } }

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.25 ); } }

		public override int RequiredMana { get { return 16; } }
		public override int UpkeepCost { get { return 5; } }

		public override BuffIcon BuffIcon { get { return BuffIcon.Inspire; } }

		// You feel inspired by the bard's spellsong.
		public override int StartEffectMessage { get { return 1115736; } }

		public Inspire( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public int DamageModifier { get; private set; }

		protected override void AddMods( ISet<AttributeMod> mods, ISet<StatMod> statMods )
		{
			int music, peace, provo, disco;
			GetSkillBonus( out music, out peace, out provo, out disco );

			int weaponDamage = 10 + 5 * ( music + provo ) + 3 * ( peace + disco );
			mods.Add( new AttributeMod( AosAttribute.WeaponDamage, weaponDamage ) );

			int spellDamage = 4 + 2 * ( music + provo ) + peace + disco;
			mods.Add( new AttributeMod( AosAttribute.SpellDamage, spellDamage ) );

			int attackChance = 4 + 2 * ( music + provo ) + peace + disco;
			mods.Add( new AttributeMod( AosAttribute.AttackChance, attackChance ) );

			int damageModifier = Math.Max( 1, music * provo + Math.Max( music, provo ) + ( disco + peace ) / 2 );
			this.DamageModifier = damageModifier;

			this.BuffInfo = new BuffInfo( this.BuffIcon, 1115612, 1151951,
				String.Format( "{0}\t{1}\t{2}\t{3}", weaponDamage, spellDamage, attackChance, DamageModifier ), false );
		}
	}
}
