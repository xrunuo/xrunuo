using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class LeafweaveLeggings : HidePants, ISetItem
	{
		public override int LabelNumber { get { return 1074299; } } // Elven Leafweave

		public override int BasePhysicalResistance { get { return 4; } }
		public override int BaseFireResistance { get { return 9; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 8; } }

		[Constructable]
		public LeafweaveLeggings()
		{
			Attributes.RegenMana = 1;
			ArmorAttributes.MageArmor = 1;
		}

		public LeafweaveLeggings( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( LeafweaveSet.FullSet( parent as Mobile ) )
					LeafweaveSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 1150 )
			{
				LeafweaveSet.RemoveBonus( this );
				LeafweaveSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			LeafweaveSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			LeafweaveSet.GetPropertiesSecond( list, this );
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