using System;
using Server;

namespace Server.Items
{
	public class WarriorStatueSoutAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new WarriorStatueSoutDeed(); } }


		[Constructable]
		public WarriorStatueSoutAddon()
		{
			AddComponent( new AddonComponent( 0x2D12 ), 0, 0, 0 );
		}

		public WarriorStatueSoutAddon( Serial serial )
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

	public class WarriorStatueSoutDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new WarriorStatueSoutAddon(); } }
		public override int LabelNumber { get { return 1072887; } }

		[Constructable]
		public WarriorStatueSoutDeed()
		{
		}

		public WarriorStatueSoutDeed( Serial serial )
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