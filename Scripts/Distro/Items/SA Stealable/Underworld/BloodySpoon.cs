using System;

namespace Server.Items
{
	public class BloodySpoon : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113664; } } // Bloody Spoon

		[Constructable]
		public BloodySpoon()
			: base( 0x09C2 )
		{
			Weight = 10.0;
		}

		public BloodySpoon( Serial serial )
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
