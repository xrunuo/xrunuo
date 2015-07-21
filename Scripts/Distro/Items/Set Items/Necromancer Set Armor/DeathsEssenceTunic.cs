using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class DeathsEssenceTunic : LeatherChest, ISetItem
	{
		public override int LabelNumber { get { return 1074305; } } // Death's Essence

		public override int BasePhysicalResistance { get { return 4; } }
		public override int BaseFireResistance { get { return 9; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 8; } }

		[Constructable]
		public DeathsEssenceTunic()
		{
			Attributes.RegenMana = 1;
			Attributes.RegenHits = 1;
		}

		public DeathsEssenceTunic( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( NecromancerSet.FullSet( parent as Mobile ) )
					NecromancerSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 1109 )
			{
				NecromancerSet.RemoveBonus( this );
				NecromancerSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			NecromancerSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			NecromancerSet.GetPropertiesSecond( list, this );
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