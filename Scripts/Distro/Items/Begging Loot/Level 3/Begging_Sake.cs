using System;
using Server;

namespace Server.Items
{
	public class Begging_Sake : Item
	{
		[Constructable]
		public Begging_Sake()
			: base( 0x24E2 )
		{
			Weight = 1.0;
		}

		public Begging_Sake( Serial serial )
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