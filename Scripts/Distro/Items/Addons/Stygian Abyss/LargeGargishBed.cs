using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class LargeGargishBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LargeGargishBedSouthAddonDeed(); } }
		public override int LabelNumber { get { return 1111761; } } // large gargish bed (south)
		public override bool RetainDeedHue { get { return true; } }

		[Constructable]
		public LargeGargishBedSouthAddon()
			: this( 0 )
		{
		}

		[Constructable]
		public LargeGargishBedSouthAddon( int hue )
		{
			AddComponent( new AddonComponent( 0x4010 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x4011 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0x4012 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 0x4013 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x4014 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0x4015 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 0x4016 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 0x4017 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 0x4018 ), 2, 2, 0 );

			Hue = hue;
		}

		public LargeGargishBedSouthAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class LargeGargishBedSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LargeGargishBedSouthAddon( this.Hue ); } }
		public override int LabelNumber { get { return 1111761; } } // large gargish bed (south)

		[Constructable]
		public LargeGargishBedSouthAddonDeed()
		{
		}

		public LargeGargishBedSouthAddonDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class LargeGargishBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LargeGargishBedEastAddonDeed(); } }
		public override int LabelNumber { get { return 1111762; } } // large gargish bed (East)
		public override bool RetainDeedHue { get { return true; } }

		[Constructable]
		public LargeGargishBedEastAddon()
			: this( 0 )
		{
		}

		[Constructable]
		public LargeGargishBedEastAddon( int hue )
		{
			AddComponent( new AddonComponent( 0x4019 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x401C ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x401F ), 0, 2, 0 );
			AddComponent( new AddonComponent( 0x401A ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0x401D ), 1, 1, 0 );
			AddComponent( new AddonComponent( 0x4020 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 0x401B ), 2, 0, 0 );
			AddComponent( new AddonComponent( 0x401E ), 2, 1, 0 );
			AddComponent( new AddonComponent( 0x4021 ), 2, 2, 0 );

			Hue = hue;
		}

		public LargeGargishBedEastAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class LargeGargishBedEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LargeGargishBedEastAddon( this.Hue ); } }
		public override int LabelNumber { get { return 1111762; } } // large gargish bed (East)

		[Constructable]
		public LargeGargishBedEastAddonDeed()
		{
		}

		public LargeGargishBedEastAddonDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}