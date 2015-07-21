using System;

namespace Server.Items
{
	public class Rocks : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public Rocks()
			: base( 0x1367 )
		{
		}

		public Rocks( Serial serial )
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

			if ( Weight != 10.0 )
			{
				Weight = 10.0;
			}
		}
	}
}
