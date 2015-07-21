using System;
using Server;


namespace Server.Items
{

	public class GiantReplicaAcorn : Item
	{

		[Constructable]
		public GiantReplicaAcorn()
			: base( 0x2D4A )
		{
			Weight = 0.0;
		}

		public GiantReplicaAcorn( Serial serial )
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

