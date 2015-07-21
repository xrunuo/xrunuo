using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class PlateOfHonorLegs : PlateLegs, ISetItem
	{
		public override int LabelNumber { get { return 1074303; } } // Plate of Honor

		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 5; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 7; } }
		public override int BaseEnergyResistance { get { return 5; } }

		[Constructable]
		public PlateOfHonorLegs()
		{
			Attributes.RegenHits = 1;
			Attributes.AttackChance = 5;
		}

		public PlateOfHonorLegs( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( PaladinSet.FullSet( parent as Mobile ) )
					PaladinSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 1150 )
			{
				PaladinSet.RemoveBonus( this );
				PaladinSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			PaladinSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			PaladinSet.GetPropertiesSecond( list, this );
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