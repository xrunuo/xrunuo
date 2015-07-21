using System;

namespace Server.Items
{
	public class ClosedBarrel : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public ClosedBarrel()
			: base( 0x0FAE )
		{
		}

		public ClosedBarrel( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
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
