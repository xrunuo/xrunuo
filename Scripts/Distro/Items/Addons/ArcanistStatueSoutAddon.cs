using System;
using Server;

namespace Server.Items
{
	public class ArcanistStatueSoutAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ArcanistStatueSoutDeed(); } }


		[Constructable]
		public ArcanistStatueSoutAddon()
		{
			AddComponent( new AddonComponent( 0x2D0E ), 0, 0, 0 );
		}

		public ArcanistStatueSoutAddon( Serial serial )
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

	public class ArcanistStatueSoutDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ArcanistStatueSoutAddon(); } }
		public override int LabelNumber { get { return 1072885; } }

		[Constructable]
		public ArcanistStatueSoutDeed()
		{
		}

		public ArcanistStatueSoutDeed( Serial serial )
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