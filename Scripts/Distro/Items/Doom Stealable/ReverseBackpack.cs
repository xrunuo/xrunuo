using System;

namespace Server.Items
{
	public class ReverseBackpack : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public ReverseBackpack()
			: base( 0x9B2 )
		{
		}

		public ReverseBackpack( Serial serial )
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

			if ( ItemID != 0x9B2 )
			{
				ItemID = 0x9B2;
			}
		}
	}
}
