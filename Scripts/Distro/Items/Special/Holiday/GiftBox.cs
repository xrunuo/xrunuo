using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0x232A, 0x232B )]
	public class GiftBox : BaseContainer
	{
		[Constructable]
		public GiftBox()
			: this( Utility.RandomDyedHue() )
		{
		}

		[Constructable]
		public GiftBox( int hue )
			: base( Utility.Random( 0x232A, 2 ) )
		{
			Weight = 2.0;
			Hue = hue;
		}

		public GiftBox( Serial serial )
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

	[Furniture]
	public class GiftBox1 : BaseContainer
	{
		// rectangle east
		public override int DefaultGumpID { get { return 0x11E; } }

		[Constructable]
		public GiftBox1()
			: this( Utility.RandomDyedHue() )
		{
		}

		[Constructable]
		public GiftBox1( int hue )
			: base( 0x46A5 )
		{
			Weight = 2.0;
			Hue = hue;
		}

		public GiftBox1( Serial serial )
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
	[Furniture]
	public class GiftBox2 : BaseContainer
	{
		// cube east
		public override int DefaultGumpID { get { return 0x11B; } }

		[Constructable]
		public GiftBox2()
			: this( Utility.RandomDyedHue() )
		{
		}

		[Constructable]
		public GiftBox2( int hue )
			: base( 0x46A2 )
		{
			Weight = 2.0;
			Hue = hue;
		}

		public GiftBox2( Serial serial )
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

	[Furniture]
	public class GiftBox3 : BaseContainer
	{
		// green cilinder east
		public override int DefaultGumpID { get { return 0x11C; } }

		[Constructable]
		public GiftBox3()
			: base( 0x46A3 )
		{
			Weight = 2.0;
		}

		public GiftBox3( Serial serial )
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

	[Furniture]
	public class GiftBox4 : BaseContainer
	{
		// yellow octogon east
		public override int DefaultGumpID { get { return 0x11D; } }

		[Constructable]
		public GiftBox4()
			: base( 0x46A4 )
		{
			Weight = 2.0;
		}

		public GiftBox4( Serial serial )
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

	[Furniture]
	public class GiftBox5 : BaseContainer
	{
		// rectangle south
		public override int DefaultGumpID { get { return 0x11E; } }

		[Constructable]
		public GiftBox5()
			: base( 0x46A6 )
		{
			Weight = 2.0;
		}

		public GiftBox5( Serial serial )
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

	[Furniture]
	public class GiftBoxRare : BaseContainer
	{
		// angels rare east
		public override int DefaultGumpID { get { return 0x11F; } }

		[Constructable]
		public GiftBoxRare()
			: base( 0x46A7 )
		{
			Weight = 2.0;
		}

		public GiftBoxRare( Serial serial )
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