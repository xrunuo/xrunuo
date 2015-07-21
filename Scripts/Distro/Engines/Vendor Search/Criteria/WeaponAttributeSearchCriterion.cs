using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.VendorSearch
{
	public abstract class WeaponAttributeSearchCriterion : ValuedSearchCriterion
	{
		public abstract WeaponAttribute Attribute { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IWeapon && ( (IWeapon) item.Item ).WeaponAttributes[Attribute] >= Value;
		}
	}

	public abstract class BooleanWeaponAttributeSearchCriterion : SearchCriterion
	{
		public abstract WeaponAttribute Attribute { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IWeapon && ( (IWeapon) item.Item ).WeaponAttributes[Attribute] != 0;
		}
	}

	public class UseBestWeaponSkillSearchCriterion : BooleanWeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1079592; } } // Use Best Weapon Skill
		public override WeaponAttribute Attribute { get { return WeaponAttribute.UseBestSkill; } }
	}

	public class BloodDrinkerSearchCriterion : BooleanWeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113591; } } // Blood Drinker
		public override WeaponAttribute Attribute { get { return WeaponAttribute.BloodDrinker; } }
	}

	public class BattleLustSearchCriterion : BooleanWeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113710; } } // Battle Lust
		public override WeaponAttribute Attribute { get { return WeaponAttribute.BattleLust; } }
	}

	public class BalancedSearchCriterion : BooleanWeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1072792; } } // Balanced
		public override WeaponAttribute Attribute { get { return WeaponAttribute.Balanced; } }
	}

	public class MageWeaponSearchCriterion : ValuedSearchCriterion
	{
		public override int LabelNumber { get { return 1060438; } } // mage weapon -~1_val~ skill

		public override bool Matches( IVendorSearchItem item )
		{
			var weapon = item.Item as IWeapon;
			return weapon != null && weapon.WeaponAttributes.MageWeapon == ( 30 - Value );
		}
	}

	public abstract class ElementalDamageSearchCriterion : ValuedSearchCriterion
	{
		public abstract ElementAttribute Element { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			var weapon = item.Item as BaseWeapon;

			if ( weapon != null )
				return ( GetElementAttributeValue( weapon, Element ) >= Value );
			else
				return false;
		}

		private static int GetElementAttributeValue( BaseWeapon weapon, ElementAttribute element )
		{
			int phys, fire, cold, pois, nrgy, chao;
			weapon.GetDamageTypes( null, out phys, out fire, out cold, out pois, out nrgy, out chao );

			switch ( element )
			{
				case ElementAttribute.Physical:
					return phys;
				case ElementAttribute.Fire:
					return fire;
				case ElementAttribute.Cold:
					return cold;
				case ElementAttribute.Poison:
					return pois;
				case ElementAttribute.Energy:
					return nrgy;
			}

			return -1;
		}
	}

	public class PhysicalDamageSearchCriterion : ElementalDamageSearchCriterion
	{
		public override int LabelNumber { get { return 1060403; } } // physical damage ~1_val~%
		public override ElementAttribute Element { get { return ElementAttribute.Physical; } }
	}

	public class FireDamageSearchCriterion : ElementalDamageSearchCriterion
	{
		public override int LabelNumber { get { return 1060405; } } // fire damage ~1_val~%
		public override ElementAttribute Element { get { return ElementAttribute.Fire; } }
	}

	public class ColdDamageSearchCriterion : ElementalDamageSearchCriterion
	{
		public override int LabelNumber { get { return 1060404; } } // cold damage ~1_val~%
		public override ElementAttribute Element { get { return ElementAttribute.Cold; } }
	}

	public class PoisonDamageSearchCriterion : ElementalDamageSearchCriterion
	{
		public override int LabelNumber { get { return 1060406; } } // poison damage ~1_val~%
		public override ElementAttribute Element { get { return ElementAttribute.Poison; } }
	}

	public class EnergyDamageSearchCriterion : ElementalDamageSearchCriterion
	{
		public override int LabelNumber { get { return 1060407; } } // energy damage ~1_val~%
		public override ElementAttribute Element { get { return ElementAttribute.Energy; } }
	}

	public class HitDispelSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060417; } } // hit dispel ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitDispel; } }
	}

	public class HitFireballSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060420; } } // hit fireball ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitFireball; } }
	}

	public class HitHarmSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060421; } } // hit harm ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitHarm; } }
	}

	public class HitCurseSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113712; } } // Hit Curse ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitCurse; } }
	}

	public class HitLifeLeechSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060422; } } // hit life leech ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLeechHits; } }
	}

	public class HitLightningSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060423; } } // hit lightning ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLightning; } }
	}

	public class VelocitySearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1072793; } } // Velocity ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.Velocity; } }
	}






	public class HitLowerAttackSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060424; } } // hit lower attack ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLowerAttack; } }
	}

	public class HitLowerDefenseSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060425; } } // hit lower defense ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLowerDefend; } }
	}

	public class HitMagicArrowSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060426; } } // hit magic arrow ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitMagicArrow; } }
	}

	public class HitManaLeechSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060427; } } // hit mana leech ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLeechMana; } }
	}

	public class HitStaminaLeechSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060422; } } // hit life leech ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLeechHits; } }
	}

	public class HitFatigueSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113700; } } // Hit Fatigue ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitFatigue; } }
	}

	public class HitManaDrainSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113699; } } // Hit Mana Drain ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitManaDrain; } }
	}

	public class SplinteringWeaponSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1112857; } } // splintering weapon ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.SplinteringWeapon; } }
	}

	public class HitPhysicalAreaSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060428; } } // hit physical area ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitPhysicalArea; } }
	}

	public class HitFireAreaSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060419; } } // hit fire area ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitFireArea; } }
	}

	public class HitColdAreaSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060416; } } // hit cold area ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitColdArea; } }
	}

	public class HitPoisonAreaSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060429; } } // hit poison area ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitPoisonArea; } }
	}

	public class HitEnergyAreaSearchCriterion : WeaponAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060418; } } // hit energy area ~1_val~%
		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitEnergyArea; } }
	}
}
