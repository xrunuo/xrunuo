using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class SkillBonusSearchCriterion : ValuedSearchCriterion
	{
		public abstract int SkillCliloc { get; }
		public abstract SkillName Skill { get; }

		public override int LabelNumber { get { return 1060451; } } // ~1_skillname~ +~2_val~

		public override string LabelArgs
		{
			get { return String.Format( "#{0}\t{1}", SkillCliloc.ToString(), Value.ToString() ); }
		}

		public override bool Matches( IVendorSearchItem item )
		{
			var bonuses = item as ISkillBonuses;

			if ( bonuses != null )
			{
				for ( int i = 0; i < 5; ++i )
				{
					SkillName skill;
					double bonus;

					if ( !bonuses.SkillBonuses.GetValues( i, out skill, out bonus ) )
						continue;

					if ( skill == Skill && ( (int) bonus ) >= Value )
						return true;
				}
			}

			return false;
		}
	}

	public class SwordsBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002151; } } // Swordsmanship
		public override SkillName Skill { get { return SkillName.Swords; } }
	}

	public class FencingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044102; } } // Fencing
		public override SkillName Skill { get { return SkillName.Fencing; } }
	}

	public class MacingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044101; } } // Mace Fighting
		public override SkillName Skill { get { return SkillName.Macing; } }
	}

	public class MageryBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002106; } } // Magery
		public override SkillName Skill { get { return SkillName.Magery; } }
	}

	public class MusicianshipBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002116; } } // Musicianship
		public override SkillName Skill { get { return SkillName.Musicianship; } }
	}

	public class WrestlingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002169; } } // Wrestling
		public override SkillName Skill { get { return SkillName.Wrestling; } }
	}

	public class TacticsBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1017321; } } // Tactics
		public override SkillName Skill { get { return SkillName.Tactics; } }
	}

	public class AnimalTamingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044095; } } // Animal Taming
		public override SkillName Skill { get { return SkillName.AnimalTaming; } }
	}

	public class ProvocationBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002125; } } // Provocation
		public override SkillName Skill { get { return SkillName.Provocation; } }
	}

	public class SpiritSpeakBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002140; } } // Spirit Speak
		public override SkillName Skill { get { return SkillName.SpiritSpeak; } }
	}

	public class StealthBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044107; } } // Stealth
		public override SkillName Skill { get { return SkillName.Stealth; } }
	}

	public class ParryBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002118; } } // Parrying
		public override SkillName Skill { get { return SkillName.Parry; } }
	}

	public class MeditationBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044106; } } // Meditation
		public override SkillName Skill { get { return SkillName.Meditation; } }
	}

	public class AnimalLoreBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002007; } } // Animal Lore
		public override SkillName Skill { get { return SkillName.AnimalLore; } }
	}

	public class DiscordanceBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044075; } } // Discordance
		public override SkillName Skill { get { return SkillName.Discordance; } }
	}

	public class FocusBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044110; } } // Focus
		public override SkillName Skill { get { return SkillName.Focus; } }
	}

	public class StealingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002142; } } // Stealing
		public override SkillName Skill { get { return SkillName.Stealing; } }
	}

	public class AnatomyBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002004; } } // Anatomy
		public override SkillName Skill { get { return SkillName.Anatomy; } }
	}

	public class EvalIntBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044076; } } // Eval Intelligence
		public override SkillName Skill { get { return SkillName.EvalInt; } }
	}

	public class VeterinaryBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044099; } } // Veterinary
		public override SkillName Skill { get { return SkillName.Veterinary; } }
	}

	public class NecromancyBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044109; } } // Necromancy
		public override SkillName Skill { get { return SkillName.Necromancy; } }
	}

	public class BushidoBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044112; } } // Bushido
		public override SkillName Skill { get { return SkillName.Bushido; } }
	}

	public class MysticismBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044115; } } // Mysticism
		public override SkillName Skill { get { return SkillName.Mysticism; } }
	}

	public class HealingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002082; } } // Healing
		public override SkillName Skill { get { return SkillName.Healing; } }
	}

	public class MagicResistBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044086; } } // Resisting Spells
		public override SkillName Skill { get { return SkillName.MagicResist; } }
	}

	public class PeacemakingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002120; } } // Peacemaking
		public override SkillName Skill { get { return SkillName.Peacemaking; } }
	}

	public class ArcheryBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1002029; } } // Archery
		public override SkillName Skill { get { return SkillName.Archery; } }
	}

	public class ChivalryBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044111; } } // Chivalry
		public override SkillName Skill { get { return SkillName.Chivalry; } }
	}

	public class NinjitsuBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044113; } } // Ninjitsu
		public override SkillName Skill { get { return SkillName.Ninjitsu; } }
	}

	public class ThrowingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044117; } } // Throwing
		public override SkillName Skill { get { return SkillName.Throwing; } }
	}

	public class LumberjackingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044104; } } // Lumberjacking
		public override SkillName Skill { get { return SkillName.Lumberjacking; } }
	}

	public class SnoopingBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044088; } } // Snooping
		public override SkillName Skill { get { return SkillName.Snooping; } }
	}

	public class MiningBonusSearchCriteria : SkillBonusSearchCriterion
	{
		public override int SkillCliloc { get { return 1044105; } } // Mining
		public override SkillName Skill { get { return SkillName.Mining; } }
	}
}
