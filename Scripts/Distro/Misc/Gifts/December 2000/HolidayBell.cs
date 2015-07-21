using System;

namespace Server.Items
{
	public class HolidayBell : Item
	{
		public int offset;

		[Constructable]
		public HolidayBell()
			: base( 0x1c12 )
		{
			Stackable = false;

			Weight = 1.0;

			Name = "Holiday Bell";

			Hue = Utility.RandomDyedHue();

			LootType = LootType.Blessed;

			offset = Utility.Random( 0, 11 );
		}

		public HolidayBell( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1041432 + offset );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Effects.PlaySound( from, from.Map, 0xF5 + offset );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 

			writer.Write( (int) offset );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();

			offset = reader.ReadInt();
		}
	}
}
