using System;

namespace Server.Items
{
	public class SculptureA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public SculptureA()
			: base( 0x2419 )
		{
		}

		public SculptureA( Serial serial )
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

	public class SculptureB : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public SculptureB()
			: base( 0x241A )
		{
		}

		public SculptureB( Serial serial )
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

	public class SculptureC : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public SculptureC()
			: base( 0x241B )
		{
		}

		public SculptureC( Serial serial )
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

	[FlipableAttribute( 0x2846, 0x2847 )]
	public class SculptureD : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public SculptureD()
			: base( 0x2846 )
		{
		}

		public SculptureD( Serial serial )
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

	[FlipableAttribute( 0x2847, 0x2846 )]
	public class SculptureDO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public SculptureDO()
			: base( 0x2847 )
		{
		}

		public SculptureDO( Serial serial )
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

	public class SculptureE : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public SculptureE()
			: base( 0x2848 )
		{
		}

		public SculptureE( Serial serial )
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

	public class SculptureEO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public SculptureEO()
			: base( 0x2849 )
		{
		}

		public SculptureEO( Serial serial )
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