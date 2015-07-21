using System;
using Server;

namespace Server.Items
{
	public class GoblinSlayingTalisman : BaseTalisman
	{
		public override int LabelNumber { get { return 1095011; } } // Talisman of Goblin Slaying

		[Constructable]
		public GoblinSlayingTalisman()
			: base( 0x1096 )
		{
			Weight = 1.0;
			TalisSlayer = TalisSlayerName.Goblin;
		}

		public GoblinSlayingTalisman( Serial serial )
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
