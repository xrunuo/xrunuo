using System;
using Server;

namespace Server.Items
{
	public class ElvenOvenEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ElvenOvenEastDeed(); } }


		[Constructable]
		public ElvenOvenEastAddon()
		{
			AddComponent( new AddonComponent( 0x2DDC ), 0, 0, 0 );
		}

		public ElvenOvenEastAddon( Serial serial )
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

	public class ElvenOvenEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ElvenOvenEastAddon(); } }
		public override int LabelNumber { get { return 1073395; } }

		[Constructable]
		public ElvenOvenEastDeed()
		{
		}

		public ElvenOvenEastDeed( Serial serial )
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