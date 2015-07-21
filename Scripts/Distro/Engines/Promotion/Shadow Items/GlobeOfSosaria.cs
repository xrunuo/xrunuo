using System;
using Server;

namespace Server.Items
{
	public class GlobeOfSosariaAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new GlobeOfSosariaDeed(); } }
		public override int LabelNumber { get { return 1076681; } } // Globe Of Sosaria

		[Constructable]
		public GlobeOfSosariaAddon()
		{
			AddComponent( new AddonComponent( 0x3657 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3658 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0x3659 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0x3660 ), 0, 0, 0 );
		}

		public GlobeOfSosariaAddon( Serial serial )
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

	public class GlobeOfSosariaDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new GlobeOfSosariaAddon(); } }
		public override int LabelNumber { get { return 1076681; } } // Globe Of Sosaria

		[Constructable]
		public GlobeOfSosariaDeed()
		{
			Hue = 1908;
		}

		public GlobeOfSosariaDeed( Serial serial )
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