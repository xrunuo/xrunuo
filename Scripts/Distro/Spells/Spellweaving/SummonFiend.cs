using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Spells.Spellweaving
{
	public class SummonFiendSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Summon Fiend", "Nylisstra",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }

		public override double RequiredSkill { get { return 38.0; } }
		public override int RequiredMana { get { return 10; } }

		public SummonFiendSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override int GetMana()
		{
			return 0; // Mana is consumed OnCast()
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Caster is PlayerMobile && !( (PlayerMobile) Caster ).CanSummonFiend )
			{
				Caster.SendLocalizedMessage( 1074564 ); // You haven't demonstrated mastery to summon a fiend.
				return false;
			}

			if ( Caster.Followers >= Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}

			int mana = ScaleMana( RequiredMana );

			if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage( 1060174, mana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				int focuslevel = GetFocusLevel( Caster );

				int amount = focuslevel + 1;

				if ( amount > ( Caster.FollowersMax - Caster.Followers ) )
					amount = Caster.FollowersMax - Caster.Followers;

				int mana = ScaleMana( RequiredMana );

				for ( int i = 0; i < amount && Caster.Mana >= mana; i++ )
				{
					SpellHelper.Summon( new LesserFiend(), Caster, 0x216, TimeSpan.FromMinutes( Math.Max( 1.0, ( Caster.Skills[SkillName.Spellweaving].Fixed / 240 ) + focuslevel ) ), false, false );
					Caster.Mana -= mana;
				}
			}

			FinishSequence();
		}
	}
}
