using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0xF65, 0xF67, 0xF69 )]
	public class Easle : Item
	{
		[Constructable]
		public Easle()
			: base( 0xF65 )
		{
			Weight = 25.0;
		}

		public Easle( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();

			if ( Weight == 10.0 )
			{
				Weight = 25.0;
			}
		}
	}

	[Furniture]
	[Flipable( 0xF67, 0xF65, 0xF69 )]
	public class EasleEast : Item
	{
		[Constructable]
		public EasleEast()
			: base( 0xF67 )
		{
			Weight = 25.0;
		}

		public EasleEast( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	[Furniture]
	[Flipable( 0xF69, 0xF67, 0xF65 )]
	public class EasleNorth : Item
	{
		[Constructable]
		public EasleNorth()
			: base( 0xF69 )
		{
			Weight = 25.0;
		}

		public EasleNorth( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}
}