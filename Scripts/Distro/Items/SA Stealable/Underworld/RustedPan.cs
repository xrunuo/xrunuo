using System;

namespace Server.Items
{
	public class RustedPan : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		public override int LabelNumber { get { return 1113670; } } // rusted pan

		[Constructable]
		public RustedPan()
			: base( 0x09E8 )
		{
			Weight = 10.0;
		}

		public RustedPan( Serial serial )
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
