using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class KnightsGloves : PlateGloves, ISetItem
	{
		public override int LabelNumber { get { return 1080161; } } // Knight's Gloves

		public override int BasePhysicalResistance { get { return 7; } }
		public override int BaseFireResistance { get { return 7; } }
		public override int BaseColdResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 7; } }
		public override int BaseEnergyResistance { get { return 7; } }

		public override int InitMaxHits { get { return 255; } }
		public override int InitMinHits { get { return 255; } }

		[Constructable]
		public KnightsGloves()
		{
			Hue = 0x47E;
			Attributes.BonusHits = 1;
		}

		public KnightsGloves( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( KnightsSet.FullSet( parent as Mobile ) )
					KnightsSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
				KnightsSet.RemoveBonus( parent as Mobile );
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			KnightsSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			KnightsSet.GetPropertiesSecond( list, this );
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