using System;
using Server;

namespace Server.Items
{
	public class CrystalRunnerStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new CrystalRunnerStatueDeed(); } }
		public override int LabelNumber { get { return 1076670; } } // Crystal RunnerStatue

		[Constructable]
		public CrystalRunnerStatueAddon()
		{
			AddComponent( new AddonComponent( 0x35FC ), 0, 0, 0 );
		}

		public CrystalRunnerStatueAddon( Serial serial )
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

	public class CrystalRunnerStatueDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new CrystalRunnerStatueAddon(); } }
		public override int LabelNumber { get { return 1076670; } } // Crystal RunnerStatue

		[Constructable]
		public CrystalRunnerStatueDeed()
		{
			Hue = 1173;
		}

		public CrystalRunnerStatueDeed( Serial serial )
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