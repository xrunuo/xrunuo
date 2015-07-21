using System;
using Server;

namespace Server.Items
{
	public class Begging_PitcherOfLiquor1 : Pitcher
	{
		[Constructable]
		public Begging_PitcherOfLiquor1()
			: base( BeverageType.Liquor )
		{
			ItemID = 0x1F99;
		}

		public Begging_PitcherOfLiquor1( Serial serial )
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