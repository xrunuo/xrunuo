using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ScoutsStuddedTunic : StuddedChest, ISetItem
	{
		public override int LabelNumber { get { return 1080476; } } // Scout's Studded Tunic

		public override int BasePhysicalResistance { get { return 7; } }
		public override int BaseFireResistance { get { return 7; } }
		public override int BaseColdResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 7; } }
		public override int BaseEnergyResistance { get { return 7; } }

		public override int InitMaxHits { get { return 255; } }
		public override int InitMinHits { get { return 255; } }

		[Constructable]
		public ScoutsStuddedTunic()
		{
			Hue = 0x47C;
			Attributes.BonusDex = 1;
			ArmorAttributes.MageArmor = 1;
		}

		public ScoutsStuddedTunic( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( ScoutsSet.FullSet( parent as Mobile ) )
					ScoutsSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
				ScoutsSet.RemoveBonus( parent as Mobile );
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			ScoutsSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			ScoutsSet.GetPropertiesSecond( list, this );
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