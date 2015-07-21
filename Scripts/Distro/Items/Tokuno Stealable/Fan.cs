using System;

namespace Server.Items
{
	public class FanA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public FanA()
			: base( 0x240A )
		{
		}

		public FanA( Serial serial )
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

	public class FanAO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public FanAO()
			: base( 0x2409 )
		{
		}

		public FanAO( Serial serial )
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

	public class FanB : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public FanB()
			: base( 0x240C )
		{
		}

		public FanB( Serial serial )
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

	public class FanBO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public FanBO()
			: base( 0x240B )
		{
		}

		public FanBO( Serial serial )
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
