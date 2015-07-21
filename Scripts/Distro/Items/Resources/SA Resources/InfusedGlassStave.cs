using System;
using Server;

namespace Server.Items
{
	public class InfusedGlassStave : GlassStaff
	{
		public override int LabelNumber { get { return 1112909; } } // infused glass stave

		[Constructable]
		public InfusedGlassStave()
		{
			Hue = 31;
		}

		public InfusedGlassStave( Serial serial )
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