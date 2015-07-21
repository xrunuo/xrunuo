using System;
using Server;

namespace Server.Items
{
	public class Begging_PitcherOfWater2 : Pitcher
	{
		[Constructable]
		public Begging_PitcherOfWater2()
			: base( BeverageType.Water )
		{
			ItemID = 0xFF9;
		}

		public Begging_PitcherOfWater2( Serial serial )
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