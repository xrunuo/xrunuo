using System;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	public class ProtectorsEssence : Item
	{
		public override int LabelNumber { get { return 1073159; } } // Protector's Essence

		[Constructable]
		public ProtectorsEssence()
			: base( 0x1ED1 )
		{
		}

		public ProtectorsEssence( Serial serial )
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