using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class SmallHalloweenWeb : Item
	{
		[Constructable]
		public SmallHalloweenWeb()
			: base( Utility.RandomList( 0x10D7, 0x10D6 ) )
		{
			Hue = Utility.RandomList( 2047, 144, 2650 );
			Weight = 1.0;
		}

		public SmallHalloweenWeb( Serial serial )
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
