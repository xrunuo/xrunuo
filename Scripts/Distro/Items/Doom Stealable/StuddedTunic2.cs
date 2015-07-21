using System;

namespace Server.Items
{
	public class StuddedTunic2 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		[Constructable]
		public StuddedTunic2()
			: base( 0x13E0 )
		{
		}

		public StuddedTunic2( Serial serial )
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
