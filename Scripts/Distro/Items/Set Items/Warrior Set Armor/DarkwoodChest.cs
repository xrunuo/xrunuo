using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class DarkwoodChest : WoodlandChest, ISetItem
	{
		public override int LabelNumber { get { return 1073482; } } // Darkwood Chest

		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 5; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 7; } }
		public override int BaseEnergyResistance { get { return 5; } }

		[Constructable]
		public DarkwoodChest()
		{
			Hue = 1109;
			Attributes.BonusHits = 2;
			Attributes.DefendChance = 5;
		}

		public DarkwoodChest( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( DarkwoodSet.FullSet( parent as Mobile ) )
					DarkwoodSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 1172 )
			{
				DarkwoodSet.RemoveBonus( this );
				DarkwoodSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			DarkwoodSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			DarkwoodSet.GetPropertiesSecond( list, this );
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