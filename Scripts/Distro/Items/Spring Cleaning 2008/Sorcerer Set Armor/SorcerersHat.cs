using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class SorcerersHat : WizardsHat, ISetItem
	{
		public override int LabelNumber { get { return 1080465; } } // Sorcerer's Hat

		public override int BasePhysicalResistance { get { return 7; } }
		public override int BaseFireResistance { get { return 7; } }
		public override int BaseColdResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 7; } }
		public override int BaseEnergyResistance { get { return 7; } }

		public override int InitMaxHits { get { return 255; } }
		public override int InitMinHits { get { return 255; } }

		[Constructable]
		public SorcerersHat()
		{
			Hue = 1165;
			Attributes.BonusInt = 1;
			Attributes.LowerRegCost = 10;
		}

		public SorcerersHat( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( SorcerersSet.FullSet( parent as Mobile ) )
					SorcerersSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
				SorcerersSet.RemoveBonus( parent as Mobile );
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			SorcerersSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			SorcerersSet.GetPropertiesSecond( list, this );
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