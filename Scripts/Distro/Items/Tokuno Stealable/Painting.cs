using System;

namespace Server.Items
{
	public class PaintingA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public PaintingA()
			: base( 0x240E )
		{
		}

		public PaintingA( Serial serial )
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

	public class PaintingA0 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public PaintingA0()
			: base( 0x240D )
		{
		}

		public PaintingA0( Serial serial )
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

	public class PaintingB : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public PaintingB()
			: base( 0x2410 )
		{
		}

		public PaintingB( Serial serial )
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

	public class PaintingBO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public PaintingBO()
			: base( 0x240F )
		{
		}

		public PaintingBO( Serial serial )
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

	[FlipableAttribute( 0x2412, 0x2411 )]
	public class PaintingC : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public PaintingC()
			: base( 0x2412 )
		{
		}

		public PaintingC( Serial serial )
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

	[FlipableAttribute( 0x2411, 0x2412 )]
	public class PaintingCO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public PaintingCO()
			: base( 0x2411 )
		{
		}

		public PaintingCO( Serial serial )
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

	[FlipableAttribute( 0x2414, 0x2413 )]
	public class PaintingD : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		[Constructable]
		public PaintingD()
			: base( 0x2414 )
		{
		}

		public PaintingD( Serial serial )
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

	[FlipableAttribute( 0x2413, 0x2414 )]
	public class PaintingDO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		[Constructable]
		public PaintingDO()
			: base( 0x2413 )
		{
		}

		public PaintingDO( Serial serial )
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

	[FlipableAttribute( 0x2416, 0x2415 )]
	public class PaintingE : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public PaintingE()
			: base( 0x2416 )
		{
		}

		public PaintingE( Serial serial )
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

	[FlipableAttribute( 0x2415, 0x2416 )]
	public class PaintingEO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public PaintingEO()
			: base( 0x2415 )
		{
		}

		public PaintingEO( Serial serial )
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

	[FlipableAttribute( 0x2418, 0x2417 )]
	public class PaintingF : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public PaintingF()
			: base( 0x2418 )
		{
		}

		public PaintingF( Serial serial )
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

	[FlipableAttribute( 0x2417, 0x2418 )]
	public class PaintingFO : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public PaintingFO()
			: base( 0x2417 )
		{
		}

		public PaintingFO( Serial serial )
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