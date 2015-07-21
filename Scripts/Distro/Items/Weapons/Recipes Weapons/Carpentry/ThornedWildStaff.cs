using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class ThornedWildStaff : WildStaff
	{
		public override int LabelNumber { get { return 1073551; } } // Thorned Wild Staff
		[Constructable]
		public ThornedWildStaff()
		{
			Attributes.ReflectPhysical = 12;
		}

		public ThornedWildStaff( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}