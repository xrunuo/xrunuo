using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class MyrmidonGorget : StuddedGorget, ISetItem
	{
		public override int LabelNumber { get { return 1074306; } } // Myrmidon Armor

		public override int BasePhysicalResistance { get { return 7; } }
		public override int BaseFireResistance { get { return 7; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return 5; } }
		public override int BaseEnergyResistance { get { return 3; } }

		[Constructable]
		public MyrmidonGorget()
		{
			Attributes.BonusHits = 2;
			Attributes.BonusStr = 1;
		}

		public MyrmidonGorget( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( MyrmidonSet.FullSet( parent as Mobile ) )
					MyrmidonSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 817 )
			{
				MyrmidonSet.RemoveBonus( this );
				MyrmidonSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			MyrmidonSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			MyrmidonSet.GetPropertiesSecond( list, this );
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