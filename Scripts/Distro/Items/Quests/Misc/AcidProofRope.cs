using System;
using Server;
using Server.Engines.Quests;

namespace Server.Items
{
	public class AcidProofRope : TransientItem
	{
		public override int LabelNumber { get { return 1074886; } } // Acid Proof Rope

		[Constructable]
		public AcidProofRope()
			: base( 0x20D )
		{
			Weight = 5.0;
			Hue = Utility.RandomList( 0x3D1, 0, 868, 2053, 2051, 1818 );
			// TODO check
		}

		public AcidProofRope( Serial serial )
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
