using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class SolleretsOfSacrifice : BaseShoes, ISetItem
	{
		public override int LabelNumber { get { return 1075194; } } // Sollerets of Sacrifice (Virtue Armor Set)

		[Constructable]
		public SolleretsOfSacrifice()
			: base( 0x2B12 )
		{
			Weight = 1.0;

			Hue = 550;

			LootType = LootType.Blessed;
		}

		public SolleretsOfSacrifice( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( VirtueSet.FullSet( parent as Mobile ) )
				{
					VirtueSet.ApplyBonusSingle( this );
					VirtueSet.ApplyBonus( parent as Mobile );
				}
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
			{
				VirtueSet.RemoveBonus( this );
				VirtueSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			VirtueSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			VirtueSet.GetPropertiesSecond( list, this );
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