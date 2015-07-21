using System;

namespace Server.Items
{
	public class StuddedLeggings : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public StuddedLeggings()
			: base( 0x13D8 )
		{
		}

		public StuddedLeggings( Serial serial )
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
