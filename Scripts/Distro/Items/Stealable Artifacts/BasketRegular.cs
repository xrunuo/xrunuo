using System;

namespace Server.Items
{
	public class BasketRegular : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public BasketRegular()
			: base( 0x0990 )
		{
		}

		public BasketRegular( Serial serial )
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
