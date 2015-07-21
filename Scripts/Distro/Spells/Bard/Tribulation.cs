using System;
using Server;
using Server.Items;
using Server.Engines.BuffIcons;

namespace Server.Spells.Bard
{
	public class Tribulation : TargetedSpellsong
	{
		public static void Initialize()
		{
			Mobile.Damaged += new DamagedEventHandler( Mobile_Damaged );
		}

		private static void Mobile_Damaged( Mobile sender, DamagedEventArgs e )
		{
			if ( !sender.Alive )
				return;

			var spellsong = Spellsong.GetEffectSpellsong<Tribulation>( sender );

			if ( spellsong != null )
				spellsong.OnDamage( sender, e.Amount );
		}

		private static SpellInfo m_Info = new SpellInfo(
				"Tribulation", "In Jux Hur Rel",
				-1,
				9002
			);

		public override BardMastery RequiredMastery { get { return BardMastery.Discordance; } }

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.5 ); } }

		public override int RequiredMana { get { return 24; } }
		public override int UpkeepCost { get { return 8; } }

		// You suddenly feel as if everything you do will go awry.
		public override int StartEffectMessage { get { return 1115778; } }

		public Tribulation( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		private int m_DamageChance, m_DamageFactor;
		private AttributeMod[] m_Mods;

		protected override void OnTarget( Mobile target )
		{
			if ( CheckHSequence( target ) )
			{
				int music, peace, provo, disco;
				GetSkillBonus( out music, out peace, out provo, out disco );

				int resist = (int) Math.Max( 0, ( Target.Skills.MagicResist.Value - 80 ) / 10 );

				int propMalus = 5 + ( 5 * disco ) + ( 2 * ( provo + peace ) );
				propMalus -= ( 2 * resist );

				int damageFactor = 8 + ( 8 * disco ) + ( 3 * ( provo + peace ) );
				damageFactor -= ( 4 * resist );

				if ( propMalus <= 0 || damageFactor <= 0 )
				{
					Caster.SendLocalizedMessage( 1115932 ); // Your target resists the effects of your spellsong.
					target.SendLocalizedMessage( 1115933 ); // You resist the effects of the spellsong.
				}
				else
				{
					m_Mods = new AttributeMod[]
					{
						new AttributeMod( MagicalAttribute.AttackChance, -propMalus ),
						new AttributeMod( MagicalAttribute.SpellDamage, -propMalus )
					};

					if ( CheckInstrumentSlays( target ) )
						damageFactor = (int) ( damageFactor * 1.5 );

					m_DamageFactor = damageFactor;

					int damageChance = 15 + ( 15 * music ) + ( 4 * ( provo + peace ) );
					m_DamageChance = damageChance;

					// Tribulation / Target: ~1_val~ <br> Damage Factor: ~2_val~% <br> Damage Chance: ~3_val~%
					Caster.AddBuff( new BuffInfo( BuffIcon.TribulationCaster, 1151387, 1151388,
						String.Format( "{0}\t{1}\t{2}", target.Name, damageFactor, damageChance ) ) );

					// Tribulation / ~1_HCI~% Hit Chance.<br>~2_SDI~% Spell Damage.<br>Damage taken has a ~3_EXP~% chance to cause additional burst of physical damage.<br>
					target.AddBuff( new BuffInfo( BuffIcon.TribulationTarget, 1115740, 1115742,
						String.Format( "-{0}\t-{1}\t{2}", propMalus, propMalus, damageChance ) ) );

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

		private static bool m_Damaging;

		protected void OnDamage( Mobile m, int amount )
		{
			if ( !m_Damaging && m_DamageChance > Utility.Random( 100 ) )
			{
				m_Damaging = true;

				amount = (int) ( amount * m_DamageFactor / 100 );
				m.Damage( amount, Caster );

				m_Damaging = false;
			}
		}

		protected override void OnTargetAdded( Mobile m )
		{
			base.OnTargetAdded( m );

			foreach ( AttributeMod mod in m_Mods )
				m.AddAttributeMod( mod );
		}

		protected override void OnTargetRemoved( Mobile m )
		{
			base.OnTargetRemoved( m );

			foreach ( AttributeMod mod in m_Mods )
				m.RemoveAttributeMod( mod );
		}

		protected override void OnSongInterrupted()
		{
			base.OnSongInterrupted();

			Caster.RemoveBuff( BuffIcon.TribulationCaster );
			Target.RemoveBuff( BuffIcon.TribulationTarget );
		}
	}
}