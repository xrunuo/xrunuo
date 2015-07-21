using System;

namespace Server.Items
{
	public class StretchedHide : StealableArtifact
	{
		public override int ArtifactRarity { get { return 2; } }

		[Constructable]
		public StretchedHide()
			: base( 0x106B )
		{
		}

		public StretchedHide( Serial serial )
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

			if ( ItemID != 0x106B )
			{
				ItemID = 0x106B;
			}
		}
	}
}
