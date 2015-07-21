using System;
using Server;

namespace Server.Items
{
	public class ArcaneBookshelfEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ArcaneBookshelfEastDeed(); } }


		[Constructable]
		public ArcaneBookshelfEastAddon()
		{
			AddComponent( new AddonComponent( 0x2DEF ), 0, 0, 0 );
		}

		public ArcaneBookshelfEastAddon( Serial serial )
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

	public class ArcaneBookshelfEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ArcaneBookshelfEastAddon(); } }
		public override int LabelNumber { get { return 1073371; } } // Arcane Bookshelf (east)

		[Constructable]
		public ArcaneBookshelfEastDeed()
		{
		}

		public ArcaneBookshelfEastDeed( Serial serial )
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