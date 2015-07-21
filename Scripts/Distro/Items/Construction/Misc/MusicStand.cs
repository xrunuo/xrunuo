using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0xEBB, 0xEBC )]
	public class TallMusicStand : Item
	{
		[Constructable]
		public TallMusicStand()
			: base( 0xEBB )
		{
			Weight = 10.0;
		}

		public TallMusicStand( Serial serial )
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

			if ( Weight == 8.0 )
			{
				Weight = 10.0;
			}
		}
	}

	[Furniture]
	[Flipable( 0xEB6, 0xEB8 )]
	public class ShortMusicStand : Item
	{
		[Constructable]
		public ShortMusicStand()
			: base( 0xEB6 )
		{
			Weight = 10.0;
		}

		public ShortMusicStand( Serial serial )
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

			if ( Weight == 6.0 )
			{
				Weight = 10.0;
			}
		}
	}

	[Furniture]
	[Flipable( 0xEBC, 0xEBB )]
	public class TallMusicStandRight : Item
	{
		[Constructable]
		public TallMusicStandRight()
			: base( 0xEBC )
		{
			Weight = 10.0;
		}

		public TallMusicStandRight( Serial serial )
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

			if ( Weight == 8.0 )
			{
				Weight = 10.0;
			}
		}
	}

	[Furniture]
	[Flipable( 0xEB8, 0xEB6 )]
	public class ShortMusicStandRight : Item
	{
		[Constructable]
		public ShortMusicStandRight()
			: base( 0xEB8 )
		{
			Weight = 10.0;
		}

		public ShortMusicStandRight( Serial serial )
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

			if ( Weight == 6.0 )
			{
				Weight = 10.0;
			}
		}
	}
}