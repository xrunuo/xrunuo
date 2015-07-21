using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Spells.Bushido;
using Server.Items;

namespace Server.Spells.Ninjitsu
{
	public abstract class NinjaSpell : Spell
	{
		public abstract double RequiredSkill { get; }
		public abstract int RequiredMana { get; }

		public override SkillName CastSkill { get { return SkillName.Ninjitsu; } }

		public override bool RevealOnCast { get { return false; } }
		public override bool ClearHandsOnCast { get { return false; } }
		public override bool ShowHandMovement { get { return false; } }

		public override bool BlocksMovement { get { return false; } }

		public override int FasterCastingCap { get { return 4; } }
		public override int CastRecoveryBase { get { return 7; } }

		public NinjaSpell( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Caster.Skills[SkillName.Ninjitsu].Value < RequiredSkill )
			{
				string args = String.Format( "{0}\t{1}\t ", RequiredSkill.ToString( "F1" ), CastSkill.ToString() );
				Caster.SendLocalizedMessage( 1063013, args ); // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.
				return false;
			}
			else if ( Caster.Mana < ScaleMana( RequiredMana ) )
			{
				Caster.SendLocalizedMessage( 1060174, RequiredMana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			return true;
		}

		public override bool CheckFizzle()
		{
			int mana = ScaleMana( RequiredMana );

			if ( Caster.Skills[SkillName.Ninjitsu].Value < RequiredSkill )
			{
				Caster.SendLocalizedMessage( 1063352, RequiredSkill.ToString( "F1" ) ); // You need ~1_SKILL_REQUIREMENT~ Ninjitsu skill to perform that attack!
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage( 1060174, RequiredMana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			if ( !base.CheckFizzle() )
				return false;

			Caster.Mana -= mana;

			return true;
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 0x1D6 );
		}

		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
				Caster.PlaySound( 0x1D6 );
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			if ( this is AnimalForm )
			{
				SendCastEffect();
			}
		}

		public virtual void SendCastEffect()
		{
			Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill - 12.5;	// Per 5 on friday 2/16/07
			max = RequiredSkill + 37.5;
		}

		public override int GetMana()
		{
			return 0;
		}
	}
}