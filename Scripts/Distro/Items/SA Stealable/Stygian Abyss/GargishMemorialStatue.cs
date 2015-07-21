using System;

namespace Server.Items
{
	public class GargishMemorialStatue : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		public override int LabelNumber { get { return 1095963; } } // Gargish Memorial Statue

		[Constructable]
		public GargishMemorialStatue()
			: base( 0x42C3 )
		{
			Weight = 10.0;
		}

		public GargishMemorialStatue( Serial serial )
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
