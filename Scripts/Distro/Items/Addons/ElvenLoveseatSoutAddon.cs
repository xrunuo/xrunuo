using System;
using Server;

namespace Server.Items
{
	public class ElvenLoveseatSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ElvenLoveseatSouthDeed(); } }


		[Constructable]
		public ElvenLoveseatSouthAddon()
		{
			AddComponent( new AddonComponent( 0x2DE0 ), 0, 0, 0 );
		}

		public ElvenLoveseatSouthAddon( Serial serial )
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

	public class ElvenLoveseatSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ElvenLoveseatSouthAddon(); } }
		public override int LabelNumber { get { return 1074898; } }

		[Constructable]
		public ElvenLoveseatSouthDeed()
		{
		}

		public ElvenLoveseatSouthDeed( Serial serial )
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