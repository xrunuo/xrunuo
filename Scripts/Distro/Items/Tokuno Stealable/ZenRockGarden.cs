using System;

namespace Server.Items
{
	public class ZenRockGardenA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 2; } }

		[Constructable]
		public ZenRockGardenA()
			: base( 0x24E4 )
		{
		}

		public ZenRockGardenA( Serial serial )
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

	public class ZenRockGardenB : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public ZenRockGardenB()
			: base( 0x24E3 )
		{
		}

		public ZenRockGardenB( Serial serial )
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

	public class ZenRockGardenC : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public ZenRockGardenC()
			: base( 0x24E5 )
		{
		}

		public ZenRockGardenC( Serial serial )
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