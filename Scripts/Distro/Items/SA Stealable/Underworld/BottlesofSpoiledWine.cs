using System;

namespace Server.Items
{
	public class BottlesofSpoiledWine2 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		public override int LabelNumber { get { return 1113676; } } // bottles of spoiled wine (2)

		[Constructable]
		public BottlesofSpoiledWine2()
			: base( 0x09C6 )
		{
			Weight = 10.0;
		}

		public BottlesofSpoiledWine2( Serial serial )
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

	public class BottlesofSpoiledWine3 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }
		public override int LabelNumber { get { return 1113677; } } // bottles of spoiled wine (3)

		[Constructable]
		public BottlesofSpoiledWine3()
			: base( 0x09C5 )
		{
			Weight = 10.0;
		}

		public BottlesofSpoiledWine3( Serial serial )
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
	public class BottlesofSpoiledWine4 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }
		public override int LabelNumber { get { return 1113678; } } // bottles of spoiled wine (4)

		[Constructable]
		public BottlesofSpoiledWine4()
			: base( 0x09C4 )
		{
			Weight = 10.0;
		}

		public BottlesofSpoiledWine4( Serial serial )
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
