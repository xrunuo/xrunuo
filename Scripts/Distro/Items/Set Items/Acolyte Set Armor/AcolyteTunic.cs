using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AcolyteTunic : LeatherChest, ISetItem
	{
		public override int LabelNumber { get { return 1074307; } } // Greymist Armor

		public override int BasePhysicalResistance { get { return 7; } }
		public override int BaseFireResistance { get { return 7; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return 4; } }
		public override int BaseEnergyResistance { get { return 4; } }

		[Constructable]
		public AcolyteTunic()
		{
			Attributes.BonusMana = 2;
			Attributes.SpellDamage = 2;
		}

		public AcolyteTunic( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( AcolyteSet.FullSet( parent as Mobile ) )
					AcolyteSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 203 )
			{
				AcolyteSet.RemoveBonus( this );

				AcolyteSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			AcolyteSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			AcolyteSet.GetPropertiesSecond( list, this );
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