using System;
using Server;

namespace Server.Items
{
	public class DarkglowScimitar : RadiantScimitar
	{
		public override int LabelNumber { get { return 1073542; } } // Darkglow Scimitar
		[Constructable]
		public DarkglowScimitar()
		{
			ItemID = 11559;
			WeaponAttributes.HitDispel = 10;
		}


		public DarkglowScimitar( Serial serial )
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