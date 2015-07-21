using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class SamuraiTeleporter : SETeleporter
	{
		public override int LabelNumber { get { return 1049382; } } // a magical teleporter

		[Constructable]
		public SamuraiTeleporter()
		{
			Hue = 0xFA;
		}

		public override bool GetDestination( PlayerMobile player, ref Point3D loc, ref Map map )
		{
			loc = new Point3D( 384, 733, 0 );

			map = Map.Malas;

			return true;
		}

		public SamuraiTeleporter( Serial serial )
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
