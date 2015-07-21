using System;

namespace Server.Items
{
	public class Teapot : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public Teapot()
			: base( 0x24E6 )
		{
		}

		public Teapot( Serial serial )
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

	public class Teapot1 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public Teapot1()
			: base( 0x24E7 )
		{
		}

		public Teapot1( Serial serial )
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