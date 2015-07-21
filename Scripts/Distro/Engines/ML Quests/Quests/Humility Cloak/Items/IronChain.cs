using System;
using Server;

namespace Server.Items
{
	public class IronChain : Item
	{
		public override int LabelNumber { get { return 1075788; } } // Iron Chain

		[Constructable]
		public IronChain()
			: base( 0x1F0A )
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

		public IronChain( Serial serial )
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