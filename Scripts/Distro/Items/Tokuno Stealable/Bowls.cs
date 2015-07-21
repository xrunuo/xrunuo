using System;

namespace Server.Items
{
	public class Bowl : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public Bowl()
			: base( 0x24DE )
		{
		}

		public Bowl( Serial serial )
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

	public class BowlsLight : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public BowlsLight()
			: base( 0x24DF )
		{
		}

		public BowlsLight( Serial serial )
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

	public class BowlsDark : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public BowlsDark()
			: base( 0x24E0 )
		{
		}

		public BowlsDark( Serial serial )
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
