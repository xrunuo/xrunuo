using System;

namespace Server.Items
{
	public class StolenBottlesofLiquor2 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }
		public override int LabelNumber { get { return 1113667; } } // Stolen Bottles of Liquor (2)

		[Constructable]
		public StolenBottlesofLiquor2()
			: base( 0x099C )
		{
			Weight = 10.0;
		}

		public StolenBottlesofLiquor2( Serial serial )
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

	public class StolenBottlesofLiquor3 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }
		public override int LabelNumber { get { return 1113666; } } // Stolen Bottles of Liquor (3)

		[Constructable]
		public StolenBottlesofLiquor3()
			: base( 0x099D )
		{
			Weight = 10.0;
		}

		public StolenBottlesofLiquor3( Serial serial )
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
	public class StolenBottlesofLiquor4 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }
		public override int LabelNumber { get { return 1113668; } } // Stolen Bottles of Liquor (4)

		[Constructable]
		public StolenBottlesofLiquor4()
			: base( 0x099E )
		{
			Weight = 10.0;
		}

		public StolenBottlesofLiquor4( Serial serial )
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
