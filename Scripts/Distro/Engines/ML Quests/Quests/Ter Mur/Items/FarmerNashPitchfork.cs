using System;
using Server;

namespace Server.Items
{
	public class FarmerNashPitchfork : Pitchfork
	{
		public override int LabelNumber { get { return 1113498; } } // Farmer Nash's Pitchfork

		[Constructable]
		public FarmerNashPitchfork()
		{
		}

		public FarmerNashPitchfork( Serial serial )
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
