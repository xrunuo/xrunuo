using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class HunterLeggings : LeafLeggings, ISetItem
	{
		public override int LabelNumber { get { return 1074301; } } // Hunter's Garb

		public override int BasePhysicalResistance { get { return 9; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return 8; } }
		public override int BaseEnergyResistance { get { return 4; } }

		[Constructable]
		public HunterLeggings()
		{
			Attributes.RegenHits = 1;
			Attributes.Luck = 50;
		}

		public HunterLeggings( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( HunterSet.FullSet( parent as Mobile ) )
					HunterSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
			{
				HunterSet.RemoveBonus( this );
				HunterSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			HunterSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			HunterSet.GetPropertiesSecond( list, this );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}