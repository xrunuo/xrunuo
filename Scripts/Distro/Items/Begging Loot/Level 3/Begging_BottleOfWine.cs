using System;
using Server;

namespace Server.Items
{
	public class Begging_BottleOfWine : BeverageBottle
	{
		[Constructable]
		public Begging_BottleOfWine()
			: base( BeverageType.Wine )
		{
		}

		public Begging_BottleOfWine( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1075129 ); // Acquired by begging
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			reader.ReadInt();
		}
	}
}