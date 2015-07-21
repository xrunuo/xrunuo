using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class LayerSearchCriterion : SearchCriterion
	{
		public abstract Layer Layer { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.Layer == Layer;
		}

		public override string ReplacementKey
		{
			get { return typeof( LayerSearchCriterion ).FullName; }
		}
	}

	public class ShoesSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154602; } } // Footwear/Talons
		public override Layer Layer { get { return Layer.Shoes; } }
	}

	public class PantsSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154603; } } // Pants/Leg Armor
		public override Layer Layer { get { return Layer.Pants; } }
	}

	public class ShirtSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154604; } } // Shirt
		public override Layer Layer { get { return Layer.Shirt; } }
	}

	public class HelmSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154605; } } // Hat/Head Armor
		public override Layer Layer { get { return Layer.Helm; } }
	}

	public class GlovesSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154606; } } // Hand/Kilt Armor
		public override Layer Layer { get { return Layer.Gloves; } }
	}

	public class RingSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154607; } } // Ring
		public override Layer Layer { get { return Layer.Ring; } }
	}

	public class TalismanSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154608; } } // Talisman
		public override Layer Layer { get { return Layer.Talisman; } }
	}

	public class NeckSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154609; } } // Necklace/Neck Armor
		public override Layer Layer { get { return Layer.Neck; } }
	}

	public class WaistSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154611; } } // Apron/Belt
		public override Layer Layer { get { return Layer.Waist; } }
	}

	public class InnerTorsoSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154612; } } // Chest Armor
		public override Layer Layer { get { return Layer.InnerTorso; } }
	}

	public class BraceletSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154613; } } // Bracelet
		public override Layer Layer { get { return Layer.Bracelet; } }
	}

	public class MiddleTorsoSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154616; } } // Surcoat/Tunic/Sash
		public override Layer Layer { get { return Layer.MiddleTorso; } }
	}

	public class EarringsSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154617; } } // Earrings/Gargish Glasses
		public override Layer Layer { get { return Layer.Earrings; } }
	}

	public class ArmsSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154618; } } // Arm Armor
		public override Layer Layer { get { return Layer.Arms; } }
	}

	public class CloakSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154619; } } // Cloak/Quiver/Wing Armor
		public override Layer Layer { get { return Layer.Cloak; } }
	}

	public class OuterTorsoSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154621; } } // Dress/Robe
		public override Layer Layer { get { return Layer.OuterTorso; } }
	}

	public class OuterLegsSearchCriterion : LayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154622; } } // Skirt
		public override Layer Layer { get { return Layer.OuterLegs; } }
	}
}
