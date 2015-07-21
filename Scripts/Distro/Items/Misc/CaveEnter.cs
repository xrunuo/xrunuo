using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class CaveEnter : Item
	{
		[Constructable]
		public CaveEnter()
			: base( 0x737 )
		{
			Movable = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			m.SendLocalizedMessage( 1046299 ); // No one is allowed to enter this cave from here.

			return true;
		}

		public CaveEnter( Serial serial )
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