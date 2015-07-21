using System;

namespace Server.Items
{
	public class UrnA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public UrnA()
			: base( 0x241D )
		{
		}

		public UrnA( Serial serial )
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
		}
	}

	public class UrnB : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public UrnB()
			: base( 0x241E )
		{
		}

		public UrnB( Serial serial )
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
		}
	}
}