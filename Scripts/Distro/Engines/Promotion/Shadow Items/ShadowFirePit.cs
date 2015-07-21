using System;
using Server;

namespace Server.Items
{
	public class ShadowFirePitAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ShadowFirePitDeed(); } }
		public override int LabelNumber { get { return 1076680; } } // Shadow Fire Pit

		[Constructable]
		public ShadowFirePitAddon()
		{
			AddComponent( new AddonComponent( 0x3651 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3652 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0x3653 ), 0, -1, 0 );
		}

		public ShadowFirePitAddon( Serial serial )
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

	public class ShadowFirePitDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new ShadowFirePitAddon(); } }
		public override int LabelNumber { get { return 1076680; } } // Shadow Fire Pit

		[Constructable]
		public ShadowFirePitDeed()
		{
			Hue = 1908;
		}

		public ShadowFirePitDeed( Serial serial )
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