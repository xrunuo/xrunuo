using System;
using Server;

namespace Server.Items
{
	public class Feathernock : BaseQuiver, ISetItem
	{
		public override int LabelNumber { get { return 1074324; } } // Feathernock (Marksman Set)

		public override int WeightReduction { get { return 30; } }

		[Constructable]
		public Feathernock()
		{
		}

		public Feathernock( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( MarksmanSet.FullSet( parent as Mobile ) )
				{
					MarksmanSet.ApplyBonus( parent as Mobile );
					InvalidateProperties();
				}
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 1428 )
			{
				MarksmanSet.RemoveBonus( this );
				MarksmanSet.RemoveBonus( parent as Mobile );

				InvalidateProperties();
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			MarksmanSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			MarksmanSet.GetPropertiesSecond( list, this );
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