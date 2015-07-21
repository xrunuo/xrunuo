using System;
using Server;

namespace Server.Items
{
	public class BurningAmber : GoldRing
	{
		public override int LabelNumber { get { return 1114790; } } // Burning Amber

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BurningAmber()
		{
			Hue = 0x496;

			Attributes.BonusDex = 5;
			Attributes.RegenMana = 2;
			Attributes.CastRecovery = 3;
			Resistances.Fire = 20;
		}

		public BurningAmber( Serial serial )
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
