using System;
using Server;

namespace Server.Items
{
	public class SoulforgeAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SoulforgeDeed(); } }

		[Constructable]
		public SoulforgeAddon()
		{
			int itemId = 0x4263;

			for ( int i = 0; i < 4; i++ )
				for ( int j = 0; j < 4; j++ )
					AddComponent( new AddonComponent( itemId++ ), j, i, 0 );
		}

		public SoulforgeAddon( Serial serial )
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

	public class SoulforgeDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SoulforgeAddon(); } }
		public override int LabelNumber { get { return 1095867; } } // soulforge

		[Constructable]
		public SoulforgeDeed()
		{
		}

		public SoulforgeDeed( Serial serial )
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

	public class RoofSoulforgeAddon : BaseAddon
	{
		[Constructable]
		public RoofSoulforgeAddon()
		{
			int itemId = 0x4277;

			for ( int i = 0; i < 4; i++ )
				for ( int j = 0; j < 4; j++ )
					AddComponent( new AddonComponent( itemId++ ), j, i, 0 );
		}

		public RoofSoulforgeAddon( Serial serial )
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

	public class SmallSoulforgeSouthAddon : BaseAddon
	{
		[Constructable]
		public SmallSoulforgeSouthAddon()
		{
			AddComponent( new AddonComponent( 0x4256 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x4257 ), 1, 0, 0 );
		}

		public SmallSoulforgeSouthAddon( Serial serial )
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