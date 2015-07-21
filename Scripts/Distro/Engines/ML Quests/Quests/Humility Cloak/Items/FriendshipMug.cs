using System;
using Server;

namespace Server.Items
{
	public class FriendshipMug : Item
	{
		public override int LabelNumber { get { return 1075777; } } // Friendship Mug

		[Constructable]
		public FriendshipMug()
			: base( 0x9EF )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public override bool NonTransferable { get { return true; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public FriendshipMug( Serial serial )
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