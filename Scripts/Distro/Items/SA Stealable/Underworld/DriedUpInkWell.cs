using System;

namespace Server.Items
{
	public class DriedUpInkWell : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		public override int LabelNumber { get { return 1113662; } } // dried up ink well

		[Constructable]
		public DriedUpInkWell()
			: base( 0x2D61 )
		{
			Weight = 10.0;
		}

		public DriedUpInkWell( Serial serial )
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
