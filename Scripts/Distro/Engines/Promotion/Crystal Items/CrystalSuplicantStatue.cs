using System;
using Server;

namespace Server.Items
{
	public class CrystalSuplicantStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalSuplicantStatueDeed(); } }
		public override int LabelNumber { get { return 1076669; } } // Crystal SuplicantStatue

		[Constructable]
		public CrystalSuplicantStatueAddon()
		{
			AddComponent( new AddonComponent( 0x35FA ), 0, 0, 0 );
		}

		public CrystalSuplicantStatueAddon( Serial serial )
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

	public class CrystalSuplicantStatueDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalSuplicantStatueAddon(); } }
		public override int LabelNumber { get { return 1076669; } } // Crystal SuplicantStatue

		[Constructable]
		public CrystalSuplicantStatueDeed()
		{
			Hue = 1173;
		}

		public CrystalSuplicantStatueDeed( Serial serial )
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