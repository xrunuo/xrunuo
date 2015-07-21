using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class BrokenLever : Item
	{
		[Constructable]
		public BrokenLever()
			: base( 0x108E )
		{
			Movable = false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			int offset = Utility.Random( 0, 4 );

			from.SendLocalizedMessage( 500357 + offset );
		}

		public BrokenLever( Serial serial )
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