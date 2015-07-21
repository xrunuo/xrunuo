using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests.MadScientist;

namespace Server.Items
{
	public class CompletedClockworkAssembly : TransientItem
	{
		public override int LabelNumber { get { return 1112879; } } // completed clockwork assembly

		[Constructable]
		public CompletedClockworkAssembly()
			: base( 0x1EAE, TimeSpan.FromMinutes( 10.0 ) )
		{
			Weight = 1.0;
		}

		public CompletedClockworkAssembly( Serial serial )
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
