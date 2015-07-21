using System;

namespace Server.Items
{
	public class LeatherTunic : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public LeatherTunic()
			: base( 0x13CA )
		{
		}

		public LeatherTunic( Serial serial )
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
