using System;

namespace Server.Items
{
	public class CreepingVine : Item
	{
		public override int LabelNumber { get { return 1116290; } } // a creeping vine

		public override bool ForceShowProperties { get { return true; } }

		[Constructable]
		public CreepingVine()
			: base( Utility.Random( 0x4792, 4 ) )
		{
			Weight = 10.0;
		}

		public CreepingVine( Serial serial )
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

			/*int version =*/
			reader.ReadInt();
		}
	}
}
