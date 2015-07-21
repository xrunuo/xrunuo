using System;
using Server;

namespace Server.Items
{
	public class ShortTableAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ShortTableDeed(); } }

		[Constructable]
		public ShortTableAddon()
		{
			AddComponent( new AddonComponent( 0x4033 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x4035 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x4036 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0x4034 ), 1, 0, 0 );
		}

		public ShortTableAddon( Serial serial )
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

	public class ShortTableDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ShortTableAddon(); } }
		public override int LabelNumber { get { return 1095307; } } // short table

		[Constructable]
		public ShortTableDeed()
		{
		}

		public ShortTableDeed( Serial serial )
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
