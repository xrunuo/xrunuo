using System;
using Server;

namespace Server.Items
{
	public class PlainGreyCloak : Cloak
	{
		public override int LabelNumber { get { return 1075789; } } // A Plain Grey Cloak

		[Constructable]
		public PlainGreyCloak()
		{
			LootType = LootType.Blessed;
			Hue = 0x386;
		}

		public override bool NonTransferable { get { return true; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public PlainGreyCloak( Serial serial )
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