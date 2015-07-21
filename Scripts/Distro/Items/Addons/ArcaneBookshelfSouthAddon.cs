using System;
using Server;

namespace Server.Items
{
	public class ArcaneBookshelfSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ArcaneBookshelfSouthDeed(); } }


		[Constructable]
		public ArcaneBookshelfSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2DF0 ), 0, 0, 0 );
		}

		public ArcaneBookshelfSouthAddon( Serial serial )
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

	public class ArcaneBookshelfSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ArcaneBookshelfSouthAddon(); } }
		public override int LabelNumber { get { return 1072871; } } // Arcane Bookshelf (South)

		[Constructable]
		public ArcaneBookshelfSouthDeed()
		{
		}

		public ArcaneBookshelfSouthDeed( Serial serial )
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