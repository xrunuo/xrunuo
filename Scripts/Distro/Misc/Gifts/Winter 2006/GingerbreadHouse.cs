using System;
using Server;

namespace Server.Items
{
	public class GingerbreadHouseAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new GingerbreadHouseDeed(); } }
		public override int LabelNumber { get { return 1077395; } } // Gingerbread House

		[Constructable]
		public GingerbreadHouseAddon()
		{
			AddComponent( new GingerbreadHouseAddonComponent( 0x2BE6 ), 0, 0, 0 );
			AddComponent( new GingerbreadHouseAddonComponent( 0x2BE7 ), 0, -1, 0 );
			AddComponent( new GingerbreadHouseAddonComponent( 0x2BE5 ), -1, 0, 0 );
		}

		public GingerbreadHouseAddon( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}

	public class GingerbreadHouseAddonComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1077395; } } // Gingerbread House

		public GingerbreadHouseAddonComponent( int itemID )
			: base( itemID )
		{
		}

		public GingerbreadHouseAddonComponent( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class GingerbreadHouseDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new GingerbreadHouseAddon(); } }
		public override int LabelNumber { get { return 1077394; } } // a Gingerbread House Deed

		[Constructable]
		public GingerbreadHouseDeed()
		{
		}

		public GingerbreadHouseDeed( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}