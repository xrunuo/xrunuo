using System;
using Server;

namespace Server.Items
{
	public class NecklaceOfDiligence : GoldNecklace
	{
		public override int LabelNumber { get { return 1113137; } } // Necklace of Diligence

		[Constructable]
		public NecklaceOfDiligence()
		{
			Hue = 0x82;

			Attributes.BonusInt = 5;
			Attributes.RegenMana = 1;
		}

		public NecklaceOfDiligence( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}