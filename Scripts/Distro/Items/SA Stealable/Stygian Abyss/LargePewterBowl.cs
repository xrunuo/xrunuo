using System;

namespace Server.Items
{
	public class LargePewterBowl : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1095958; } } // Large Pewter Bowl

		[Constructable]
		public LargePewterBowl()
			: base( 0x42BE )
		{
			Weight = 10.0;
		}

		public LargePewterBowl( Serial serial )
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
