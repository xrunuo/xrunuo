using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class BasketA : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 1; } }

		[Constructable]
		public BasketA()
			: base( 0x24DD )
		{
		}

		public BasketA( Serial serial )
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

	public class BasketB : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 1; } }

		[Constructable]
		public BasketB()
			: base( 0x24D7 )
		{
		}

		public BasketB( Serial serial )
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

	public class BasketC : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 1; } }

		[Constructable]
		public BasketC()
			: base( 0x24DA )
		{
		}

		public BasketC( Serial serial )
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

	public class BasketC1 : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 1; } }

		[Constructable]
		public BasketC1()
			: base( 0x24D9 )
		{
		}

		public BasketC1( Serial serial )
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

	public class BasketD : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 2; } }

		[Constructable]
		public BasketD()
			: base( 0x24D8 )
		{
		}

		public BasketD( Serial serial )
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

	public class BasketE0 : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 2; } }

		[Constructable]
		public BasketE0()
			: base( 0x24DB )
		{
		}

		public BasketE0( Serial serial )
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

	public class BasketE : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 2; } }

		[Constructable]
		public BasketE()
			: base( 0x24DC )
		{
		}

		public BasketE( Serial serial )
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

	[FlipableAttribute( 0x24D5, 0x24D6 )]
	public class BasketF : StealableContainerArtifact
	{
		public override int ArtifactRarity { get { return 2; } }

		[Constructable]
		public BasketF()
			: base( 0x24D5 )
		{
		}

		public BasketF( Serial serial )
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
