using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class DaimyoHaochisKatana : Item
	{
		public override int LabelNumber { get { return 1063165; } } // Daimyo Haochi's Katana

		[Constructable]
		public DaimyoHaochisKatana()
			: base( 0x13FF )
		{
			Weight = 1.0;
		}

		public override bool DropToWorld( Mobile from, Point3D p )
		{
			Delete();

			return true;
		}

		public DaimyoHaochisKatana( Serial serial )
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
