using System;
using Server;

namespace Server.Spells.Necromancy
{
	public abstract class NecromancerSpell : Spell
	{
		public abstract double RequiredSkill { get; }
		public abstract int RequiredMana { get; }

		public override SkillName CastSkill { get { return SkillName.Necromancy; } }
		public override SkillName DamageSkill { get { return SkillName.SpiritSpeak; } }

		public override bool ClearHandsOnCast { get { return false; } }

		public NecromancerSpell( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public override int ComputeKarmaAward()
		{
			// TODO: Verify this formula being that Necro spells don't HAVE a circle.

			return -( 40 + (int) ( 10 * ( CastDelayBase.TotalSeconds / CastDelaySecondsPerTick ) ) );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill;
			max = RequiredSkill + 40.0;
		}

		public override int GetMana()
		{
			return RequiredMana;
		}
	}
}