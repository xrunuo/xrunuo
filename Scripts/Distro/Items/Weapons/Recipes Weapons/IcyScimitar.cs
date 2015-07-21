using System;
using Server;

namespace Server.Items
{
	public class IcyScimitar : RadiantScimitar
	{
		public override int LabelNumber { get { return 1073543; } } // Icy Scimitar
		[Constructable]
		public IcyScimitar()
		{
			ItemID = 11559;
			WeaponAttributes.HitHarm = 15;
		}


		public IcyScimitar( Serial serial )
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