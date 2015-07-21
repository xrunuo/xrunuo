using System;

namespace Server.Items
{
	[Furniture]
	[FlipableAttribute( 0x4026, 0x4025 )]
	public class GargishChest : LockableContainer
	{
		public override int DefaultGumpID { get { return 0x42; } }

		[Constructable]
		public GargishChest()
			: base( 0x4026 )
		{
			Weight = 1.0;
		}

		public GargishChest( Serial serial )
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