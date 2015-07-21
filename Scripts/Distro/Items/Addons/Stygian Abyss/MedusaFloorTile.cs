using System;
using Server;

namespace Server.Items
{
	public class MedusaFloorTileAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new MedusaFloorTileDeed(); } }

		[Constructable]
		public MedusaFloorTileAddon()
		{
			for ( int i = 0; i < 5; i++ )
			{
				for ( int j = 0; j < 5; j++ )
				{
					AddComponent( new AddonComponent( 16577 + j + 5 * i ), -2 + j, -2 + i, 0 );
				}
			}
		}

		public MedusaFloorTileAddon( Serial serial )
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

	public class MedusaFloorTileDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new MedusaFloorTileAddon(); } }
		public override int LabelNumber { get { return 1113918; } } // a Medusa Floor deed

		[Constructable]
		public MedusaFloorTileDeed()
		{
		}

		public MedusaFloorTileDeed( Serial serial )
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
