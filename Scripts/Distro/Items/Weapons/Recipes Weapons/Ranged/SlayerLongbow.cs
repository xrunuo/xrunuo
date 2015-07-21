using System;
using Server;

namespace Server.Items
{
	public class SlayerLongbow : ElvenCompositeLongBow
	{
		public override int LabelNumber { get { return 1073506; } } // Slayer Longbow

		[Constructable]
		public SlayerLongbow()
		{
			Slayer2 = BaseRunicTool.GetRandomSlayer();
		}

		public SlayerLongbow( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}