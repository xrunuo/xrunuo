using System;
using Server;

namespace Server.Items
{
	public class AncestralShield : MetalKiteShield
	{
		public override int LabelNumber { get { return 1075308; } } // Ancestral Shield
		public override int QuestItemHue { get { return PrivateHue; } }

		[Constructable]
		public AncestralShield()
		{
			LootType = LootType.Blessed;
		}

		public AncestralShield( Serial serial )
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