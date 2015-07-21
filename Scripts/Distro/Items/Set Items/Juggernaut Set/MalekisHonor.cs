using System;
using Server;

namespace Server.Items
{
	public class MalekisHonor : MetalKiteShield, ISetItem
	{
		public override int LabelNumber { get { return 1074312; } } // Maleki's Honor (Juggernaut Set)

		public override int BasePhysicalResistance { get { return 3; } }
		public override int BaseFireResistance { get { return 3; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return 3; } }
		public override int BaseEnergyResistance { get { return 3; } }

		[Constructable]
		public MalekisHonor()
		{
		}

		public MalekisHonor( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( JuggernautSet.FullSet( parent as Mobile ) )
					JuggernautSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 1901 )
			{
				JuggernautSet.RemoveBonus( this );
				JuggernautSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			JuggernautSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			JuggernautSet.GetPropertiesSecond( list, this );
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