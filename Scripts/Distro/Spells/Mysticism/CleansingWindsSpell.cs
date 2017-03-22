using System;
using System.Collections.Generic;
using Server;
using Server.Engines.PartySystem;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;
using Server.Engines.BuffIcons;

namespace Server.Spells.Mysticism
{
	public class CleansingWindsSpell : MysticSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Cleansing Winds", "In Vas Mani Hur",
				-1,
				9002,
				Reagent.DragonsBlood,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }

		public override double RequiredSkill { get { return 58.0; } }
		public override int RequiredMana { get { return 20; } }

		public CleansingWindsSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile targeted )
		{
			if ( !Caster.CanSee( targeted ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckBSequence( targeted ) )
			{
				SpellHelper.Turn( Caster, targeted );

				/* Soothing winds attempt to neutralize poisons, lift curses, and heal a valid
				 * Target. The Caster's Mysticism and either Focus or Imbuing (whichever is
				 * greater) skills determine the effectiveness of the Cleansing Winds.
				 */

				targeted.PlaySound( 0x64C );

				var targets = new List<Mobile> {targeted};

				var map = targeted.Map;

				if ( map != null )
				{
					foreach ( var m in map.GetMobilesInRange( targeted.Location, 2 ) )
					{
						if ( targets.Count >= 3 )
							break;

						if ( targeted != m && IsValidTarget( m ) )
							targets.Add( m );
					}
				}

				var baseToHeal = (int) ( ( GetBaseSkill( Caster ) + GetBoostSkill( Caster ) ) / 4.0 ) + Utility.RandomMinMax( -3, 3 );
				baseToHeal /= targets.Count;

				foreach ( var m in targets )
				{
					Caster.DoBeneficial( m );

					m.FixedParticles( 0x3709, 1, 30, 9963, 13, 3, EffectLayer.Head );

					IEntity from = new DummyEntity( Serial.Zero, new Point3D( m.X, m.Y, m.Z - 10 ), Caster.Map );
					IEntity to = new DummyEntity( Serial.Zero, new Point3D( m.X, m.Y, m.Z + 50 ), Caster.Map );
					Effects.SendMovingParticles( @from, to, 0x2255, 1, 0, false, false, 13, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );

					Poison poison = m.Poison;
					var toHeal = baseToHeal;
					var canHeal = true;

					if ( MortalStrike.IsWounded( m ) )
					{
						// Cleansing Winds will not heal the target after removing mortal wound.
						canHeal = false;
					}

					// Each Curse reduces healing by 3 points + 1% per curse level.
					var cursePower = EnchantedApple.GetCursePower( m );
					toHeal -= cursePower * 3;
					toHeal -= (int) ( toHeal * cursePower * 0.01 );

					// Curse removal no longer based on chance.
					RemoveCurses( m );

					if ( poison != null )
					{
						int chanceBonus = (int) ( ( GetBaseSkill( Caster ) + GetBoostSkill( Caster ) ) * 37.5 );
						int cureChance = 10000 + chanceBonus - ( ( poison.Level + 1 ) * 3500 );

						if ( cureChance > Utility.Random( 10000 ) )
						{
							m.CurePoison( Caster );

							// Poison reduces healing factor by 15% per level of poison.
							toHeal -= (int) ( toHeal * ( poison.Level + 1 ) * 0.15 );
						}
						else
						{
							// If the cure fails, the target will not be healed.
							canHeal = false;
						}
					}

					if ( canHeal )
						m.Heal( toHeal, Caster );
				}
			}

			FinishSequence();
		}

		private bool IsValidTarget( Mobile target )
		{
			var party = Party.Get( Caster );

			return party != null && party.Contains( target );
		}

		private static string[] StatModNames = new string[] {
				"[Magic] Str Malus",
				"[Magic] Dex Malus",
				"[Magic] Int Malus"
			};

		private void RemoveCurses( Mobile m )
		{
			StatMod mod;

			foreach ( var statModName in StatModNames )
			{
				mod = m.GetStatMod( statModName );
				if ( mod != null && mod.Offset < 0 )
					m.RemoveStatMod( statModName );
			}

			m.Paralyzed = false;

			EvilOmenSpell.CheckEffect( m );
			StrangleSpell.RemoveCurse( m );
			CorpseSkinSpell.RemoveCurse( m );
			CurseSpell.RemoveEffect( m );
			MortalStrike.EndWound( m );
			BloodOathSpell.EndEffect( m );
			SpellPlagueSpell.RemoveEffect( m );
			SleepSpell.RemoveEffect( m );
			MindRotSpell.ClearMindRotScalar( m );

			BuffInfo.RemoveBuff( m, BuffIcon.Clumsy );
			BuffInfo.RemoveBuff( m, BuffIcon.FeebleMind );
			BuffInfo.RemoveBuff( m, BuffIcon.Weaken );
			BuffInfo.RemoveBuff( m, BuffIcon.MassCurse );
			BuffInfo.RemoveBuff( m, BuffIcon.Curse );
			BuffInfo.RemoveBuff( m, BuffIcon.EvilOmen );
			BuffInfo.RemoveBuff( m, BuffIcon.MortalStrike );
			BuffInfo.RemoveBuff( m, BuffIcon.Sleep );
			BuffInfo.RemoveBuff( m, BuffIcon.MassSleep );
			BuffInfo.RemoveBuff( m, BuffIcon.Mindrot );
		}

		private class InternalTarget : Target
		{
			private CleansingWindsSpell m_Owner;

			public InternalTarget( CleansingWindsSpell owner )
				: base( 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile) o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
