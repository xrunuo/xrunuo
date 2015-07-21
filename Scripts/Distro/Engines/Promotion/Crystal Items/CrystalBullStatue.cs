using System;
using Server;

namespace Server.Items
{
	public class CrystalBullStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalBullStatueDeed(); } }
		public override int LabelNumber { get { return 1076671; } } // Crystal Bull Statue

		[Constructable]
		public CrystalBullStatueAddon()
		{
			AddComponent( new AddonComponent( 0x35FE ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x35FF ), 0, -1, 0 );
		}

		public CrystalBullStatueAddon( Serial serial )
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

	public class CrystalBullStatueDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalBullStatueAddon(); } }
		public override int LabelNumber { get { return 1076671; } } // Crystal Bull Statue

		[Constructable]
		public CrystalBullStatueDeed()
		{
			Hue = 1173;
		}

		public CrystalBullStatueDeed( Serial serial )
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