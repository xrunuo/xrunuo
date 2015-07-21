using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class GauntletsOfValor : BaseArmor, ISetItem
	{
		public override int LabelNumber { get { return 1075238; } } // Gauntlets of Valor (Virtue Armor Set)

		public override int BasePhysicalResistance { get { return 6; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 8; } }
		public override int BasePoisonResistance { get { return 9; } }
		public override int BaseEnergyResistance { get { return 6; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int StrengthReq { get { return 50; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		[Constructable]
		public GauntletsOfValor()
			: base( 0x2B0C )
		{
			Weight = 4.0;

			Hue = 550;

			LootType = LootType.Blessed;
			Layer = Layer.Gloves;
		}

		public GauntletsOfValor( Serial serial )
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