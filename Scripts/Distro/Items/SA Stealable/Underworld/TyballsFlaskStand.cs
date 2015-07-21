using System;

namespace Server.Items
{
	public class TyballsFlaskStand : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		public override int LabelNumber { get { return 1113659; } } // Tyball's flask stand

		[Constructable]
		public TyballsFlaskStand()
			: base( 0x1829 )
		{
			Weight = 10.0;
		}

		public TyballsFlaskStand( Serial serial )
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
