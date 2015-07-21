using System;

namespace Server.Items
{
	public class SwordDisplayA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public SwordDisplayA()
			: base( 0x2842 )
		{
		}

		public SwordDisplayA( Serial serial )
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

	public class SwordDisplayAO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public SwordDisplayAO()
			: base( 0x2843 )
		{
		}

		public SwordDisplayAO( Serial serial )
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

	[FlipableAttribute( 0x2844, 0x2845 )]
	public class SwordDisplayB : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		[Constructable]
		public SwordDisplayB()
			: base( 0x2844 )
		{
		}

		public SwordDisplayB( Serial serial )
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

	[FlipableAttribute( 0x2845, 0x2844 )]
	public class SwordDisplayBO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		[Constructable]
		public SwordDisplayBO()
			: base( 0x2845 )
		{
		}

		public SwordDisplayBO( Serial serial )
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

	public class SwordDisplayC : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public SwordDisplayC()
			: base( 0x2855 )
		{
		}

		public SwordDisplayC( Serial serial )
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

	public class SwordDisplayCO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public SwordDisplayCO()
			: base( 0x2856 )
		{
		}

		public SwordDisplayCO( Serial serial )
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

	public class SwordDisplayD : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public SwordDisplayD()
			: base( 0x2853 )
		{
		}

		public SwordDisplayD( Serial serial )
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

	public class SwordDisplayDO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public SwordDisplayDO()
			: base( 0x2854 )
		{
		}

		public SwordDisplayDO( Serial serial )
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

	[FlipableAttribute( 0x2851, 0x2852 )]
	public class SwordDisplayE : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public SwordDisplayE()
			: base( 0x2851 )
		{
		}

		public SwordDisplayE( Serial serial )
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

	[FlipableAttribute( 0x2852, 0x2851 )]
	public class SwordDisplayEO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public SwordDisplayEO()
			: base( 0x2852 )
		{
		}

		public SwordDisplayEO( Serial serial )
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