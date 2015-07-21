using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class TerMurStyleDresserSouthAddon : BaseAddonContainer
	{
		public override BaseAddonContainerDeed Deed { get { return new TerMurStyleDresserSouthAddonDeed(); } }
		public override int LabelNumber { get { return 1095299; } } // Ter-Mur style dresser
		public override int DefaultGumpID { get { return 0x107; } }
		public override int DefaultDropSound { get { return 0x42; } }

		[Constructable]
		public TerMurStyleDresserSouthAddon()
			: base( 0x402B )
		{
			// TODO (SA): Debe poder tener dos containers en vez de uno
			AddComponent( new AddonContainerComponent( 0x402C ), 1, 0, 0 );
		}

		public TerMurStyleDresserSouthAddon( Serial serial )
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

	public class TerMurStyleDresserSouthAddonDeed : BaseAddonContainerDeed
	{
		public override BaseAddonContainer Addon { get { return new TerMurStyleDresserSouthAddon(); } }
		public override int LabelNumber { get { return 1111783; } } // Ter-Mur style dresser

		[Constructable]
		public TerMurStyleDresserSouthAddonDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public TerMurStyleDresserSouthAddonDeed( Serial serial )
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

	public class TerMurStyleDresserEastAddon : BaseAddonContainer
	{
		public override BaseAddonContainerDeed Deed { get { return new TerMurStyleDresserEastAddonDeed(); } }
		public override int LabelNumber { get { return 1095299; } } // Ter-Mur style dresser
		public override int DefaultGumpID { get { return 0x107; } }
		public override int DefaultDropSound { get { return 0x42; } }

		[Constructable]
		public TerMurStyleDresserEastAddon()
			: base( 0x402D )
		{
			AddComponent( new AddonContainerComponent( 0x402E ), 0, 1, 0 );
		}

		public TerMurStyleDresserEastAddon( Serial serial )
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

	public class TerMurStyleDresserEastAddonDeed : BaseAddonContainerDeed
	{
		public override BaseAddonContainer Addon { get { return new TerMurStyleDresserEastAddon(); } }
		public override int LabelNumber { get { return 1111784; } } // Ter-Mur style dresser

		[Constructable]
		public TerMurStyleDresserEastAddonDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public TerMurStyleDresserEastAddonDeed( Serial serial )
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
