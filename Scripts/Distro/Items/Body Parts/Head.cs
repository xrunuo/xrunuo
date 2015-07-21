using System;
using Server;

namespace Server.Items
{
	public class Head : Item
	{
		[Constructable]
		public Head()
			: this( null )
		{
		}

		[Constructable]
		public Head( string name )
			: base( 0x1DA0 )
		{
			Name = name;
			Weight = 1.0;
		}

		public Head( Serial serial )
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