using System;
using Server;

namespace Server.Items
{
	public class DemonBridleRing : GoldRing
	{
		public override int LabelNumber { get { return 1113651; } } // Demon Bridle Ring

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DemonBridleRing()
		{
			Hue = 438;

			Attributes.RegenHits = 1;
			Attributes.RegenMana = 1;
			Attributes.DefendChance = 10;
			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 2;
			Attributes.LowerManaCost = 4;
			Resistances.Fire = 5;
		}

		public DemonBridleRing( Serial serial )
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
