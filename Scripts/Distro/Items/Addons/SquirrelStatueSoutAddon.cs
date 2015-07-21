using System;
using Server;

namespace Server.Items
{
	public class SquirrelStatueSoutAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SquirrelStatueSoutDeed(); } }


		[Constructable]
		public SquirrelStatueSoutAddon()
		{
			AddComponent( new AddonComponent( 0x2D10 ), 0, 0, 0 );
		}

		public SquirrelStatueSoutAddon( Serial serial )
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

	public class SquirrelStatueSoutDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SquirrelStatueSoutAddon(); } }
		public override int LabelNumber { get { return 1072885; } }

		[Constructable]
		public SquirrelStatueSoutDeed()
		{
		}

		public SquirrelStatueSoutDeed( Serial serial )
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