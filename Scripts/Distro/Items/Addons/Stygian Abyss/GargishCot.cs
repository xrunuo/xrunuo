using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class GargishCotSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new GargishCotSouthAddonDeed(); } }
		public override int LabelNumber { get { return 1111920; } } // gargish cot (south)
		public override bool RetainDeedHue { get { return true; } }

		[Constructable]
		public GargishCotSouthAddon( int hue )
		{
			AddComponent( new AddonComponent( 0x400C ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x400D ), 0, 1, 0 );

			Hue = hue;
		}

		public GargishCotSouthAddon( Serial serial )
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

	public class GargishCotSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new GargishCotSouthAddon( this.Hue ); } }
		public override int LabelNumber { get { return 1111920; } } // gargish cot (south)

		[Constructable]
		public GargishCotSouthAddonDeed()
		{
			LootType = LootType.Blessed;
		}

		public GargishCotSouthAddonDeed( Serial serial )
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

	public class GargishCotEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new GargishCotEastAddonDeed(); } }
		public override int LabelNumber { get { return 1111921; } } // gargish cot (east)
		public override bool RetainDeedHue { get { return true; } }

		[Constructable]
		public GargishCotEastAddon( int hue )
		{
			AddComponent( new AddonComponent( 0x400E ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x400F ), 1, 0, 0 );

			Hue = hue;
		}

		public GargishCotEastAddon( Serial serial )
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

	public class GargishCotEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new GargishCotEastAddon( this.Hue ); } }
		public override int LabelNumber { get { return 1111921; } } // gargish cot (east)

		[Constructable]
		public GargishCotEastAddonDeed()
		{
			LootType = LootType.Blessed;
		}

		public GargishCotEastAddonDeed( Serial serial )
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