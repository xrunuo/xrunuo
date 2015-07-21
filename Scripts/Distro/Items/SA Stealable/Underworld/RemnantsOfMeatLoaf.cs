using System;

namespace Server.Items
{
	public class RemnantsOfMeatLoaf : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113661; } } // remnants of meat loaf

		[Constructable]
		public RemnantsOfMeatLoaf()
			: base( 0x09AE )
		{
			Weight = 10.0;
		}

		public RemnantsOfMeatLoaf( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
