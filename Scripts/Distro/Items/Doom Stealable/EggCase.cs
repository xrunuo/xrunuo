using System;

namespace Server.Items
{
	public class EggCase : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public EggCase()
			: base( 0x10D9 )
		{
		}

		public EggCase( Serial serial )
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
