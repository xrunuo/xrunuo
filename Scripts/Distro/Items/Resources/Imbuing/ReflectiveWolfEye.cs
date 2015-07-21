using System;
using Server;

namespace Server.Items
{
	public class ReflectiveWolfEye : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113362; } } // reflective wolf eye

		[Constructable]
		public ReflectiveWolfEye()
			: this( 1 )
		{
		}

		[Constructable]
		public ReflectiveWolfEye( int amount )
			: base( 0x5749 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public ReflectiveWolfEye( Serial serial )
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
