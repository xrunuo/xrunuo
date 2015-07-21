using System;

namespace Server.Items
{
	public class NavreysWebA1 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		public override int LabelNumber { get { return 1113671; } } // Navrey's web (1)

		[Constructable]
		public NavreysWebA1()
			: base( 0x0EE3 )
		{
			Weight = 10.0;
		}

		public NavreysWebA1( Serial serial )
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

	public class NavreysWebB1 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113671; } } // Navrey's web (1)

		[Constructable]
		public NavreysWebB1()
			: base( 0x0EE3 )
		{
			Weight = 10.0;
		}

		public NavreysWebB1( Serial serial )
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

	public class NavreysWebA2 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		public override int LabelNumber { get { return 1113672; } } // Navrey's web (2)

		[Constructable]
		public NavreysWebA2()
			: base( 0x0EE5 )
		{
			Weight = 10.0;
		}

		public NavreysWebA2( Serial serial )
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

	public class NavreysWebB2 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113672; } } // Navrey's web (2)

		[Constructable]
		public NavreysWebB2()
			: base( 0x0EE5 )
		{
			Weight = 10.0;
		}

		public NavreysWebB2( Serial serial )
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

	public class NavreysWeb3 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113673; } } // Navrey's web (3)

		[Constructable]
		public NavreysWeb3()
			: base( 0x0EE4 )
		{
			Weight = 10.0;
		}

		public NavreysWeb3( Serial serial )
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

	public class NavreysWeb4 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113675; } } // Navrey's web (4)

		[Constructable]
		public NavreysWeb4()
			: base( 0x0EE6 )
		{
			Weight = 10.0;
		}

		public NavreysWeb4( Serial serial )
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
