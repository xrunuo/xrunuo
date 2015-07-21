using System;
using Server;
using Server.Mobiles;

namespace Server.Spells.Spellweaving
{
	public abstract class SpellweavingSpell : Spell
	{
		public abstract double RequiredSkill { get; }
		public abstract int RequiredMana { get; }

		public override SkillName CastSkill { get { return SkillName.Spellweaving; } }
		public override SkillName DamageSkill { get { return SkillName.Spellweaving; } }

		public override bool ClearHandsOnCast { get { return false; } }

		public override int FasterCastingCap { get { return 4; } }

		public SpellweavingSpell( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public static int GetFocusLevel( Mobile from )
		{
			ArcaneFocus focus = FindArcaneFocus( from );

			if ( focus == null || focus.Deleted )
				return 0;

			return focus.StrengthBonus;
		}

		public static ArcaneFocus FindArcaneFocus( Mobile from )
		{
			if ( from == null || from.Backpack == null )
				return null;

			if ( from.Holding is ArcaneFocus )
				return from.Holding as ArcaneFocus;

			return from.Backpack.FindItemByType( typeof( ArcaneFocus ) ) as ArcaneFocus;
		}

		public static bool CheckExpansion( Mobile from )
		{
			return true;
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Caster is PlayerMobile )
			{
				if ( !CheckExpansion( Caster ) )
				{
					Caster.SendLocalizedMessage( 1072176 ); // You must upgrade to the Mondain's Legacy Expansion Pack before using that ability
					return false;
				}

				PlayerMobile pm = Caster as PlayerMobile;

				if ( !pm.Arcanist )
				{
					pm.SendLocalizedMessage( 1073220 ); // You must have completed the epic arcanist quest to use this ability.
					return false;
				}
			}

			int mana = ScaleMana( RequiredMana );

			if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage( 1060174, mana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendLocalizedMessage( 1063013, String.Format( "{0}\t{1}\t ", RequiredSkill.ToString( "F1" ), CastSkill.ToString() ) ); // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.
				return false;
			}

			return true;
		}

		public override void DoFizzle()
		{
			Caster.PlaySound( 0x1D6 );
		}

		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
			{
				Caster.PlaySound( 0x1D6 );
			}
		}

		public override void OnBeginCast()
		{
			SendCastEffect();

			base.OnBeginCast();
		}

		public virtual void SendCastEffect()
		{
			Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill - 12.5;	// per 5 on friday, 2/16/07
			max = RequiredSkill + 37.5;
		}

		public override int GetMana()
		{
			return RequiredMana;
		}

		public virtual bool CheckResisted( Mobile m )
		{
			double percent = ( 50 + 2 * ( GetResistSkill( m ) - GetDamageSkill( m ) ) ) / 100;	//TODO: According to the guide this is it.. but.. is it correct per OSI?

			if ( percent <= 0 )
				return false;

			if ( percent >= 1.0 )
				return true;

			return ( percent >= Utility.RandomDouble() );
		}
	}
}