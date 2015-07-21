using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class AncientWildStaff : WildStaff
	{
		public override int LabelNumber { get { return 1073550; } } // Ancient Wild Staff 

		[Constructable]
		public AncientWildStaff()
		{
			Resistances.Poison = 5;
		}

		public AncientWildStaff( Serial serial )
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