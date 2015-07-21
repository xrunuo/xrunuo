using System;

namespace Server.Items
{
	public class TarotCards : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public TarotCards()
			: base( 0x12A5 )
		{
		}

		public TarotCards( Serial serial )
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

			if ( ItemID != 0x12A5 )
			{
				ItemID = 0x12A5;
			}
		}
	}
}
