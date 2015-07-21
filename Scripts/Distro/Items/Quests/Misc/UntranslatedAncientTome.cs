using System;
using Server;

namespace Server.Items
{
	public class UntranslatedAncientTome : Item
	{
		public override int LabelNumber { get { return 1112992; } } // Untranslated Ancient Tome

		[Constructable]
		public UntranslatedAncientTome()
			: base( 0xFF2 )
		{
			Weight = 1.0;
			Hue = 1109;
		}

		public UntranslatedAncientTome( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
